using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace TFP_RoundMutators
{
    public class Plugin : Qurre.Plugin
    {
        /*
            Created by Treeshold#0001 (aka Star Butterfly)
            Intended for The Fallen Project (SCPSL Server)
            For the sake of the god, do not claim this Plugin as your own!
        */

        #region overrides
        public override string Developer => "Treeshold (aka Star Butterfly)";
        public override string Name => "TFP-RoundMutators";

        #endregion

        public static string overridenMutator = "";
        static object ActiveMutator;

        static private Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return
              assembly.GetTypes()
                      .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                      .ToArray();
        }

        public static Config CustomConfig { get; set; }

        public override void Disable()
        {
            CustomConfigs.Clear();
            Qurre.Events.Round.Start -= RoundStarted;
            Qurre.Events.Round.End -= RoundEnded;
            Qurre.Events.Round.Restart -= RoundRestart;
            Qurre.Events.Round.Waiting -= WaitingForPlayers;
        }

        public override void Enable()
        {
            CustomConfig = new Config();
            CustomConfigs.Add(CustomConfig);
            Qurre.Events.Round.Start += RoundStarted;
            Qurre.Events.Round.End += RoundEnded;
            Qurre.Events.Round.Restart += RoundRestart;
            Qurre.Events.Round.Waiting += WaitingForPlayers;
        }

        static void RoundStarted()
        {
            Type mut;
            Type[] types = GetTypesInNamespace(Assembly.GetExecutingAssembly(), "TFP_RoundMutators.Mutators");
            object refereceObject = null;
            bool allClear = false; ;
            while (true)
            {
                int randsel = UnityEngine.Random.Range(0, types.Length - 1);
                mut = types[randsel];
                if (overridenMutator != "")
                {
                    mut = types.First(mutPred => mutPred.Name == overridenMutator);
                }
                Qurre.Log.Info($"Trying to engage {mut.Name}.");
                if (!mut.Name.Contains("<") && !mut.Name.Contains(">"))
                {
                    refereceObject = Activator.CreateInstance(mut);
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
                    if (response.Equals(true) && response != null)
                    {
                        Qurre.Log.Info("Yup, all clear! Engaging selected mutator!");
                        allClear = true;
                    }
                    else if (response == null)
                    {
                        Qurre.Log.Warn("While trying to get response from DoIWantToEngage we got null'ed! This is bad! Anyway, trying again!");
                    }
                    else
                    {
                        Qurre.Log.Warn("Mutator didn't want to be engaged. Whelp, trying again!");
                        overridenMutator = "";
                    }
                }
                else
                {
                    Qurre.Log.Warn("<>c was found in mutator's name. THIS IS AN ISSUE, RETRYING...");
                }
                if (!mut.Name.Contains("<>c") && allClear)
                {
                    break;
                }
            }
            Qurre.Log.Info($"Engaged mutator {mut.Name}.");
            var eng = mut.GetMethod("Engaged");
            eng.Invoke(refereceObject, null);
            
            Qurre.API.Map.ClearBroadcasts();
            Qurre.API.Map.Broadcast($"Был запущен мутатор <b>{mut.GetProperty("Displayname").GetValue(refereceObject)}</b>\n<size=20>{mut.GetProperty("Description").GetValue(refereceObject)}</size>", 20, true);

            ActiveMutator = refereceObject;
            overridenMutator = "";
        }

        static void RoundEnded(Qurre.API.Events.RoundEndEvent ev)
        {
            try
            {
                Qurre.Log.Info($"Disengaged mutator {ActiveMutator.GetType().Name}.");
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
                Qurre.Log.Info($"Disengaged mutator {ActiveMutator.GetType().Name}.");
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
                Qurre.Log.Info($"Disengaged mutator {ActiveMutator.GetType().Name}.");
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
