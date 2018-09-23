using System.Collections.Generic;
using OblPR.Data.Entities;

namespace OblPR.Data.Services
{
    public interface IUserManager
    {
        void AddUser(User user);
        IEnumerator<User> getAllRegisteredUsers();
        IEnumerator<User> getAllActiveUsers();

    }
}