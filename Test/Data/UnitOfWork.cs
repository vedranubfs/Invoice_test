using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Test.Data.Interfaces;
using Test.Database;

namespace Test.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public IRepository<Invoice> InvoiceRepository { get; private set; }
        public IRepository<Product> ProductRepository { get; private set; }
        public IRepository<Person> PersonRepository { get; private set; }



        private readonly InvoiceAppEntities _context;

        public UnitOfWork(InvoiceAppEntities _context)
        {
            this._context = _context;
            InvoiceRepository = new Repository<Invoice>(this._context);
            ProductRepository = new Repository<Product>(this._context);
            PersonRepository = new Repository<Person>(this._context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}