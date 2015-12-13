using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult ManagePendingOrders()
        {
            var model = orderServices.GetNonRejectedOrderViewModelList();
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