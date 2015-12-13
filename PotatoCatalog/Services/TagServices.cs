using System;
using System.Collections.Generic;
using System.Linq;
using PotatoCatalog.Models;

namespace PotatoCatalog.Services
{
    public class TagServices
    {

        public List<TagItemViewModel> GetTagItemViewModelsByGameID(int Id)
        {
            List<TagItemViewModel> tagItemList = new List<TagItemViewModel>();
            using (var db = new ApplicationDbContext())
            {
                List<TagItem> dbTagItems = db.TagItems.Where(t => t.GameId == Id).ToList();
                var dbTags = from tagItem in dbTagItems
                    join tag in db.Tags
                        on tagItem.TagId equals tag.Id
                    select new TagItemViewModel {Id = tagItem.Id, Name = tag.Name};
                foreach (var tag in dbTags)
                {
                    tagItemList.Add(tag);
                }
            }
            return tagItemList;
        }
        public void CreateTag(Tag newTag)
        {
            using (var db = new ApplicationDbContext())
            {

                var tag = db.Tags.Any(x => x.Name.Equals(newTag.Name));
                if (!tag)
                {
                    db.Tags.Add(newTag);
                    db.SaveChanges();
                }
            }
        }

        public void DeleteTag(int Id)
        {
            using (var db = new ApplicationDbContext())
            {
                var tag = db.Tags.Any(x => x.Id == Id);
                if (tag)
                {
                    var thisTag = db.Tags.FirstOrDefault(g => g.Id == Id);
                    var tagItemsToDelete = from i in db.TagItems where i.TagId==thisTag.Id select i;
                    foreach (var tagItem in tagItemsToDelete)
                    {
                        db.TagItems.Remove(tagItem);
                    }
                    db.Tags.Remove(thisTag);
                    db.SaveChanges();
                }
            }
        }

        public List<Tag> GetAllTagsList()
        {
            List<Tag> tagList;
            using (var db = new ApplicationDbContext())
            {
                tagList = db.Tags.AsEnumerable().ToList();
            }
            return tagList;
        }

        public Tag GetTagById(int Id)
        {
            Tag result;
            using (var db = new ApplicationDbContext())
            {
                result = db.Tags.FirstOrDefault(x => x.Id == Id);
            }
            return result;
        }

        public int GetTagIDByValue(string value)
        {
            int Id = 0;
            using (var db = new ApplicationDbContext())
            {
                Tag result;
                result = db.Tags.FirstOrDefault(x=>x.Name.Equals(value));
                Id = result.Id;
            }
            return Id;
        }
        /// <summary>
        /// Check if this tag already exists
        /// if it does update all tagItems with the new tagID and delete the old tag
        /// if not just update the tag value
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="name"></param>
        public void EditTag(Tag targetTag)
        {
            using (var db = new ApplicationDbContext())
            {
                var exists = db.Tags.Any(t => t.Name.Equals(targetTag.Name));
                if (exists)
                {
                    Tag existingTag = db.Tags.FirstOrDefault(t => t.Name.Equals(targetTag.Name));
                        //,StringComparison.OrdinalIgnoreCase));
                    if (existingTag.Id != targetTag.Id)
                    {
                        db.TagItems.Where(t => t.TagId == targetTag.Id).ToList().ForEach(x => x.TagId = existingTag.Id);
                        Tag tagToDelete = db.Tags.FirstOrDefault(t => t.Id == targetTag.Id);
                        db.Tags.Remove(tagToDelete);
                        db.SaveChanges();
                    }
                }
                else
                {
                    Tag oldTag = db.Tags.FirstOrDefault(t => t.Id == targetTag.Id);
                    oldTag.Name = targetTag.Name;
                    db.SaveChanges();
                }
            }
        }
    }
}