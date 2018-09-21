using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new Server("127.0.0.1", 4000);
            server.Start();
            while (true)
            {
                
            }
        }
    }
}
