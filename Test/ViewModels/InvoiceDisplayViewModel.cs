using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Test.Database;
using PagedList;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

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


        public int GetTotalItemsCount(Invoice invoice)
        {
            return invoice.InvoiceProducts.Count;
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