using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Game
{
    public interface IClientHandler
    {
        void NotifyPlayerNear(string result);
        void NotifyMatchEnd(string result);
    }
}
