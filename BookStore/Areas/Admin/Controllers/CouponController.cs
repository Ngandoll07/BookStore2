﻿using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.Web.UI;

namespace BookStore.Areas.Admin.Controllers
{
    public class CouponController : Controller
    {
        // GET: Admin/Category
        DBBookStoreEntities db = new DBBookStoreEntities();
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);

            var coupon = db.Coupons.ToList().ToPagedList(pageNum, pageSize); // Fetch all categories from the database
            return View(coupon);
        }
        public ActionResult Create()
        {
            var coupon = new Coupon(); // Khởi tạo một thể hiện mới của Category
            return View(coupon);
        }

        [HttpPost]
        public ActionResult Create(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Coupons.Add(coupon);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError("", validationError.ErrorMessage);
                        }
                    }
                }
            }

            // Return to the Create view with the current model state to show validation errors
            return View(coupon);
        }

        public ActionResult Edit(string id)
        {
            var coupon = db.Coupons.Find(id);
            if (coupon == null)
            {
                return HttpNotFound(); // Return a 404 error if the category is not found
            }
            return View(coupon); // Pass the category to the view
        }

        [HttpPost]
        public ActionResult Edit(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Mark the entity as modified
                    db.Entry(coupon).State = System.Data.Entity.EntityState.Modified;

                    // Save changes to the database
                    db.SaveChanges();

                    // Redirect to the Index action after successful edit
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Log the exception message for debugging purposes
                    ModelState.AddModelError("", "Error updating coupons: " + ex.Message);
                }
            }

            // If we got this far, something failed; redisplay the form with validation errors
            return View(coupon);
        }

        public ActionResult Delete(string id)
        {
            var coupon = db.Coupons.Find(id);
            if (coupon == null)
            {
                return HttpNotFound(); // Return a 404 error if the category is not found
            }
            return View(coupon); // Pass the category to the confirmation view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                // Find the category by ID
                var coupon = db.Coupons.Find(id);

                if (coupon == null)
                {
                    return HttpNotFound(); // Return a 404 if the category is not found
                }

                // Remove the found category from the database
                db.Coupons.Remove(coupon);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log the exception message (optional, for debugging purposes)
                return Content("Lỗi: " + ex.Message);
            }
        }
    }
}