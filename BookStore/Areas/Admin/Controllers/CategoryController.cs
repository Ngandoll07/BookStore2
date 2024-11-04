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
    public class CategoryController : Controller
    {
        // GET: Admin/Category
        DBBookStoreEntities db = new DBBookStoreEntities();
        public ActionResult Index(int ? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);

            var categories = db.Categories.ToList().ToPagedList(pageNum, pageSize); // Fetch all categories from the database
            return View(categories);
        }
        public ActionResult Create()
        {
            var category = new Category(); // Khởi tạo một thể hiện mới của Category
            return View(category);
        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Categories.Add(category);
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
            return View(category);
        }

        public ActionResult Edit(string id)
        {
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound(); // Return a 404 error if the category is not found
            }
            return View(category); // Pass the category to the view
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Mark the entity as modified
                    db.Entry(category).State = System.Data.Entity.EntityState.Modified;

                    // Save changes to the database
                    db.SaveChanges();

                    // Redirect to the Index action after successful edit
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Log the exception message for debugging purposes
                    ModelState.AddModelError("", "Error updating category: " + ex.Message);
                }
            }

            // If we got this far, something failed; redisplay the form with validation errors
            return View(category);
        }

        public ActionResult Delete(string id)
        {
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound(); // Return a 404 error if the category is not found
            }
            return View(category); // Pass the category to the confirmation view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                // Find the category by ID
                var cat = db.Categories.Find(id);

                if (cat == null)
                {
                    return HttpNotFound(); // Return a 404 if the category is not found
                }

                // Remove the found category from the database
                db.Categories.Remove(cat);
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