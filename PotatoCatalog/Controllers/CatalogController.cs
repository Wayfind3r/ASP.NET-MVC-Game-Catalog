using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
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
        public ActionResult Index(int page =1, int pageSize = 9)
        {
            var list = gameServices.GetAllGameViewModels();
            var model = new PagedList<GameViewModel>(list, page, pageSize);
            return View(model);
        }
        
    }
}