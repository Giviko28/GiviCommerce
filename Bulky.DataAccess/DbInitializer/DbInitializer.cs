using GiviCommerce.DataAccess.Data;
using GiviCommerce.DataAccess.Repository.IRepository;
using GiviCommerce.Models;
using GiviCommerce.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiviCommerce.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count () > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception e)
            {

                throw;
            }
            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole { Name = SD.Role_Customer }).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole { Name = SD.Role_Employee }).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole { Name = SD.Role_Admin }).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole { Name = SD.Role_Company }).GetAwaiter().GetResult();
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admini@gmail.com",
                    Email = "admini@gmail.com",
                    Name = "Givie",
                    PhoneNumber = "995598753355",
                    StreetAddress = "Telasi",
                    State = "IL",
                    PostalCode = "0460",
                    City = "Chicago"
                }, "Admini123!").GetAwaiter().GetResult();

                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admini@gmail.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }


        }
    }
}
