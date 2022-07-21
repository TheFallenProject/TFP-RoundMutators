using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFP_RoundMutators_Exiled.Interfaces
{
    internal interface IMutator
    {
        /// <summary>
        /// Это название отображается в начале раунда
        /// </summary>
        string Displayname { get; }

        /// <summary>
        /// Это описание отображается в начале раунда
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Is mutator unsafe? (Can it break something?)
        /// </summary>
        bool IsUnsafe { get; }

        /// <summary>
        /// Этот метод вызывается, когда мутатор был выбран И вернул true в DoIWantToEngage
        /// </summary>
        void Engaged();

        /// <summary>
        /// Этот метод вызывается в [WaitingForPlayers, RoundEnded, RoundRestart]
        /// </summary>
        void Disengaged();

        /// <summary>
        /// Этот медод вызывается в начале раунда, 
        /// </summary>
        /// <returns></returns>
        bool DoIWantToEngage();
    }
}
