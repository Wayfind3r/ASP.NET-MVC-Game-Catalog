using System.Linq;
using PotatoCatalog.Models;

namespace PotatoCatalog.Services
{
    public class TagItemServices
    {
        /// <summary>
        /// Create a new tag Item with specific gameId and tagId
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="tagId"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Delete tag Item by gameId and tagId
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="tagId"></param>
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