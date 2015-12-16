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
        public ActionResult Index(int page =1, int pageSize = 9, string searchBy = null, string searchString = null)
        {
            var list = new List<GameViewModel>();
            if (String.IsNullOrEmpty(searchString))
            {
                list = gameServices.GetAllGameViewModels();
            }
            else
            {
                if (searchBy == "Title")
                {
                    list = gameServices.GetAllGameViewModelsWithTitle(searchString);
                }
                else
                {
                    if (searchBy == "Tag")
                    {
                        list = gameServices.GetAllGameViewModelsWithTag(searchString);
                    }
                }
            }
            var model = new PagedList<GameViewModel>(list, page, pageSize);
            return View(model);
        }
        
    }
}