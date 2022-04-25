using Microsoft.EntityFrameworkCore;
using rentalAppAPI.DAL.Entities;
using rentalAppAPI.DAL.Interfaces;
using rentalAppAPI.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentalAppAPI.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserModel> toUserModel(User userEntity)
        {
            var userModel = new UserModel
            {
                UserName = userEntity.UserName,
                Email = userEntity.Email,
                ProfilePicturePath = userEntity.ProfilePicturePath,
                UserType = userEntity.UserType,
                Verified = userEntity.Verified
            };
            return userModel;
        }

        async Task<List<UserModel>> IUserRepository.GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            var list = new List<UserModel>();
            foreach (var user in users)
            {
                var userModel = await toUserModel(user);
                list.Add(userModel);
            }
            return list;
        }
    }
}
