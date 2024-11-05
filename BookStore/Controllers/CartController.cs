using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class CartController : Controller
    {
        private DBBookStoreEntities db = new DBBookStoreEntities();
        // GET: Cart
        public ActionResult Index()
        {
            if (Session["Cart"] == null)
                return RedirectToAction("Index","Cart");
            Cart _cart = Session["Cart"] as Cart;
            return View(_cart);
        }
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if(cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"]=cart;
            }  
            return cart;
        }
        public ActionResult AddToCart(string id)
        {
            var _book=db.Books.SingleOrDefault(s=>s.ID==id);
            if(_book!=null)
            {
                GetCart().Add_Product_Cart(_book);
            }
            return RedirectToAction("Index", "Cart");
        }
        /*public ActionResult UpdateCartQuantity(FormCollection form)
        { Cart cart = Session["Cart"] as Cart;
            string id_Book = form["idBook"];
            int _quantity = int.Parse(form["cartQuantity"]);
            cart.UpDateQuantity(id_Book, _quantity);
            return RedirectToAction("Index", "Cart");
        }*/
        public ActionResult UpdateCartQuantity(FormCollection form)
        {
            // Lấy đối tượng giỏ hàng từ Session
            Cart cart = Session["Cart"] as Cart;
            if (cart == null)
            {
                // Nếu giỏ hàng chưa tồn tại, chuyển hướng về trang Index
                return RedirectToAction("Index", "Cart");
            }

            // Lấy id_Book và số lượng từ form
            string id_Book = form["idBook"];
            int _quantity;

            // Kiểm tra nếu id_Book là null hoặc _quantity không phải là số
            if (string.IsNullOrEmpty(id_Book) || !int.TryParse(form["cartQuantity"], out _quantity))
            {
                // Chuyển hướng về trang Index nếu có lỗi trong dữ liệu đầu vào
                return RedirectToAction("Index", "Cart");
            }

            // Gọi phương thức cập nhật số lượng sản phẩm trong giỏ hàng
            cart.UpDateQuantity(id_Book, _quantity);

            return RedirectToAction("Index", "Cart");
        }

        public ActionResult RemoveCart(string id)
        {
            Cart cart = Session["Cart"] as Cart;
            cart.ReMoveItem(id);
            return RedirectToAction("Index", "Cart");
        }
      public PartialViewResult BagCart()
        {
            int total_quantity_item = 0;
            Cart cart = Session["Cart"]as Cart;
            if (cart != null)
                total_quantity_item = cart.TotalQuantity();
            ViewBag.TotalQuantity = total_quantity_item;
            return PartialView ("BagCart");
        }
    }
}