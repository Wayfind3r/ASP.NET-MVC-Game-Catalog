using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PagedList;
using PotatoCatalog.Models;
using PotatoCatalog.Services;

namespace PotatoCatalog.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private UserServices userServices;

        public AdminController()
        {
            userServices = new UserServices();
        }

        [HttpGet]
        public ActionResult ManageAccounts(int page=1, int pageSize = 12, string searchString = null)
        {
            var list = new List<UserTableViewModel>();
            if (String.IsNullOrEmpty(searchString))
            {
                list = userServices.GetUserTableViewModels();
            }
            else
            {
                list = userServices.GetUserTableViewModels(searchString);
            }
            var model = new PagedList<UserTableViewModel>(list, page, pageSize);
            return View(model);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}
