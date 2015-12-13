using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PotatoCatalog.Models;
using PotatoCatalog.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace PotatoCatalog.Controllers
{
    public class CartController : Controller
    {
        private GameEditionServices gameEditionServices;
        private UserServices userServices;
        private OrderServices orderServices;
        public CartController()
        {
            gameEditionServices = new GameEditionServices();
            userServices = new UserServices();
            orderServices = new OrderServices();
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }
        public RedirectToRouteResult AddToCart(Cart cart,int gameEditionId, string returnUrl)
        {
            GameEditionCartViewModel edition = gameEditionServices.GetGameEditionCartViewModel(gameEditionId);
            if (edition != null)
            {
                cart.AddItem(edition, 1);
            }
            return RedirectToAction("Index", new {returnUrl});
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart,int gameEditionId, string returnUrl)
        {
            GameEditionCartViewModel edition = gameEditionServices.GetGameEditionCartViewModel(gameEditionId);
            if (edition != null)
            {
                cart.RemoveLine(edition);
            }
            return RedirectToAction("Index", new {returnUrl});
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public PartialViewResult Potatoes()
        {
            var potatoes = userServices.GetCurrentPotatoes();
            return PartialView(potatoes);
        }

        public ViewResult Checkout()
        {
            return View(new AddressViewModel());
        }
        [HttpPost]
        public ViewResult Checkout(Cart cart, AddressViewModel model)
        {
            if (!cart.Lines.Any())
            {
                ModelState.AddModelError("","Sorry, your cart is empty!");
            }
            if (ModelState.IsValid)
            {
                if (orderServices.TryProcessOrder(cart, model))
                {
                    cart.Clear();
                    return View("Completed");
                }
                ModelState.AddModelError("", "Not enough potatoes!");
            }
                return View(model);
        }
    }
}