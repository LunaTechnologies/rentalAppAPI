using rentalAppAPI.BLL.Interfaces;
using rentalAppAPI.DAL.Interfaces;
using rentalAppAPI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentalAppAPI.BLL.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IUserRepository _userRepo;
        public UserManager(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<Boolean> removeUser(string username)
        {
            return await _userRepo.removeUser(username);
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            return await _userRepo.GetAllUsers();
        }
    }
}
