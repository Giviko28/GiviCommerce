using GiviCommerce.DataAccess.Data;
using GiviCommerce.DataAccess.Repository.IRepository;
using GiviCommerce.Models;
using GiviCommerce.Models.ViewModel;
using GiviCommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace GiviCommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;   
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RoleManagement(string userId)
        {
            var applicationUser = _db.ApplicationUsers
                .Include(au => au.Company)
                .FirstOrDefault(au => au.Id == userId);
            var roleId = _db.UserRoles.FirstOrDefault(ur => ur.UserId == applicationUser.Id).RoleId;
            var userRole = _db.Roles.FirstOrDefault(r => r.Id == roleId);
            applicationUser.Role = userRole.Name;
            var companies = _db.Companies.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            var roles = _db.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id.ToString()
            });
            RoleManagementVM roleManagementVM = new()
            {
                ApplicationUser = applicationUser,
                Companies = companies,
                Roles = roles,
                RoleId = userRole.Id
            };

            return View(roleManagementVM);
        }

        [HttpPost]
        public IActionResult RoleManagement(RoleManagementVM roleManagementVM)
        {
            var role = _db.Roles.FirstOrDefault(r => r.Id == roleManagementVM.RoleId);
            var userToUpdate = _db.ApplicationUsers.FirstOrDefault(au => au.Id == roleManagementVM.ApplicationUser.Id);

            var userRole = _db.UserRoles.FirstOrDefault(ur => ur.UserId == userToUpdate.Id);
            _db.UserRoles.Remove(userRole);

            _userManager.AddToRoleAsync(userToUpdate, role.Name).GetAwaiter().GetResult();


            if (role.Name != "Company")
            {
                userToUpdate.CompanyId = null;
            }
            else
            {
                userToUpdate.CompanyId = roleManagementVM.CompanyId;
            }

            _db.UpdateRange(userToUpdate, userRole);
            _db.SaveChanges();


            return RedirectToAction(nameof(Index));
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ApplicationUser> objUserList = _db.ApplicationUsers.Include(u => u.Company).ToList();

            var userRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach (var user in objUserList)
            {
                var roleId = userRoles.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(r => r.Id == roleId).Name;


                if (user.Company is null)
                {
                    user.Company = new Company { Name = "" };
                }
            }


            return Json(new { data = objUserList });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody]string id)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if (objFromDb is null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }

            if (objFromDb.LockoutEnd is not null && objFromDb.LockoutEnd > DateTime.Now)
            {
                objFromDb.LockoutEnd = DateTime.Now;
            }
            else
            {
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            }

            _db.SaveChanges();

            return Json(new { success = true, message = "Lock/Unlock succesfull" });
        }
        #endregion
    }
}
