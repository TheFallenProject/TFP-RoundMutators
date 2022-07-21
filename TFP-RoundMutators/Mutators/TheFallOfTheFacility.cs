using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFP_RoundMutators.Interfaces;
using MEC;

namespace TFP_RoundMutators.Mutators
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
                yield return Timing.WaitForSeconds(UnityEngine.Random.Range(30f, 45f) + (UnityEngine.Random.Range(30f, 45f) * Qurre.API.Map.Generators.Count(gen => gen.Engaged)));
                Qurre.API.Controllers.Cassie.Send(".g1 .g1 .g1", false, false, true);
                Qurre.API.Controllers.Lights.TurnOff(25f);
            }
        }

        IEnumerator<float> DoorFailure()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(UnityEngine.Random.Range(1f, 4f) + (UnityEngine.Random.Range(2f, 5f) * Qurre.API.Map.Generators.Count(gen => gen.Engaged)));
                int randsel = UnityEngine.Random.Range(0, Qurre.API.Map.Doors.Count);
                var door = Qurre.API.Map.Doors.ElementAt(randsel);
                door.Open = !door.Open;
            }
        }

        public bool DoIWantToEngage()
        {
            return true;
        }

    }
}
