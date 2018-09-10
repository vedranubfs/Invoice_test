using Test.Data.Interfaces;
using Test.Database;

namespace Test.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private IRepository<Invoice> invoiceRepository;
        public IRepository<Invoice> InvoiceRepository
        {
            get
            {
                if(invoiceRepository == null)
                {
                    invoiceRepository = new Repository<Invoice>(this._context);
                }
                return invoiceRepository;
            }
        }

        private IRepository<Product> productRepository;
        public IRepository<Product> ProductRepository
        {
            get
            {
                if (productRepository == null)
                {
                    productRepository = new Repository<Product>(this._context);
                }
                return productRepository;
            }
        }

        private IRepository<Person> personRepository;
        public IRepository<Person> PersonRepository
        {
            get
            {
                if (personRepository == null)
                {
                    personRepository = new Repository<Person>(this._context);
                }
                return personRepository;
            }
        }

        private readonly InvoiceAppEntities _context;

        public UnitOfWork()
        {
        }

        public UnitOfWork(InvoiceAppEntities _context)
        {
            this._context = _context;
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