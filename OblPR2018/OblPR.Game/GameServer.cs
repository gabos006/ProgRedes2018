using System;
using System.Collections.Generic;
using OblPR.Data.Entities;
using OblPR.Data.Services;
using OblPR.Game;

namespace OblPR.Game
{
    public class GameServer
    {
        private readonly IPlayerManager _playerManager;
        private readonly ILoginManager _loginManager;
        private readonly IGameMatchManager _matchManager;
        private readonly IActionLogger _logger;
        private readonly GameLogic _gameLogic;

        public GameServer(IPlayerManager playerManager, ILoginManager loginManager, IGameMatchManager matchManager, IActionLogger logger)
        {
            this._playerManager = playerManager;
            this._loginManager = loginManager;
            this._matchManager = matchManager;
            this._logger = logger;
            this._gameLogic = new GameLogic(_logger);
        }

        public void StartServer(string ip, int port)
        {
            var server = new ClientListener(_playerManager, _loginManager, _gameLogic);
            server.StartListening(ip, port);
            MainMenu();
        }

        private void MainMenu()
        {
            while (true)
            {
                DisplayPosibleChoices();
                var choice = ReadChoice();
                switch (choice)
                {
                    case "s":
                        DisplayConnectedPlayersMenu();
                        break;
                    case "a":
                        DisplayAllPlayersMenu();
                        break;
                    case "m":
                        CreateMatchMenu();
                        break;
                    default: break;
                }
            }
        }

        private void CreateMatchMenu()
        {
            Console.WriteLine("Match Started");
            _logger.Log("Match Started");
            var match = _gameLogic.CreateMatch();
            _matchManager.AddMatch(match);

        }

        private void DisplayAllPlayersMenu()
        {
            var list = _playerManager.GetAllRegisteredPlayers();
            foreach (var player in list)
            {
                Console.WriteLine(player);
            }
            Console.WriteLine("");

        }

        private void DisplayConnectedPlayersMenu()
        {
            var list = _playerManager.GetAllActivePlayers();
            foreach (var player in list)
            {
                Console.WriteLine(player);
            }
            Console.WriteLine("");
        }

        private void DisplayPosibleChoices()
        {
            Console.WriteLine("Welcome!");
            Console.WriteLine("Press s to show connected players");
            Console.WriteLine("Press a to show all players");
            Console.WriteLine("Press m to start a new match");
        }

        private string ReadChoice()
        {
            var choice = Console.ReadLine();
            return choice;
        }



    }

}
