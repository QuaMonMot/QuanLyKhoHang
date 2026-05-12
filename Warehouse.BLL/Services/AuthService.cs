using Warehouse.BLL.Interfaces;
using Warehouse.DAL.Interfaces;
using Warehouse.Models;

namespace Warehouse.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(
            IAuthRepository authRepository
        )
        {
            _authRepository = authRepository;
        }

        // =========================
        // LOGIN
        // =========================
        public User? Login(
            string username,
            string password
        )
        {
            return _authRepository.Login(
                username,
                password
            );
        }

        // =========================
        // REGISTER
        // =========================
        public void Register(User user)
        {
            _authRepository.Register(user);
        }

        // =========================
        // UPDATE PROFILE
        // =========================
        public void UpdateProfile(int userId, string username, string password)
        {
            _authRepository.UpdateProfile(userId, username, password);
        }
    }
}