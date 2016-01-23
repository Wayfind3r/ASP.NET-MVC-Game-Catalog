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
        private OrderServices orderServices;

        public AdminController()
        {
            userServices = new UserServices();
            orderServices = new OrderServices();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult ManageAccountOrders(int id, int page = 1, int pageSize = 12)
        {
            var model = new SingleAccountOrderBag(page, pageSize, id);
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Give300Potatoes(int id)
        {
            decimal potatoes = 0;
            var success = userServices.ModifyPotatoes(id, 300,out potatoes);
            if (success)
            {
                return Json(new {Status = "success", Value = potatoes.ToString("0.00")});
            }
            return Json(new {Status = "error"});
        }
    }
}
