using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFP_RoundMutators.Interfaces;
using MEC;
using System.Threading;

namespace TFP_RoundMutators.Mutators
{
    internal class WarheadAlwaysEnabled : IMutator
    {
        public string Displayname { get; set; } = "<color=orange>Боеголовку заело.</color>";
        public string Description { get; set; } = "<color=red>Боеголовка</color> всегда <color=green>включена</color> и её <color=red>нельзя отключить</color> во время детонации!";

        public void Disengaged()
        {
            Timing.KillCoroutines("warhead");
            Qurre.API.Controllers.Alpha.Enabled = false;
            Qurre.API.Controllers.Alpha.Locked = false;
        }

        public void Engaged()
        {
            
            Qurre.API.Controllers.Alpha.Enabled = true;
            Qurre.API.Controllers.Alpha.Locked = true;
            Timing.RunCoroutine(WarheadSwitchKeeper(), "warhead");
        }

        IEnumerator<float> WarheadSwitchKeeper()
        {
            while (true)
            {
                Qurre.API.Controllers.Alpha.Enabled = true;
                yield return Timing.WaitForSeconds(0.2f);
            }
        }
        public bool DoIWantToEngage()
        {
            return true;
        }
    }
}
