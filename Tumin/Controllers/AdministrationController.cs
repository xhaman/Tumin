using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tumin.Data;
using Tumin.Models;

namespace Tumin.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdministrationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public  IActionResult Index()
        {

            var users = _context.UserRoles.Count(a=>a.RoleId==( _context.Roles.FirstOrDefault(b => b.Name == "User").Id));
            var admins = _context.UserRoles.Count(a => a.RoleId == (_context.Roles.FirstOrDefault(b => b.Name == "Admin").Id));
            var business = _context.UserRoles.Count(a => a.RoleId == (_context.Roles.FirstOrDefault(b => b.Name == "business").Id));

            //var admins = await _userManager.GetUsersInRoleAsync("Admin");

             ViewData["Users"] = users;
            ViewData["Administrators"] = admins;
            ViewData["Business"] = business;

            return View();
        }
    }
}