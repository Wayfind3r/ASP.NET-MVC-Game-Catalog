using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PotatoCatalog.Models;

namespace PotatoCatalog.Services
{
    public class EditionServices
    {
        //Cannot create duplicate editions
        public void CreateEdition(Edition edition)
        {
            using (var db = new ApplicationDbContext())
            {
                var editionExist = db.Editions.Any(x => x.Name.Equals(edition.Name));
                if (!editionExist)
                {
                    db.Editions.Add(edition);
                    db.SaveChanges();
                }
            }
        }

        //This method will delete GameEditions too!!!
        public void DeleteEdition(string editionName)
        {
            using (var db = new ApplicationDbContext())
            {
                var editon = db.Editions.Any(x => x.Name.Equals(editionName));
                if (editon)
                {
                    var thisEd = db.Editions.FirstOrDefault(g => g.Name == editionName.ToLower());
                    var editionToDelete = from i in db.GameEditions where i.EditionId == thisEd.Id select i;
                    foreach (var item in editionToDelete)
                    {
                        db.GameEditions.Remove(item);
                    }
                    db.Editions.Remove(thisEd);
                    db.SaveChanges();
                }
            }
        }
        //This method will delete only Editions!!
        public void DeleteEdition(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var target = db.Editions.FirstOrDefault(x => x.Id.Equals(id));
                db.Editions.Remove(target);
                db.SaveChanges();
            }
        }

        public void EditEdition(int Id, string newValue)
        {
            using (var db = new ApplicationDbContext())
            {
                var edition = db.Editions.Any(x => x.Id.Equals(Id));
                if (edition)
                {
                    var thisEd = db.Editions.FirstOrDefault(x => x.Id.Equals(Id));
                    thisEd.Name = newValue;
                    db.SaveChanges();
                }
            }
        }

        public List<EditionViewModel> GetEditionViewModelList()
        {
            List<EditionViewModel> edList = new List<EditionViewModel>();
            using (var db = new ApplicationDbContext())
            {
                var query = from e in db.Editions
                    join g in db.GameEditions on e.Id equals g.EditionId
                    group new {e, g} by new {e.Id, e.Name}
                    into g
                    select new EditionViewModel
                    {
                        Id = g.Key.Id,
                        Name = g.Key.Name,
                        Instances = g.Count()
                    };
                //Default order
                edList = query.OrderByDescending(x => x.Instances).ToList();
            }
            return edList;
        }

        public Edition GetEdition(int Id)
        {
            Edition result;
            using (var db = new ApplicationDbContext())
            {
                result = db.Editions.FirstOrDefault(x => x.Id.Equals(Id));
            }
            return result;
        }

        public SelectList GetEditionsSelectList()
        {
            var list = new List<SelectListItem>();
            using (var db = new ApplicationDbContext())
            {
                list =
                    db.Editions.Select(x => new SelectListItem() {Text = x.Name, Value = x.Id.ToString()}).ToList();
            }
            var result = new SelectList(list, "Value", "Text");
            return result;
        }
    }
}