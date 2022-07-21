using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFP_RoundMutators_Exiled.Interfaces;
using MEC;

namespace TFP_RoundMutators_Exiled.Mutators
{
    internal class PassiveRegen : IMutator
    {
        public string Displayname { get; } = "<color=green>Пассивная регенерация</color>";
        public string Description { get; } = "У <color=yellow>всех людей</color> хп <color=green>будет восстанавливаться</color> по <color=yellow>2 хп/секунду</color>";

        public bool IsUnsafe => false;

        public void Disengaged()
        {
            Timing.KillCoroutines("heal");
        }

        public bool DoIWantToEngage()
        {
            return true;
        }

        public void Engaged()
        {
            Timing.RunCoroutine(humanHeal(), "heal");
        }

        IEnumerator<float> humanHeal()
        {
            while (true)
            {
                foreach (var pl in Exiled.API.Features.Player.List)
                {
                    if (pl.Role.Team != Team.SCP)
                    {
                        pl.Heal(1f, false);
                    }
                }
                yield return Timing.WaitForSeconds(0.5f);
            }
        }
    }
}
