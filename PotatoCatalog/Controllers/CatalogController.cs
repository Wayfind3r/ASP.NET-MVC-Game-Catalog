using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PotatoCatalog.Models;
using PotatoCatalog.Services;

namespace PotatoCatalog.Controllers
{
    public class CatalogController : Controller
    {
        private GameServices gameServices;

        public CatalogController()
        {
            gameServices = new GameServices();
        }
        // GET: Catalog
        public ActionResult Index()
        {
            var model = gameServices.GetAllGameViewModels();
            return View(model);
        }
        
    }
}