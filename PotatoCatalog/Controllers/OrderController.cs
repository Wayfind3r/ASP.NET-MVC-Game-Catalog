using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Microsoft.AspNet.Identity;
using PotatoCatalog.Enums;
using PotatoCatalog.Models;
using PotatoCatalog.Services;

namespace PotatoCatalog.Controllers
{
    public class OrderController : Controller
    {
        private OrderServices orderServices;

        public OrderController()
        {
            orderServices = new OrderServices();
        }
        [HttpGet]
        public ActionResult OrderDetails(int orderId, string returnUrl)
        {
            var model = new OrderDetailedViedModel(orderId, returnUrl);
            return View(model);
        }

        public ActionResult PersonalOrderHistory()
        {
            var userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var model = orderServices.GetOrderViewModelListForUser(userId);
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult ManagePendingOrders()
        {
            var model = orderServices.GetPendingOrderViewModelList();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult OrderHistory()
        {
            var model = orderServices.GetNotPendingOrderViewModelList();
            return View(model);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult ApproveOrder(int id)
        {
            orderServices.ChangePendingOrderStatus(id, OrderStatus.Approved);
            return Json(new { Status = "success" });
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult RejectOrder(int id)
        {
            orderServices.ChangePendingOrderStatus(id, OrderStatus.Rejected);
            return Json(new { Status = "success" });
        }
    }
}