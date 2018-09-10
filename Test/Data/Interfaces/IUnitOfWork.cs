using System;
using Test.Database;

namespace Test.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Invoice> InvoiceRepository { get; }
        IRepository<Product> ProductRepository { get; }
        IRepository<Person> PersonRepository { get; }
        void Save();
    }
}