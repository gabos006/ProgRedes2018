using OblPR.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OblPR.Data.Services;

namespace OblPR.Protocol
{
    public class LoginHandler : IActionHandler<Player>
    {
        private ILoginManager _loginManager;
        private IPlayerManager _playerManager;

        public LoginHandler(ILoginManager loginManager, IPlayerManager playerManager)
        {
            this._loginManager = loginManager;
            this._playerManager = playerManager;
        }

        public Player Execute(AbstractPayload payload)
        {
            throw new NotImplementedException();
        }
    }
}
