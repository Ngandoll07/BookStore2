using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private DBBookStoreEntities db = new DBBookStoreEntities();
        public ActionResult Index(string id)
        {
            // Kiểm tra nếu id không rỗng và không null
            if (!string.IsNullOrEmpty(id))
            {
                // Lọc các sản phẩm theo Category ID
                var filteredBooks = db.Books.Where(b => b.Category.ID == id).ToList();
                return View(filteredBooks);
            }
            else
            {
                // Nếu không có ID thể loại, trả về toàn bộ sản phẩm hoặc xử lý lỗi nếu cần
                var allBooks = db.Books.ToList();
                return View(allBooks);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}