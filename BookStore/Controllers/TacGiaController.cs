using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class TacGiaController : Controller
    {
        private DBBookStoreEntities db = new DBBookStoreEntities();
        // GET: TacGia
        public ActionResult Index(string id)
        {
            // Kiểm tra nếu id không rỗng và không null
            if (!string.IsNullOrEmpty(id))
            {
                // Lọc các sản phẩm theo Tác Giả ID
                var filteredBooks = db.Books.Where(b => b.TacGia.ID == id).ToList();
                return View(filteredBooks);
            }
            else
            {
                // Nếu không có ID thể loại, trả về toàn bộ sản phẩm hoặc xử lý lỗi nếu cần
                var allBooks = db.Books.ToList();
                return View(allBooks);
            }
        }
        public ActionResult Partial_TacGia()
        {
            var items = db.TacGias.ToList();
            return PartialView("Partial_TacGia", items); // Ensure you have a corresponding view
        }
    }
}