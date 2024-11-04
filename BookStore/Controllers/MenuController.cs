using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class MenuController : Controller
    {
        private DBBookStoreEntities db = new DBBookStoreEntities();
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MenuProductCate()
        {
            var items=db.Categories.ToList();
            return PartialView("_MenuProductCate",items);
        }
    }
}