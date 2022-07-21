using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFP_RoundMutators_Exiled.Interfaces;
using MEC;
using System.Threading;

namespace TFP_RoundMutators_Exiled.Mutators
{
    internal class WarheadAlwaysEnabled : IMutator
    {
        public string Displayname { get; } = "<color=orange>Боеголовку заело.</color>";
        public string Description { get; } = "<color=red>Боеголовка</color> всегда <color=green>включена</color> и её <color=red>нельзя отключить</color> во время детонации!";
        public bool IsUnsafe => false;

        public void Disengaged()
        {
            Timing.KillCoroutines("warhead");
            Exiled.API.Features.Warhead.LeverStatus = false;
            Exiled.API.Features.Warhead.IsLocked = false;
        }

        public void Engaged()
        {
            Exiled.API.Features.Warhead.LeverStatus = true;
            Exiled.API.Features.Warhead.IsLocked = true;
            Timing.RunCoroutine(WarheadSwitchKeeper(), "warhead");
        }

        IEnumerator<float> WarheadSwitchKeeper()
        {
            while (true)
            {
                Exiled.API.Features.Warhead.LeverStatus = true;
                yield return Timing.WaitForSeconds(0.2f);
            }
        }
        public bool DoIWantToEngage()
        {
            return true;
        }
    }
}
