using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Client
{
    public static class ClientCommand
    {

        public const int DISCONNECT = 0;
        public const int CONNECT = 1;
        public const int LOGIN = 1;
        public const int ADD_PLAYER = 2;
        public const int ACTIVE_GAME_MONSTER = 1;
        public const int ACTIVE_GAME_SURVIVOR = 2;
        public const int EXIT_GAME = 3;

        public const int MOVE = 1;
        public const int ATTACK = 2;
    }
}
