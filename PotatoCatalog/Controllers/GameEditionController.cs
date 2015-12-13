using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PotatoCatalog.Models;
using PotatoCatalog.Services;

namespace PotatoCatalog.Controllers
{
    public class GameEditionController : Controller
    {
        private GameEditionServices services;

        public GameEditionController()
        {
            services = new GameEditionServices();
        }

        // GET: GameEdition
        [Authorize(Roles = "Admin")]
        public ActionResult ManageGameEditions(int gameId)
        {
            var model = new GameEditionViewModelBag(gameId);
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteGameEdition(int id)
        {
            var editionToDelete = services.GetGameEditionByID(id);
            return PartialView(editionToDelete);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGameEdition(int gameId, int editionId)
        {
            services.DeleteGameEdition(editionId);
            return RedirectToAction("ManageGameEditions", new { gameId = gameId });
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditGameEdition(int id)
        {
            var model = services.GetGameEditionByID(id);
            return PartialView(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult SaveChanges(GameEditionViewModel model)
        {
            //null exception !????????
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            services.UpdateFromModel(model);
            return RedirectToAction("ManageGameEditions", new { gameId= model.GameId});
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult CreateGameEdition(int gameId)
        {
            var model = new CreateGameEditionViewModel();
            model.GameId = gameId;
            var serv = new EditionServices();
            model.EditionsList = serv.GetEditionsSelectList();
            return PartialView(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateGameEdition(GameEditionViewModel model)
        {
            services.CreateGameEdition(model.GameId,model.EditionId,model.PriceInPotatoes,model.Contents);
            return this.RedirectToAction("ManageGameEditions", new { gameId = model.GameId});
        }
    }
}