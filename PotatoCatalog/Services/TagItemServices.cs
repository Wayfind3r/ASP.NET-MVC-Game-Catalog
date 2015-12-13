using System.Linq;
using PotatoCatalog.Models;

namespace PotatoCatalog.Services
{
    public class TagItemServices
    {
        public int CreateTagItem(int gameId, int tagId)
        {
            int Id;
            TagItem newTagItem = new TagItem { GameId = gameId, TagId = tagId };
            using (var db = new ApplicationDbContext())
            {
                    db.TagItems.Add(newTagItem);
                    db.SaveChanges();
                db.Entry(newTagItem).GetDatabaseValues();
                Id = newTagItem.Id;
            }
            return Id;
        }

        public TagItemViewModel GetTagItemViewModelByID(int Id)
        {
            TagItemViewModel tagItemView = new TagItemViewModel();
            using (var db = new ApplicationDbContext())
            {
                TagItem targetTagItem = db.TagItems.FirstOrDefault(x=>x.Id==Id);
                tagItemView.Id = targetTagItem.Id;
                var tag = db.Tags.FirstOrDefault(t=>t.Id == targetTagItem.TagId);
                tagItemView.Name = tag.Name;
            }
            return tagItemView;
        }
        public void DeleteTagItem(int gameId, int tagId)
        {
            using (var db = new ApplicationDbContext())
            {
                var tagItemToDelete = db.TagItems.FirstOrDefault(t => t.GameId == gameId && t.TagId == tagId);
                db.TagItems.Remove(tagItemToDelete);
                db.SaveChanges();
            }
        }
    }
}