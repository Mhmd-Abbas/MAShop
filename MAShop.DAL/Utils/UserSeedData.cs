using MAShop.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAShop.DAL.Utils
{
    public class UserSeedData : ISeedData
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSeedData(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task DataSeed()
        {
            if(!await _userManager.Users.AnyAsync())
            {
                var user1 = new ApplicationUser 
                {
                    UserName = "Mohammad",
                    Email = "MA@gmail.com",
                    FullName = "Mohammad Abbas",
                    EmailConfirmed = true
                };
                var user2 = new ApplicationUser
                {
                    UserName = "Ahmad",
                    Email = "AA@gmail.com",
                    FullName = "Ahmad Abbas",
                    EmailConfirmed = true
                };
                var user3 = new ApplicationUser
                {
                    UserName = "Samer",
                    Email = "SA@gmail.com",
                    FullName = "Samer Abbas",
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(user1,"Pass@123");
                await _userManager.CreateAsync(user2,"Pass@123");
                await _userManager.CreateAsync(user3,"Pass@123");

                await _userManager.AddToRoleAsync(user1, "SuperAdmin");
                await _userManager.AddToRoleAsync(user2, "Admin");
                await _userManager.AddToRoleAsync(user3, "User");
            }
            
        }
    }
}
