using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Server
{
    public interface IClientNotifier
    {
        void NotifyPlayerNear();
        void NotifyDamageTaken();
        void NotifyMatchEnd();
    }
}
