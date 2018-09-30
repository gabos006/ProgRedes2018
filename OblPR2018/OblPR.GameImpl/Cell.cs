using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OblPR.Game;

namespace OblPR.GameImpl
{
    public class Cell
    {
        public int CoordX { get; private set; }
        public int CoordY { get; private set; }
        public ICharacterHandler Handler { get;  set; }

        public Cell(int x, int y)
        {
            CoordX = x;
            CoordY = y;
        }

    }
}
