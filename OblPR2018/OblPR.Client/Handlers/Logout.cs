using System;
using System.Net.Sockets;

namespace OblPR.Client
{
    public class Logout : IHandler
    {
        public void OnHandle(Socket socket)
        {
            Console.WriteLine("Logout correctly. Please login again or exit.");
        }
    }
}