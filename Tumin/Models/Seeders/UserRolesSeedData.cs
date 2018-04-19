using Microsoft.AspNetCore.Identity;
using Tumin.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tumin.Models.Seeders
{
    public class UserRolesSeedData
    {
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRolesSeedData(UserManager<ApplicationUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager )
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;

        }

        public async Task SeedUsersRoles()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                var role = new IdentityRole() { Name = "Admin" };
                await _roleManager.CreateAsync(role);
            }
            if (!await _roleManager.RoleExistsAsync("User"))
            {
                var role = new IdentityRole() { Name = "User" };
                await _roleManager.CreateAsync(role);
            }
            if (!await _roleManager.RoleExistsAsync("business"))
            {
                var role = new IdentityRole() { Name = "business" };
                await _roleManager.CreateAsync(role);
            }



            var defaultUser = "yourMail@outlook.com";
            var user = await _userManager.FindByEmailAsync(defaultUser);
            if (user == null)
            {
                var newUser = new ApplicationUser()
                {
                    UserName = defaultUser,
                    Email = defaultUser,
                };


                string userPWD = "Q!w2e3r4";
                var chkUser = await _userManager.CreateAsync(newUser, userPWD);

                if (chkUser.Succeeded)
                {
                    var result1 = await _userManager.AddToRoleAsync(newUser, "Admin");
                    var admin = await _userManager.FindByEmailAsync(defaultUser);
                    var userInformation = new UserInformation()
                    {
                        UserId = new Guid(admin.Id),
                        FistName = "Admin",
                        LastName = "Default",
                        MothersLastName = "Default",
                        Curp = "qweqweqw",
                        State = 0
                    };
                    await _context.UserInformation.AddAsync(userInformation);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                if (!await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    var result1 = await _userManager.AddToRoleAsync(user, "Admin");
                }
            }


        }


    }

}
