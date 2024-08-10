using GiviCommerce.DataAccess.Repository.IRepository;
using GiviCommerce.Models;
using GiviCommerce.Models.ViewModel;
using GiviCommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace GiviCommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]

    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;   
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {

            if (id is null || id == 0)
            {
                return View(new Company());
            }
            else
            {
                Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
            if (ModelState.IsValid)
            {
                if (companyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(companyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(companyObj);
                }
                _unitOfWork.Save();
                TempData["Success"] = "CompanyController created my boi";
                return RedirectToAction("Index");
            }
            else
            {
                return View(companyObj);
            }
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> companies = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = companies });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToDelete = _unitOfWork.Company.Get(c => c.Id == id);

            if (companyToDelete is null)
            {
                return Json(new { sucess = false, message = "Error while deleting"}); 
            }


            _unitOfWork.Company.Remove(companyToDelete);
            _unitOfWork.Save();

            return Json(new { sucess = true, message = "Delete succesfull" });
        }
        #endregion
    }
}
