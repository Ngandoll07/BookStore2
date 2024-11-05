using BookStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using PagedList.Mvc;
using PagedList;

namespace BookStore.Areas.Admin.Controllers
{
    public class BookController : Controller
    {
        DBBookStoreEntities db = new DBBookStoreEntities();
        // GET: Admin/Book
        public ActionResult Index(int ? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);

            var books = db.Books.ToList().ToPagedList(pageNum, pageSize);
            return View(books);
        }
        [HttpGet]
        public ActionResult Create()
        {
            // Lấy danh sách Category từ database
            // Lấy danh sách thể loại
            ViewBag.Categories = new SelectList(db.Categories.ToList(), "ID", "NameCate");

            // Lấy danh sách tác giả
            ViewBag.TacGias = new SelectList(db.TacGias.ToList(), "ID", "NameAuthor");

            // Lấy danh sách mã giảm giá (Coupon)
            ViewBag.Coupons = new SelectList(db.Coupons.ToList(), "ID", "Code");

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book newBook, HttpPostedFileBase[] HinhSachFiles)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (HinhSachFiles != null && HinhSachFiles.Length > 0)
                    {
                        var imagePaths = new List<string>();

                        foreach (var file in HinhSachFiles)
                        {
                            if (file != null && file.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                var path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                                file.SaveAs(path);
                                imagePaths.Add("/Content/images/" + fileName);
                            }
                        }

                        // Lưu danh sách đường dẫn hình ảnh vào model
                        newBook.HinhSach = string.Join(",", imagePaths); // nối các đường dẫn lại

                        // Lưu model vào cơ sở dữ liệu
                        db.Books.Add(newBook);
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("HinhSachFiles", "Vui lòng tải lên đúng 3 hình ảnh.");
                    }
                }

                // Nếu không hợp lệ, tải lại các danh sách chọn
                ViewBag.Categories = new SelectList(db.Categories.ToList(), "ID", "NameCate");
                ViewBag.TacGias = new SelectList(db.TacGias.ToList(), "ID", "NameAuthor");
                ViewBag.Coupons = new SelectList(db.Coupons.ToList(), "ID", "Code");

                return View(newBook);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;

                ViewBag.Categories = new SelectList(db.Categories.ToList(), "ID", "NameCate");
                ViewBag.TacGias = new SelectList(db.TacGias.ToList(), "ID", "NameAuthor");
                ViewBag.Coupons = new SelectList(db.Coupons.ToList(), "ID", "Code");

                return View(newBook);
            }
        }


        public ActionResult Edit(string id)
        {
            var book = db.Books.Find(id); // Lấy sản phẩm theo ID
            if (book == null)
            {
                return HttpNotFound(); // Nếu không tìm thấy sản phẩm
            }

            var model = new Book
            {
                ID = book.ID,
                NameBook = book.NameBook,
                IDCAT = book.IDCAT,
                GIA = book.GIA,
                BookDescrip = book.BookDescrip,
                StockQuantity = book.StockQuantity,
                HinhSach = book.HinhSach,
                IDCoupon = book.IDCoupon,
                IDTG = book.IDTG
            };

            // Tạo danh sách cho dropdown
            ViewBag.Categories = new SelectList(db.Categories.ToList(), "ID", "NameCate");
            ViewBag.TacGias = new SelectList(db.TacGias.ToList(), "ID", "NameAuthor");
            ViewBag.Coupons = new SelectList(db.Coupons.ToList(), "ID", "Code");

            return View(model); // Trả về View với model đã có thông tin
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Book newBook, HttpPostedFileBase[] HinhSachFiles)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingBook = db.Books.Find(newBook.ID); // Tìm sản phẩm cũ bằng ID
                    if (existingBook != null)
                    {
                        // Cập nhật các thuộc tính
                        existingBook.NameBook = newBook.NameBook;
                        existingBook.IDCAT = newBook.IDCAT;
                        existingBook.GIA = newBook.GIA;
                        existingBook.BookDescrip = newBook.BookDescrip;
                        existingBook.StockQuantity = newBook.StockQuantity;
                        existingBook.IDCoupon = newBook.IDCoupon;
                        existingBook.IDTG = newBook.IDTG;

                        // Nếu có hình mới, cập nhật đường dẫn
                        if (HinhSachFiles != null && HinhSachFiles.Length > 0)
                        {
                            var imagePaths = new List<string>();

                            foreach (var file in HinhSachFiles)
                            {
                                if (file != null && file.ContentLength > 0)
                                {
                                    var fileName = Path.GetFileName(file.FileName);
                                    var path = Path.Combine(Server.MapPath("~/Content/images/"), fileName);
                                    file.SaveAs(path);
                                    imagePaths.Add("/Content/images/" + fileName);
                                }
                            }
                            //Ghi đè 
                            existingBook.HinhSach = string.Join(",", imagePaths); // Cập nhật thuộc tính HinhSach chỉ với hình ảnh mới
                        }

                        // Lưu thay đổi vào cơ sở dữ liệu
                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("", "Không tìm thấy sản phẩm.");
                }
                // Nếu không hợp lệ, load lại các dropdowns
                LoadDropdowns();
                return View(newBook);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;

                // Nếu có lỗi, load lại các dropdowns
                LoadDropdowns();
                return View(newBook);
            }
        }
        // Phương thức để load các dropdown
        private void LoadDropdowns()
        {
            ViewBag.Categories = new SelectList(db.Categories.ToList(), "ID", "NameCate");
            ViewBag.TacGias = new SelectList(db.TacGias.ToList(), "ID", "NameAuthor");
            ViewBag.Coupons = new SelectList(db.Coupons.ToList(), "ID", "Code");
        }
        // GET: Books/Delete/5
        public ActionResult Delete(string id)
        {
            var book = db.Books.Find(id); // Tìm sản phẩm theo ID
            if (book == null)
            {
                return HttpNotFound(); // Nếu không tìm thấy sản phẩm
            }
            return View(book); // Trả về view để xác nhận xóa
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var book = db.Books.Find(id); // Tìm sản phẩm theo ID
            if (book != null)
            {
                db.Books.Remove(book); // Xóa sản phẩm
                db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
            }
            return RedirectToAction("Index"); // Chuyển hướng về danh sách sản phẩm
        }
    }
}