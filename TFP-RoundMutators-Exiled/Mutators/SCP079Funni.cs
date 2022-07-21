using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFP_RoundMutators_Exiled.Interfaces;
using MapGeneration.Distributors;
using Mirror;
using HarmonyLib;
using Qurre.API.Objects;
using UnityEngine;
using Interactables.Interobjects;
using InventorySystem.Items.Firearms.Attachments;
using Exiled.Events.EventArgs;

namespace TFP_RoundMutators_Exiled.Mutators
{
    internal class SCP079Funni : IMutator
    {
        List<int> GensImmunityTemp = new List<int>();

        public string Displayname { get; } = "<color=orange>4 гига, 4 ядра, игровая видеокарта</color>";
        public string Description { get; } = "У <color=red>SCP-079</color> <color=green><b>ЭНЕРГИЯ БЕСКОНЕЧНА</b></color> <color=grey>(однако блокировка дверей всё ещё будет расходовать её)</color>, но <color=red>все генераторы запускаются за 40 секунд</color>. <color=grey>(к слову, комп есть =))</color>";

        public bool IsUnsafe => false;

        public void Disengaged()
        {
            Exiled.Events.Handlers.Scp079.ChangingCamera -= OnChangeCamera;
            Exiled.Events.Handlers.Scp079.InteractingTesla -= OnTriggeringTesla;
            Exiled.Events.Handlers.Scp079.TriggeringDoor -= OnTrigerringDoor;
            Exiled.Events.Handlers.Scp079.StartingSpeaker -= OnStartingSpeaker;
            Exiled.Events.Handlers.Scp079.ElevatorTeleporting -= OnTriggeringElevator;
        }

        public void Engaged()
        {
            Exiled.Events.Handlers.Scp079.ChangingCamera += OnChangeCamera;
            Exiled.Events.Handlers.Scp079.InteractingTesla += OnTriggeringTesla;
            Exiled.Events.Handlers.Scp079.TriggeringDoor += OnTrigerringDoor;
            Exiled.Events.Handlers.Scp079.StartingSpeaker += OnStartingSpeaker;
            Exiled.Events.Handlers.Scp079.ElevatorTeleporting += OnTriggeringElevator;

            foreach (var gen in Exiled.API.Features.Generator.List)
            {
                gen.ActivationTime = 40f;
            }
            /*
            Create(new UnityEngine.Vector3(107.2f, 992.9f, -48.3f), UnityEngine.Quaternion.LookRotation(new UnityEngine.Vector3(107.2f, 992.1f, -60.5f) - new UnityEngine.Vector3(107.2f, 992.1f, -48.3f)));
            Create(new UnityEngine.Vector3(62.0f, 987.1f, -64.2f), UnityEngine.Quaternion.LookRotation(new UnityEngine.Vector3(62.0f, 987.1f, -63.7f) - new UnityEngine.Vector3(62.0f, 987.1f, -64.2f)));
            Create(new UnityEngine.Vector3(-25.2f, 1000.6f, -70.1f), UnityEngine.Quaternion.LookRotation(new UnityEngine.Vector3(10.0f, 1001.3f, -70.1f) - new UnityEngine.Vector3(-24.7f, 1001.3f, -70.1f)));
            */
        }

        public bool DoIWantToEngage()
        {
            if (Exiled.API.Features.Player.List.Any(pl => pl.Role.Type == RoleType.Scp079))
                return true;
            return false;
        }

        void OnChangeCamera(ChangingCameraEventArgs ev) 
        {
            ev.AuxiliaryPowerCost = 0f;
        }

        void OnTriggeringTesla(InteractingTeslaEventArgs ev)
        {
            ev.AuxiliaryPowerCost = 0f;
        }

        void OnTrigerringDoor(TriggeringDoorEventArgs ev)
        {
            ev.AuxiliaryPowerCost = 0f;
        }

        void OnTriggeringElevator(ElevatorTeleportingEventArgs ev)
        {
            ev.AuxiliaryPowerCost = 0f;
        }

        void OnStartingSpeaker(StartingSpeakerEventArgs ev)
        {
            ev.AuxiliaryPowerCost = 0f;
        }
	}
}

