using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class CategoryController : Controller
    {
        private DBBookStoreEntities db = new DBBookStoreEntities();
        // GET: Category
        public ActionResult Index()
        {
            var categories = db.Categories.ToList(); // Replace with actual data fetching logic
            return View(categories);
        }
        public ActionResult Partial_Category()
        {
            var items = db.Books.Take(12).ToList();
            return PartialView(items);
        }
        
        public ActionResult Partial_MenuCate()
        {
            var items = db.Categories.ToList();
            return PartialView("_PartialMenuCate",items); // Ensure you have a corresponding view
        }
        

    }
}