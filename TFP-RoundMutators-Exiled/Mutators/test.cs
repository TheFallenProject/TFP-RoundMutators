using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFP_RoundMutators_Exiled.Mutators
{
    internal class test : TFP_RoundMutators_Exiled.Interfaces.IMutator
    {
        public string Displayname { get; set; } = "<color=white>Тествовый мутатор</color>";
        public string Description { get; set; } = "Это тестовый мутатор";

        public void Disengaged()
        {
            /*
             * Этот код выполняется, когда происходит:
             *  Рестарт раунда
             *  Конец раунда
             *  Ожидание игроков
             */
        }

        public bool DoIWantToEngage()
        {
            /*
             * Здесь можно проверить некоторые условия, при котором мутатор может быть проигнорирован.
             * Например, у нас мутатор меняет фичи у компа, но его может не быть.
             * Вот тут можно проверить условие и вернуть `true`, если вы хотите чтобы мутатор был запущен
             * или `false`, если вы хотите, чтобы мутатор был скипнут
             */
            return false;
        }

        public void Engaged()
        {
            /*
             * Этот код выполняется, когда происходит:
             *  Начало раунда
             */
        }
    }
}
