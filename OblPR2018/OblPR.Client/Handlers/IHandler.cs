using System.Net.Sockets;

namespace OblPR.Client
{
    public interface IHandler
    {
        void OnHandle(Socket socket);
    }
}