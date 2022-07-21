using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFP_RoundMutators_Exiled.Interfaces;

namespace TFP_RoundMutators_Exiled.Mutators
{
    internal class AssaultMoment : IMutator
    {
        public string Displayname { get; } = "<color=orange>Ох уж эти туториалы!</color>";
        public string Description { get; } = "В случае респавна MTF или Хаоса приедут туториалы. Они нейтральны ко всем.";
        public bool IsUnsafe { get; } = true;

        public void Disengaged()
        {
            Exiled.Events.Handlers.Server.RespawningTeam -= TeamRespawn;
        }

        public bool DoIWantToEngage()
        {
            return true;
        }

        public void Engaged()
        {
            Exiled.Events.Handlers.Server.RespawningTeam += TeamRespawn;
        }

        void TeamRespawn(Exiled.Events.EventArgs.RespawningTeamEventArgs ev)
        {
            ev.IsAllowed = false;
            Exiled.API.Features.Cassie.MessageTranslated($"MTFUnit Error Error Designated Error Error HasEntered AllRemaining {(Exiled.API.Features.Player.List.Count(pl => pl.Role.Team == Team.SCP) > 0 ? $"AwaitingRecontainment {Exiled.API.Features.Player.List.Count(pl => pl.Role.Team == Team.SCP)} ScpSubjects" : $"NoSCPsLeft")}",
                $"Мобильная оперативная группа *ошибка*-*ошибка* обозначенная как *ошибка*-*ошибка* прибыла в комплекс. Всему персоналу рекомендуется следовать стандартным протоколам эвакуации, пока опер-группа не зачистит комплекс. {(Exiled.API.Features.Player.List.Count(pl => pl.Role.Team == Team.SCP) > 0 ? $"Ожидается сдерживание {Exiled.API.Features.Player.List.Count(pl => pl.Role.Team == Team.SCP)} SCP Объектов." : $"Угроза сохраняется на территории комплекса. Продолжайте с осторожностью.")}");
            foreach (var pl in Exiled.API.Features.Player.List)
            {
                if (pl.Role.Type == RoleType.Spectator)
                {
                    pl.SetRole(RoleType.Tutorial, Exiled.API.Enums.SpawnReason.Revived);
                    pl.EnableEffect(new CustomPlayerEffects.Flashed(), 3);
                    Timing.CallDelayed(3f, () => {
                        pl.Position = new UnityEngine.Vector3(86.7f, 988.5f, -68.0f);

                        pl.AddItem(ItemType.ArmorCombat);
                        pl.AddItem(ItemType.GrenadeHE);
                        pl.AddItem(ItemType.Radio);
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.Medkit);
                        pl.AddItem(ItemType.KeycardNTFOfficer);
                        pl.AddItem(ItemType.GunE11SR); });
                }
            }
        }
    }
}
