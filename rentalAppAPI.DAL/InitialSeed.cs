using Microsoft.AspNetCore.Identity;
using rentalAppAPI.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentalAppAPI.DAL
{
    public class InitialSeed
    {
        private readonly RoleManager<Role> _roleManager;
        public InitialSeed(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public async void CreateRoles()
        {
            string[] roleNames =
            {
                "Admin",
                "User"
            };

            foreach (var name in roleNames)
            {
                var role = new Role
                {
                    Name = name
                };

                _roleManager.CreateAsync(role).Wait();
            }
        }
    }
}
