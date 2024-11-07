using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class CheckOutController : Controller
    {
        private DBBookStoreEntities db = new DBBookStoreEntities();
        // GET: CheckOut
        public ActionResult Index()
        {
            // Assuming you have a method to get the current cart for the user
            var cart = GetCart(); // Replace this with actual logic to retrieve the cart data

            // Check if cart is null, and if so, initialize it or handle it as needed
           
            return View(cart);
        }
        private Cart GetCart()
        {
            // Lấy giỏ hàng từ session, hoặc tạo mới nếu chưa tồn tại
            var cart = (Cart)Session["Cart"];
           
            return cart;
        }

    }
}