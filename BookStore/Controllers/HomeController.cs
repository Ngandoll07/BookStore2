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

        public ActionResult DiscountedBooks()
        {
            // Lọc sách có mã giảm giá (các sách có Coupon liên kết)
            var discountedBooks = db.Books.Where(b => b.IDCoupon != null && b.Coupon != null).ToList();
            return View(discountedBooks);
        }
        public ActionResult BestSellers()
        {
            var bestSellingBooks = db.Books
             .Where(b => b.StockQuantity <= 21) // Điều kiện cho sách bán chạy (số lượng tồn kho <= 20)
             .Take(8) // Lấy tối đa 8 cuốn sách
             .ToList();

            return View(bestSellingBooks); // Trả về view với danh sách sách bán chạy
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