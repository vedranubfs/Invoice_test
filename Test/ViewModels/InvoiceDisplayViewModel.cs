using System.Collections.Generic;
using System.Linq;
using Test.Database;
using PagedList;
using System.Web.Mvc;

namespace Test.ViewModels
{
    public class InvoiceDisplayViewModel
    {
        public PagedList<Invoice> InvoicesList { get; set; }

        public Invoice Invoice { get; set; }

        public string SearchType { get; set; }
        public string SearchString { get; set; }

        public static List<SelectListItem> SearchTypes = new List<SelectListItem>()
        {
            new SelectListItem() {Text = "Company Name", Value = "Company"},
            new SelectListItem() {Text = "Invoice Payment Deadline", Value = "PaymentDeadline"},
            new SelectListItem() {Text = "Invoice Creation Date", Value = "CreationDate"}
        };


        public string GetTotalItemsCount(Invoice invoice)
        {
            string tmp = "";
            if (invoice.IsProductReturned)
            {
                tmp = ($"-{invoice.InvoiceProducts.Count}");
            }
            else
            {
                tmp = invoice.InvoiceProducts.Count.ToString();
            }
            return tmp;
        }

        public decimal GetTotalPrice(Invoice invoice)
        {
            decimal price = 0;
            var tmpList = invoice.InvoiceProducts.ToList();
            for (int i = 0; i < tmpList.Count; i++)
            {
                price += tmpList[i].Product.Price * tmpList[i].Quantity; // todo add qty on invoiceProduct table level
            }
            return price;
        }

        public decimal GetTotalPriceForSingleProduct(Product product,int qty)
        {
            return product.Price * qty;
        }
    }
}