using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFP_RoundMutators_Exiled.Interfaces;
using MEC;

namespace TFP_RoundMutators_Exiled.Mutators
{
    internal class SCPsHaveGuns : IMutator
    {
        public string Displayname { get; set; } = "<color=red>Вооружённые SCP.</color>";
        public string Description { get; set; } = "У <color=red>SCP</color> есть <color=yellow>пистолеты</color> <color=gray>(их урон снижен в 3 раза и всего 50 патронов в инвентаре)</color>, но урон по ним <color=red>увеличен в 3 раза</color>.";

        public void Disengaged()
        {
            Exiled.Events.Handlers.Player.Hurting -= DamageMoment;
        }

        public void Engaged()
        {
            Exiled.Events.Handlers.Player.Hurting += DamageMoment;
            Timing.CallDelayed(2f, () =>
            {
                foreach (var pl in Exiled.API.Features.Player.List)
                {
                    if (pl.Role.Team == Team.SCP)
                    {
                        pl.AddItem(ItemType.GunCOM18);
                        pl.Ammo[ItemType.Ammo9x19] = 50;
                    }
                }
            });
        }

        void DamageMoment(Exiled.Events.EventArgs.HurtingEventArgs ev)
        {
            if (ev.Target.Role.Team == Team.SCP)
            {
                ev.Amount *= 3;
            }
            if (ev.Attacker.Role.Team == Team.SCP && !(ev.Handler.Type == Exiled.API.Enums.DamageType.Scp))
            {
                ev.Amount /= 3;
            }
        }

        public bool DoIWantToEngage()
        {
            foreach (var pl in Exiled.API.Features.Player.List)
            {
                if (pl.Role.Team == Team.SCP)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
