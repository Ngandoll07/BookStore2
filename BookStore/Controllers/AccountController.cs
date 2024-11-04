using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers
{
    public class AccountController : Controller
    {
        private DBBookStoreEntities db = new DBBookStoreEntities();
        // GET: Account
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User _user)
        {

            if (ModelState.IsValid)
            {
                // Kiểm tra xem email đã tồn tại chưa
                if (db.Users.Any(u => u.Email == _user.Email))
                {
                    ModelState.AddModelError("Email", "Email đã được sử dụng.");
                    return View(_user);
                }

                _user.HashedPassword = HashPassword(_user.HashedPassword);
                _user.ID = GenerateUserID(); // Gán UserID mới
                _user.CreatedAt = DateTime.Now;
                _user.Status = true;

                try
                {
                    db.Users.Add(_user);
                    db.SaveChanges();
                    return RedirectToAction("Login", "Account");
                }
                catch (DbEntityValidationException ex)
                {
                    // Xử lý lỗi xác thực
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError("", validationError.ErrorMessage);
                        }
                    }
                }
            }

            return View(_user);
        }
        // GET: Account/Login
        public ActionResult Login(string message = null)
        {
            ViewBag.Message = message; // Hiển thị thông báo (nếu có)
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User _user)
        {
            if (ModelState.IsValid)
            {
                // Tìm người dùng trong cơ sở dữ liệu theo email
                var user = db.Users.SingleOrDefault(u => u.Email == _user.Email);

                if (user != null)
                {
                    // Sử dụng phương thức VerifyPassword để so sánh mật khẩu đã nhập với mật khẩu đã băm
                    if (VerifyPassword(_user.HashedPassword, user.HashedPassword))
                    {
                        // Lưu thông tin người dùng vào session
                        Session["UserName"] = user.NameUser;
                        Session["UserEmail"] = user.Email;
                        Session["UserRole"] = user.Role;

                        // Chuyển hướng dựa trên vai trò của người dùng
                        if (user.Role == "Admin")
                        {
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        // Thông báo nếu mật khẩu không đúng
                        ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
                    }
                }
                else
                {
                    // Thông báo nếu không tìm thấy người dùng
                    ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
                }
            }

            return View(_user);
        }


        private bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            // Hash mật khẩu nhập vào
            string hashedInputPassword = HashPassword(inputPassword);
            // So sánh mật khẩu đã hash
            return hashedInputPassword == hashedPassword;
        }

        private string GenerateUserID()
        {
            var lastUser = db.Users.OrderByDescending(u => u.ID).FirstOrDefault();
            int nextId = 1; // Giá trị mặc định

            if (lastUser != null && lastUser.ID.StartsWith("KH"))
            {
                // Lấy số sau "KH"
                string lastIdPart = lastUser.ID.Substring(2); // Lấy phần số
                if (int.TryParse(lastIdPart, out int lastNumber))
                {
                    nextId = lastNumber + 1; // Tăng lên 1
                }
            }

            return $"KH{nextId:D3}"; // Định dạng thành KH001, KH002, ...
        }

        private string HashPassword(string password)
        {
            // Thực hiện mã hóa mật khẩu (sử dụng SHA256 hoặc BCrypt...)
            // Đơn giản hóa cho ví dụ, bạn nên sử dụng thư viện mạnh hơn để hash
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }
        public ActionResult Logout()
        {
            Session.Clear(); // Xóa tất cả session
            return RedirectToAction("Index", "Home");
        }
    }
}