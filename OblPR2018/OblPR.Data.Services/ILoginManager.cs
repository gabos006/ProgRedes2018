using OblPR.Data.Entities;

namespace OblPR.Data.Services
{
    public interface ILoginManager
    {
        Player Login(string userName);
        void Logout(string userName);
    }
}
