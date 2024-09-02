using GiviCommerce.DataAccess.Data;
using GiviCommerce.DataAccess.Repository.IRepository;
using GiviCommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiviCommerce.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product obj)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == obj.Id);
            if (product is not null)
            {
                product.ISBN = obj.ISBN;
                product.ListPrice = obj.ListPrice;
                product.Price = obj.Price;
                product.Price100 = obj.Price100;
                product.Author = obj.Author;
                product.Description = obj.Description;
                product.CategoryId = obj.CategoryId;
                product.Price50 = obj.Price50;
                product.Title = obj.Title;
                product.ProductImages = obj.ProductImages;
                //if (obj.ImageURL is not null)
                //{
                //    product.ImageURL = obj.ImageURL;
                //}
            }
        }

    }
}
