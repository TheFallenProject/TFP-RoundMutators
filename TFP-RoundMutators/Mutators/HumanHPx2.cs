using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFP_RoundMutators.Interfaces;
using MEC;

namespace TFP_RoundMutators.Mutators
{
    internal class HumanHPx2 : IMutator
    {
        public string Displayname { get; set; } = "<color=green>В здоровом теле - здоровый дух!</color>";
        public string Description { get; set; } = "У <color=green>людей</color> в <color=yellow>два раза</color> больше <color=green>хп</color>";

        public void Disengaged()
        {
            Qurre.Events.Player.RoleChange -= roleChange;
        }

        public void Engaged()
        {
            Qurre.Events.Player.RoleChange += roleChange;
            Timing.CallDelayed(2f, () =>
            {
                foreach (var pl in Qurre.API.Player.List)
                {
                    if (pl.Team != Team.SCP)
                    {
                        pl.MaxHp *= 2;
                        pl.Heal(690420, false);
                    }
                }
            });
        }

        static void roleChange(Qurre.API.Events.RoleChangeEvent ev)
        {
            if (ev.NewRole != RoleType.Spectator)
            {
                Timing.CallDelayed(2f, () =>
                {
                    ev.Player.MaxHp *= 2;
                    ev.Player.Heal(690420, false);
                });
            }
        }

        public bool DoIWantToEngage()
        {
            return true;
        }
    }
}
