using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Protocol
{
    public interface IActionHandler<T>
    {
        T Execute(AbstractPayload payload);
    }
}
