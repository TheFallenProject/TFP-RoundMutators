using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFP_RoundMutators.Interfaces;
using Mirror;
using HarmonyLib;

namespace TFP_RoundMutators.Mutators
{
    internal class SCP079Funni : IMutator
    {
        List<int> GensImmunityTemp = new List<int>();

        public string Displayname { get; set; } = "<color=orange>4 гига, 4 ядра, игровая видеокарта!</color>";
        public string Description { get; set; } = "У <color=red>SCP-079</color> <color=green><b>ЭНЕРГИЯ БЕСКОНЕЧНА</b></color>, но <color=red>все генераторы на улице</color>. <color=grey>(к слову, комп есть =))</color>";

        public void Disengaged()
        {

        }

        public void Engaged()
        {
            foreach (var pl in Qurre.API.Player.List)
            {
                if (pl.Role == RoleType.Scp079)
                {
                    pl.BypassMode = true;
                }
            }
            foreach (var gen in Qurre.API.Map.Generators)
            {
                NetworkServer.Destroy(gen.GameObject);
            }
            Qurre.API.Map.Generators.Clear();
            Qurre.API.Controllers.Generator.Create(new UnityEngine.Vector3(107.2f, 992.9f, -48.3f), UnityEngine.Quaternion.LookRotation(new UnityEngine.Vector3(107.2f, 992.1f, -60.5f) - new UnityEngine.Vector3(107.2f, 992.1f, -48.3f)));
            Qurre.API.Controllers.Generator.Create(new UnityEngine.Vector3(62.0f, 987.1f, -64.2f), UnityEngine.Quaternion.LookRotation(new UnityEngine.Vector3(62.0f, 987.1f, -63.7f) - new UnityEngine.Vector3(62.0f, 987.1f, -64.2f)));
            Qurre.API.Controllers.Generator.Create(new UnityEngine.Vector3(-25.2f, 1000.6f, -70.1f), UnityEngine.Quaternion.LookRotation(new UnityEngine.Vector3(10.0f, 1001.3f, -70.1f) - new UnityEngine.Vector3(-24.7f, 1001.3f, -70.1f)));
        }

        public bool DoIWantToEngage()
        {
            if (Qurre.API.Player.List.Any(pl => pl.Role == RoleType.Scp079))
            {
                return true;
            }
            return false;
        }
    }
}
