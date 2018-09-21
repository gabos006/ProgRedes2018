using System;
using System.Collections.Generic;

namespace OblPR.Server
{
    public class UserHandler
    {
        private List<User> _activeUsers;

        public UserHandler()
        {
            this._activeUsers = new List<User>();
        }

        public IEnumerator<User> getActiveUsers()
        {
            return this._activeUsers.GetEnumerator();
        }

        public User Login(string userName)
        {
            throw new NotImplementedException();
        }
    }
}