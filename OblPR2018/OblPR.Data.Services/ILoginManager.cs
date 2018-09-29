using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OblPR.Data.Entities;

namespace OblPR.Data.Services
{
    public interface ILoginManager
    {
        Player Login(string userName);
        void Logout(string userName);
    }
}
