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
    public class TagController : Controller
    {
        private TagServices tagServices;
        public TagController()
        {
            tagServices = new TagServices();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        //[ValidateAntiForgeryToken]
        public ActionResult ManageTags(int page = 1, int pageSize = 50, string searchString = null)
        {
            var list = new List<TagViewModel>();
            if (String.IsNullOrEmpty(searchString))
            {
                list = tagServices.GetAllTagsWithInstanceCount();
            }
            else
            {
                list = tagServices.SearchAllTagsWithInstanceCount(searchString);
            }
            var model = new PagedList<TagViewModel>(list,page,pageSize);
            return View(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult ManageTags(Tag thisTag)
        {
            tagServices.EditTag(thisTag);
            List<Tag> model = tagServices.GetAllTagsList();
            ViewBag.tagList = model;
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult EditTag(int Id)
        {
            var targetTag = tagServices.GetTagById(Id);
            EditTagViewModel model= new EditTagViewModel {Id = targetTag.Id, Name = targetTag.Name};
            return PartialView(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult EditTag(EditTagViewModel model)
        {
            Tag tag = new Tag {Id = model.Id,Name = model.Name};
            tagServices.EditTag(tag);
            return this.RedirectToAction("ManageTags");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult DeleteTag(int id)
        {
            var targetTag = tagServices.GetTagById(id);
            EditTagViewModel model = new EditTagViewModel { Id = targetTag.Id, Name = targetTag.Name };
            return PartialView(model);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult DeleteTag(EditTagViewModel model)
        {
            int id = model.Id;
            tagServices.DeleteTag(id);
            return this.RedirectToAction("ManageTags");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult CreateTag()
        {
            return PartialView();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreateTag(Tag newTag)
        {
            tagServices.CreateTag(newTag);
            return this.RedirectToAction("ManageTags");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        //Creates tagItem and tag if necessary
        public ActionResult CreateTagItem(int currentGameId,string newTag)
        {
            if (!string.IsNullOrEmpty(newTag) && newTag!="")
            {
                tagServices.CreateTag(new Tag {Name = newTag});
                int tagID = tagServices.GetTagIDByValue(newTag);
                TagItemServices tagItemsServices = new TagItemServices();
                tagItemsServices.CreateTagItem(currentGameId, tagID);
                return Json(new { Status = "success" });
            }
            return Json(new { Status = "error" });
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult DeleteTagItemByGameIdAndValue(int GameID, string tagValue)
        {
            if (!string.IsNullOrEmpty(tagValue) && tagValue!="")
            {
                int tagID = tagServices.GetTagIDByValue(tagValue);
                TagItemServices tagItemsServices = new TagItemServices();
                tagItemsServices.DeleteTagItem(GameID, tagID);
                return Json(new { Status = "success" });
            }
            return Json(new { Status = "error" });
        }
    }
}