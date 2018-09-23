using System;
using OblPR.Server;

namespace OblPR.Data.Services
{
    public class GameServer
    {
        private IUserManager _userManager;

        public GameServer(IUserManager userManager)
        {
            this._userManager = userManager;
        }

        public void StartServer(string ip, int port)
        {
            var server = new ClientListener(ip, port);
            server.Start();
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
                    case "c":
                        CreatePlayerMenu();
                        break;
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
            throw new NotImplementedException();
        }

        private void DisplayAllPlayersMenu()
        {
            throw new NotImplementedException();
        }

        private void DisplayConnectedPlayersMenu()
        {
            throw new NotImplementedException();
        }

        private void DisplayPosibleChoices()
        {
            Console.WriteLine("Welcome!");
            Console.WriteLine("Press c to create a new player");
            Console.WriteLine("Press s to show connected players");
            Console.WriteLine("Press a to show all players");
            Console.WriteLine("Press m to start a new match");
        }



        private void CreatePlayerMenu()
        {
            throw new NotImplementedException();
        }

        private string ReadChoice()
        {
            var choice = Console.ReadLine();
            return choice;
        }
    }
}
