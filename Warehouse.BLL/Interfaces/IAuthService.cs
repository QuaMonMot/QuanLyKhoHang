using Warehouse.Models;

namespace Warehouse.BLL.Interfaces
{
    public interface IAuthService
    {
        User? Login(string username, string password);

        void Register(User user);
        void UpdateProfile(int userId, string username, string password);
    }
}