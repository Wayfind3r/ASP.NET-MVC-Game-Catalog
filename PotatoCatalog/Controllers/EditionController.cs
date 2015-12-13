using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PotatoCatalog.Models;
using PotatoCatalog.Services;

namespace PotatoCatalog.Controllers
{
    public class EditionController : Controller
    {
        private EditionServices editionServices;

        public EditionController()
        {
            editionServices = new EditionServices();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult ManageEditions()
        {
            List<EditionViewModel> model = editionServices.GetEditionViewModelList();
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult EditEdition(int Id, string  newVal)
        {
            var model = editionServices.GetEdition(Id);
            return PartialView(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditEdition(Edition model)
        {
            editionServices.EditEdition(model.Id,model.Name);
            return this.RedirectToAction("ManageEditions");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult DeleteEdition(int id)
        {
            var model = editionServices.GetEdition(id);
            return PartialView(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult DeleteEdition(Edition model)
        {
            editionServices.DeleteEdition(model.Id);
            return this.RedirectToAction("ManageEditions");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult CreateEdition()
        {
            return PartialView();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateEdition(Edition newEdition)
        {
            editionServices.CreateEdition(newEdition);
            return this.RedirectToAction("ManageEditions");
        }
    }
}
