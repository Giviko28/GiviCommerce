using GiviCommerce.DataAccess.Data;
using GiviCommerce.DataAccess.Repository.IRepository;
using GiviCommerce.Models;
using GiviCommerce.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiviCommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (category.Name == category.DisplayOrder.ToString())
            {
                ModelState.AddModelError("", "Name and DisplayOrder cant be the same");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Save();
                TempData["Success"] = "Category created succesfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id is null || id <= 0)
            {
                return NotFound();
            }
            var category = _unitOfWork.Category.Get((c) => c.Id == id);
            if (category is null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Save();
                TempData["Success"] = "Category edited succesfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id is null || id <= 0)
            {
                return NotFound();
            }
            var category = _unitOfWork.Category.Get((c) => c.Id == id);
            if (category is null)
            {
                return NotFound();
            }


            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category obj = _unitOfWork.Category.Get(c => c.Id == id);
            if (obj is null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Category Deleted succesfully";
            return RedirectToAction("Index");
        }
    }
}
