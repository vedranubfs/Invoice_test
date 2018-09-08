using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //IInvoiceRepository InvoiceRepository { get; }
        void Save();
    }
}