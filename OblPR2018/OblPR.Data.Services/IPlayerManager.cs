﻿using System.Collections.Generic;
using OblPR.Data.Entities;

namespace OblPR.Data.Services
{
    public interface IPlayerManager
    {
        void AddUser(Player player);
        IEnumerator<Player> GetAllRegisteredPlayers();
        IEnumerator<Player> GetAllActivePlayers();
    }
}