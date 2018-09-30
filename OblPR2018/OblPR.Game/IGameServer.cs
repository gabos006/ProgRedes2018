using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OblPR.Data.Entities;

namespace OblPR.Game
{
    public interface IGameServer
    {
        ICharacterHandler JoinGame(IClientHandler handler, Character character);
    }
}
