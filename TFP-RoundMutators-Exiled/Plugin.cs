using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using MEC;

namespace TFP_RoundMutators
{
    public class Plugin : Exiled.API.Features.Plugin<Config>
    {
        /*
            Created by Treeshold#0001 (aka Star Butterfly)
            Intended for The Fallen Project (SCPSL Server)
            For the sake of the god, do not claim this Plugin as your own!
        */

        #region overrides
        public override string Author => "Treeshold (aka Star Butterfly)";
        public override string Name => "TFP-RoundMutators";

        #endregion

        public static string overridenMutator = "";
        static object ActiveMutator;
        public static Config JOJO;

        static private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return
              assembly.GetTypes()
                      .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                      .ToArray();
        }

        public static Config CustomConfig { get; set; }

        public override void OnDisabled()
        {
            JOJO = null;
            Exiled.Events.Handlers.Server.RoundStarted -= RoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded -= RoundEnded;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= WaitingForPlayers;
        }

        public override void OnEnabled()
        {
            JOJO = Config;
            Exiled.Events.Handlers.Server.RoundStarted += RoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded += RoundEnded;
            Exiled.Events.Handlers.Server.WaitingForPlayers += WaitingForPlayers;
        }

        static void RoundStarted()
        {
            Exiled.API.Features.Map.ClearBroadcasts();
            Exiled.API.Features.Map.Broadcast(10, "Выбираю случайный мутатор на этот раунд...", Broadcast.BroadcastFlags.Normal, true);
            Timing.CallDelayed(5f, () =>
            {
                Type mut;
                Type[] types = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "TFP_RoundMutators_Exiled.Mutators");
                object refereceObject = null;
                bool allClear = false; ;
                if (UnityEngine.Random.Range(0, 11) < 6)
                {
                    Exiled.API.Features.Map.ClearBroadcasts();
                    Exiled.API.Features.Map.Broadcast(20, $"Был запущен мутатор <b>Обычный раунд</b>\n<size=20><color=gray>Мутатор не был выбран</color></size>", Broadcast.BroadcastFlags.Normal, true);
                }
                while (true)
                {
                    int randsel = UnityEngine.Random.Range(0, types.Length - 1);
                    mut = types[randsel];
                    if (overridenMutator != "")
                    {
                        mut = types.First(mutPred => mutPred.Name == overridenMutator);
                    }
                    Exiled.API.Features.Log.Info($"Trying to engage {mut.Name}.");
                    if (!mut.Name.Contains("<") && !mut.Name.Contains(">"))
                    {
                        refereceObject = Activator.CreateInstance(mut);
                        bool isSafe = false;
                        if ((bool)mut.GetField("IsUnsafe").GetValue(refereceObject) && !JOJO.UnsafeMode) //This is an explicit bool conversion. If it is not bool, r/funny exception will be raised!
                        {
                            Exiled.API.Features.Log.Warn("Mutator is unsafe. Config is against it!");
                        }
                        else
                        {
                            isSafe = true;
                        }
                        var DoWantEngage = mut.GetMethod("DoIWantToEngage");
                        object response;
                        if (DoWantEngage == null)
                        {
                            response = null;
                        }
                        else
                        {
                            response = DoWantEngage.Invoke(refereceObject, null);
                        }
                        if (response.Equals(true) && !(response is null) && isSafe)
                        {
                            Exiled.API.Features.Log.Info("Yup, all clear! Engaging selected mutator!");
                            allClear = true;
                        }
                        else if (!isSafe)
                        {
                            Exiled.API.Features.Log.Warn("Selected mutator is unsafe and config is against it!");
                        }
                        else if (response == null)
                        {
                            Exiled.API.Features.Log.Warn("While trying to get response from DoIWantToEngage we got null'ed! This is bad! Anyway, trying again!");
                        }
                        else
                        {
                            Exiled.API.Features.Log.Warn("Mutator didn't want to be engaged. Whelp, trying again!");
                            overridenMutator = "";
                        }
                    }
                    else
                    {
                        Exiled.API.Features.Log.Warn("<>c was found in mutator's name. THIS IS AN ISSUE, RETRYING...");
                    }
                    if (!mut.Name.Contains("<>c") && allClear)
                    {
                        break;
                    }
                }
                Exiled.API.Features.Log.Info($"Engaged mutator {mut.Name}.");
                try
                {
                    var eng = mut.GetMethod("Engaged");
                    eng.Invoke(refereceObject, null);
                }
                catch (Exception ex)
                {
                    Exiled.API.Features.Map.Broadcast(20, $"Не удалось запустить мутатор (возникло исключение). Больше инфы в консоли!", Broadcast.BroadcastFlags.Normal, true);
                    Exiled.API.Features.Log.Error(ex);
                    return;
                }

                Exiled.API.Features.Map.Broadcast(20, $"Был запущен мутатор <b>{mut.GetProperty("Displayname").GetValue(refereceObject)}</b>\n<size=20>{mut.GetProperty("Description").GetValue(refereceObject)}</size>", Broadcast.BroadcastFlags.Normal, true);

                ActiveMutator = refereceObject;
                overridenMutator = "";
            });
        }

        static void RoundEnded(Exiled.Events.EventArgs.RoundEndedEventArgs ev)
        {
            try
            {
                Exiled.API.Features.Log.Info($"Disengaged mutator {ActiveMutator.GetType().Name}.");
                var diseng = ActiveMutator.GetType().GetMethod("Disengaged");
                diseng.Invoke(ActiveMutator, null);
                ActiveMutator = null;
            }
            catch (Exception ex)
            {

            }
        }

        static void RoundRestart()
        {
            try
            {
                Exiled.API.Features.Log.Info($"Disengaged mutator {ActiveMutator.GetType().Name}.");
                var diseng = ActiveMutator.GetType().GetMethod("Disengaged");
                diseng.Invoke(ActiveMutator, null);
                ActiveMutator = null;
            }
            catch (Exception ex)
            {

            }
        }

        static void WaitingForPlayers()
        {
            try
            {
                Exiled.API.Features.Log.Info($"Disengaged mutator {ActiveMutator.GetType().Name}.");
                var diseng = ActiveMutator.GetType().GetMethod("Disengaged");
                diseng.Invoke(ActiveMutator, null);
                ActiveMutator = null;
            }
            catch (Exception ex)
            {

            }
        }
    }
}
