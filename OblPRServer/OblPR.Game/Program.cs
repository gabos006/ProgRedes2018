using OblPR.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Game
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameServer = new GameServer(null, null);
            gameServer.StartServer("127.0.0.1", 4000);
        }
    }
}
