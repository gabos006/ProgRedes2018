using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Game
{
    public interface ICharacterHandler
    {
        void ExitMatch();
        void Attack();
        void Move(int x, int y);

    }
}
