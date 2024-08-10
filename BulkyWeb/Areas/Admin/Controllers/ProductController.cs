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

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;   
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category
                .GetAll()
                .Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });


            ProductVM productVM = new()
            {
                Product = new Product(),
                CategoryList = categoryList
            };

            if (id is null || id == 0)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file is not null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images/product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageURL))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageURL.TrimStart('/'));
                        
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream); 
                    }

                    productVM.Product.ImageURL = @"/images/product/" + fileName;
                }
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                _unitOfWork.Save();
                TempData["Success"] = "product created my boi";
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll()
                    .Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    });
                return View(productVM);
            }
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll(int id)
        {
            List<Product> products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = products });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Product productToDelete = _unitOfWork.Product.Get(c => c.Id == id);

            if (productToDelete is null)
            {
                return Json(new { sucess = false, message = "Error while deleting"}); 
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToDelete.ImageURL.TrimStart('/'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToDelete);
            _unitOfWork.Save();

            return Json(new { sucess = true, message = "Delete succesfull" });
        }
        #endregion
    }
}
