using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Protocol
{
    public interface ICommandDispatcher
    {
        LoginHandler Dispatch(LoginPayload payload);
    }
}
