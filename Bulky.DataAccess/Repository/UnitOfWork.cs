﻿using GiviCommerce.DataAccess.Data;
using GiviCommerce.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiviCommerce.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }


        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(db);
            Product = new ProductRepository(db);
            Company = new CompanyRepository(db);
        }


        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
