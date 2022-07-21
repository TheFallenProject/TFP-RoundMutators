using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFP_RoundMutators_Exiled.Interfaces;
using MEC;

namespace TFP_RoundMutators_Exiled.Mutators
{
    internal class TheFallOfTheFacility : IMutator
    {
        public string Displayname { get; set; } = "<color=orange>Перебои в работе комплекса.</color>";
        public string Description { get; set; } = "Системы комплекса <color=red>отказывают</color>. <color=blue>Двери</color> <color=yellow>открываются и закрываются</color> сами по себе, <color=blue>свет</color> <color=yellow>выключается</color> сам по себе. Запуск генераторов <color=green>может ослабить</color> неисправности комплекса.";
        public void Engaged()
        {
            Timing.RunCoroutine(LightFailure(), "lights");
            Timing.RunCoroutine(DoorFailure(), "doors");
        }

        public void Disengaged()
        {
            Timing.KillCoroutines("lights");
            Timing.KillCoroutines("doors");
        }

        IEnumerator<float> LightFailure()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(UnityEngine.Random.Range(30f, 45f) + (UnityEngine.Random.Range(30f, 45f) * Exiled.API.Features.Generator.List.Count(gen => gen.IsEngaged)));
                Exiled.API.Features.Cassie.Message(".g1 .g1 .g1", false, false, true);
                Exiled.API.Features.Map.TurnOffAllLights(25f);
            }
        }

        IEnumerator<float> DoorFailure()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(UnityEngine.Random.Range(0.5f, 2f) + (UnityEngine.Random.Range(2f, 5f) * Exiled.API.Features.Generator.List.Count(gen => gen.IsEngaged)));
                int randsel = UnityEngine.Random.Range(0, Exiled.API.Features.Door.List.Count());
                var door = Exiled.API.Features.Door.List.ElementAt(randsel);
                door.IsOpen = !door.IsOpen;
            }
        }

        public bool DoIWantToEngage()
        {
            return true;
        }

    }
}
