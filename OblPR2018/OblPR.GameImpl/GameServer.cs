using System;
using System.Collections.Generic;
using OblPR.Data.Services;
using OblPR.Server;

namespace OblPR.GameImpl
{
    public class GameServer
    {
        private readonly IPlayerManager _playerManager;
        private readonly ILoginManager _loginManager;
        private readonly GameController _gameController;

        public GameServer(IPlayerManager playerManager, ILoginManager loginManager)
        {
            this._playerManager = playerManager;
            this._loginManager = loginManager;
            this._gameController = new GameController();
        }

        public void StartServer(string ip, int port)
        {
            var server = new ClientListener(_playerManager, _loginManager, _gameController);
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
            _gameController.Start();
        }

        private void DisplayAllPlayersMenu()
        {
            var enumerator = _playerManager.GetAllRegisteredPlayers();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current);
            }
            Console.WriteLine("");

        }

        private void DisplayConnectedPlayersMenu()
        {
            var enumerator = _playerManager.GetAllActivePlayers();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current);
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
