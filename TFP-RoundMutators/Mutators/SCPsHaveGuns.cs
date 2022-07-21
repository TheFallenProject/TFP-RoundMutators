using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFP_RoundMutators.Interfaces;
using MEC;

namespace TFP_RoundMutators.Mutators
{
    internal class SCPsHaveGuns : IMutator
    {


        public string Displayname { get; set; } = "<color=red>Вооружённые SCP.</color>";
        public string Description { get; set; } = "У <color=red>SCP</color> есть <color=yellow>пистолеты</color>, но урон по ним <color=red>увеличен в 3 раза</color>.";

        public void Disengaged()
        {
            Qurre.Events.Player.DamageProcess -= DamageMoment;
        }

        public void Engaged()
        {
            Qurre.Events.Player.DamageProcess += DamageMoment;
            Timing.CallDelayed(2f, () =>
            {
                foreach (var pl in Qurre.API.Player.List)
                {
                    if (pl.Team == Team.SCP)
                    {
                        pl.AddItem(ItemType.GunCOM18);
                    }
                }
            });
        }

        void DamageMoment(Qurre.API.Events.DamageProcessEvent ev)
        {
            if (ev.Target.Team == Team.SCP)
            {
                ev.Amount *= 3;
            }
        }
        public bool DoIWantToEngage()
        {
            foreach (var pl in Qurre.API.Player.List)
            {
                if (pl.Team == Team.SCP)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
