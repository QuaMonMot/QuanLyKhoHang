using Warehouse.Models;

namespace Warehouse.BLL.Interfaces
{
    public interface IAuthService
    {
        User? GetById(int id);
        User? Login(string username, string password);

        void Register(User user);
        void UpdateProfile(int userId, UpdateProfileDTO dto);
    }
}