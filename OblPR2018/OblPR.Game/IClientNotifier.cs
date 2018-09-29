using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Game
{
    public interface IClientNotifier
    {
        void NotifyPlayerNear();
        void NotifyDamageTaken();
        void NotifyMatchEnd();
    }
}
