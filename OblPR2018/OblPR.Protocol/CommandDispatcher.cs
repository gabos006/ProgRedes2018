using OblPR.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OblPR.Protocol
{
    public class CommandDispatcher:ICommandDispatcher
    {
        private ILoginManager _loginManager;
        private IPlayerManager _playerManager;

        public CommandDispatcher(ILoginManager loginManager, IPlayerManager playerManager)
        {
            _loginManager = loginManager;
            _playerManager = playerManager;
        }

        public LoginHandler Dispatch(LoginPayload payload)
        {
            return new LoginHandler(_loginManager, _playerManager);
        }
    }
}
