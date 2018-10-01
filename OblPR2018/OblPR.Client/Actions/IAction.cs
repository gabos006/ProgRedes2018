using System.Net.Sockets;

namespace OblPR.Client
{
    public interface IAction
    {
       bool DoAction(Socket socket);
    }
}
