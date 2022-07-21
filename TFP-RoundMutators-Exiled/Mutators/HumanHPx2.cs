using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFP_RoundMutators_Exiled.Interfaces;
using MEC;
using Exiled.API.Features;

namespace TFP_RoundMutators_Exiled.Mutators
{
    internal class HumanHPx2 : IMutator
    {
        public string Displayname { get; } = "<color=green>В здоровом теле - здоровый дух!</color>";
        public string Description { get; } = "У <color=green>людей</color> в <color=yellow>два раза</color> больше <color=green>хп</color>";

        public bool IsUnsafe => false;

        public void Disengaged()
        {
            Exiled.Events.Handlers.Player.ChangingRole -= roleChange;
        }

        public void Engaged()
        {
            Exiled.Events.Handlers.Player.ChangingRole += roleChange;
            Timing.CallDelayed(2f, () =>
            {
                foreach (var pl in Player.List)
                {
                    if (pl.Role.Team != Team.SCP)
                    {
                        pl.MaxHealth *= 2;
                        pl.Heal(690420, false);
                    }
                }
            });
        }

        static void roleChange(Exiled.Events.EventArgs.ChangingRoleEventArgs ev)
        {
            if (ev.NewRole != RoleType.Spectator)
            {
                Timing.CallDelayed(2f, () =>
                {
                    ev.Player.MaxHealth *= 2;
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
