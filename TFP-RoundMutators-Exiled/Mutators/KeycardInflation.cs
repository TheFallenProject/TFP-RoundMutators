using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFP_RoundMutators_Exiled.Interfaces;
using MEC;

namespace TFP_RoundMutators_Exiled.Mutators
{
    internal class KeycardInflation : IMutator
    {
        public string Displayname { get; set; } = "<color=orange>Инфляция ключкарт.</color>";
        public string Description { get; set; } = "У <color=yellow>каждого человека</color> будет <b><color=#59f9e1>карта совета O5</color></b> на спавне.";

        public void Disengaged()
        {
            
        }

        public void Engaged()
        {
            Timing.CallDelayed(2f, () =>
            {
                foreach (var pl in Exiled.API.Features.Player.List)
                {
                    if (pl.Role.Team != Team.SCP)
                    pl.AddItem(ItemType.KeycardO5);
                }
            });
        }

        public bool DoIWantToEngage()
        {
            return true;
        }
    }
}
