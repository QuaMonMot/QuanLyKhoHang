using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Warehouse.Models;

namespace Warehouse.DAL.Interfaces
{
    public interface IAuthRepository
    {
        User Login(string username, string password);
        void Register(User user);
        void UpdateProfile(int userId, string username, string password);
    };

}
