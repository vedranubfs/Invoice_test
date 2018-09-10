using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Test.Database;
using Test.ViewModels;
using PagedList;
using Test.Data.Interfaces;

namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private IUnitOfWork unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ActionResult Index(int pageIndex = 1, string searchType = "", string searchString = "")
        {
            List<Invoice> invoices = unitOfWork.InvoiceRepository.GetAll().OrderBy(g => g.PaymentDay).ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                DateTime date;

                switch (searchType)
                {
                    case "Company":
                        invoices = unitOfWork.InvoiceRepository.Find(g => g.Person.Name.Contains(searchString)).OrderByDescending(i => i.Person.Name).ToList();
                        break;
                    case "PaymentDeadline":
                        if (DateTime.TryParse(searchString, out date))
                        {
                            invoices = unitOfWork.InvoiceRepository.Find(g => g.PaymentDay == date).OrderByDescending(i => i.PaymentDay).ToList();
                        }
                        else
                        {
                            ViewBag.SearchError = "You need to enter date in Month/Day/Year format (12/20/2018)";
                        }
                        break;
                    case "CreationDate":
                        if (DateTime.TryParse(searchString, out date))
                        {
                            invoices = unitOfWork.InvoiceRepository.GetAll().Where(g => g.Date == DateTime.Parse(searchString)).ToList().OrderByDescending(i => i.Date).ToList();
                        }
                        else
                        {
                            ViewBag.SearchError = "You need to enter date in Month/Day/Year format (12/20/2018)";
                        }
                        break;
                    default:
                        break;
                }
            }

            var products = unitOfWork.ProductRepository.Find(p => p.Quantity > 0).ToList();

            var persons = unitOfWork.PersonRepository.GetAll().ToList();

            InvoiceViewModelManager viewManager = new InvoiceViewModelManager();

            InvoiceDisplayViewModel displayViewModel = new InvoiceDisplayViewModel();

            InvoiceAddViewModel addViewModel = new InvoiceAddViewModel(persons, products);


            displayViewModel.InvoicesList = (PagedList<Invoice>)invoices.ToPagedList(pageIndex, 5);


            viewManager.AddViewModel = addViewModel;
            viewManager.DisplayViewModel = displayViewModel;

            return View(viewManager);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmSave(InvoiceAddViewModel model)
        {
            ModelState.Remove("PaymentDay");
            if (ModelState.IsValid)
            {
                Person person = unitOfWork.PersonRepository.Get(model.SelectedPersonId);
                model.SelectedPerson = person;
                model.PaymentDay = person.PaymentDeadline;
                model.SelectedProductsQty = new int[model.SelectedProductsIds.Length];

                for (int i = 0; i < model.SelectedProductsIds.Length; i++)
                {
                    int tmpProductId = model.SelectedProductsIds[i];
                    Product product = unitOfWork.ProductRepository.Get(tmpProductId);
                    if (product != null)
                    {
                        if (product.Quantity > 0)
                        {
                            model.SelectedProductsQty[i] = product.Quantity;
                            model.AddToProductsList(product);
                        }
                    }
                }
                return PartialView("Partials/ConfirmInvoiceModal", model);
            }
            else
            {
                ViewBag.Errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .First();
                Response.StatusCode = 400;
                return PartialView("Partials/ErrorModal");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(InvoiceAddViewModel model)
        {

            if (ModelState.IsValid)
            {
                Person person = unitOfWork.PersonRepository.Get(model.SelectedPersonId);
                int paymentDeadline = -1;

                if (model.PaymentDay != 0)
                    paymentDeadline = model.PaymentDay;
                else
                    paymentDeadline = person.PaymentDeadline;


                Invoice newInvoice = new Invoice();
                newInvoice.Date = DateTime.Now;
                newInvoice.PaymentDay = newInvoice.Date.AddDays(paymentDeadline);
                newInvoice.Person = person;
                newInvoice.IsProductReturned = model.IsProductReturned;

                for (int i = 0; i < model.SelectedProductsIds.Length; i++)
                {
                    int tmpProductId = model.SelectedProductsIds[i];
                    Product product = unitOfWork.ProductRepository.Get(tmpProductId);
                    if (product != null)
                    {
                        if (model.IsProductReturned) // checking if product is returned to me or solded
                        {
                            product.Quantity += model.SelectedProductsQty[i];
                        }
                        else
                        {
                            product.Quantity -= model.SelectedProductsQty[i];
                        }

                        newInvoice.InvoiceProducts.Add(new InvoiceProduct() { Product = product, Quantity = model.SelectedProductsQty[i] });
                    }
                    else
                    {
                        return HttpNotFound();
                    }
                }

                unitOfWork.InvoiceRepository.Add(newInvoice);
                unitOfWork.Save();
                TempData["SuccessMessage"] = "Invoice is successfully added!";
                Response.StatusCode = 201;
                return RedirectToAction("Index");

            }
            else
            {
                ViewBag.Errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .First();
                Response.StatusCode = 400;
                return PartialView("Partials/ErrorModal");
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var invoiceDetails = unitOfWork.InvoiceRepository.Get(id);
            InvoiceDisplayViewModel model = new InvoiceDisplayViewModel() { Invoice = invoiceDetails };
            if (invoiceDetails == null)
            {
                return HttpNotFound();
            }

            return PartialView("Partials/InvoiceDetailsModal", model);
        }

    }
}