using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Test.Database;
namespace Test.ViewModels
{
    public class InvoiceAddViewModel
    {
        public List<Person> Persons { get; set; }
        public List<Product> Products { get; set; }

        [Display(Name = "Selected person")]
        public int SelectedPersonId { get; set; } = -1;

        [MinLength(1)]
        [Required(ErrorMessage = "You need to select at least one product")]
        public int[] SelectedProductsIds { get; set; }

        public int[] SelectedProductsQty { get; set; }

        public Person SelectedPerson { get; set; }

        [Range(1,1000, ErrorMessage = "You need to set payment day for this company")]
        public int PaymentDay { get; set; } = 0;

        public bool IsProductReturned { get; set; }

        public InvoiceAddViewModel()
        {

        }

        public InvoiceAddViewModel(List<Person> persons,List<Product> products)
        {
            this.Persons = persons;
            this.Products = products;
        }

        public IEnumerable<SelectListItem> GetPersons {
            get { return new SelectList(Persons,"Id","Name"); }
        }

        public void AddToProductsList(Product p)
        {
            if (Products == null)
                Products = new List<Product>();

            Products.Add(p);
        }

        public decimal GetProductTotalPrice(decimal price,int qty)
        {
            return price * qty;
        }
    }
}