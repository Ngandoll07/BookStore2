using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class ProductController : Controller
    {
        private DBBookStoreEntities db = new DBBookStoreEntities();

        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Partial_ItemsByCate()
        {
            var items = db.Books.Take(12).ToList();
            return PartialView(items);
        }
    }
}