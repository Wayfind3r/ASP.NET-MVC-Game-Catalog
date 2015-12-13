using System.Collections.Generic;
using System.Linq;
using PotatoCatalog.Models;

namespace PotatoCatalog.Services
{
    public class GameEditionServices
    {
        public void CreateGameEdition(int gameId, int editionId, decimal price, string contents)
        {
            using (var db = new ApplicationDbContext())
            {
                var newGame = new GameEdition
                {
                    GameId = gameId,
                    EditionId = editionId,
                    PriceInPotatoes = price,
                    Contents = contents
                };
                db.GameEditions.Add(newGame);
                db.SaveChanges();
            }
        }

        public GameEditionCartViewModel GetGameEditionCartViewModel(int id)
        {
            var result = new GameEditionCartViewModel();
            using (var db = new ApplicationDbContext())
            {
                var orig = db.GameEditions.FirstOrDefault(g => g.Id == id);
                result.Id = orig.Id;
                result.GameTitle = orig.Game.Title;
                result.EditionName = orig.Edition.Name;
                result.PriceInPotatoes = orig.PriceInPotatoes;
                result.Contents = orig.Contents;
            }
            return result;
        }

        public void DeleteGameEdition(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var gameEdition = db.GameEditions.FirstOrDefault(x => x.Id.Equals(id));
                db.GameEditions.Remove(gameEdition);
                db.SaveChanges();
            }
        }

        public void UpdateFromModel(GameEditionViewModel model)
        {
            using (var db = new ApplicationDbContext())
            {
                var edition = db.GameEditions.FirstOrDefault(x => x.Id == model.Id);
                edition.Contents = model.Contents;
                edition.PriceInPotatoes = model.PriceInPotatoes;
                db.SaveChanges();
            }
        }
        public List<GameEditionViewModel> GetGameEditions(int gameId)
        {
            var result = new List<GameEditionViewModel>();
            using (var db = new ApplicationDbContext())
            {
                result =
                    db.GameEditions.Where(t => t.GameId == gameId)
                        .Select(
                            t =>
                                new GameEditionViewModel
                                {
                                    Id = t.Id,
                                    GameId = t.GameId,
                                    EditionId = t.EditionId,
                                    EditionTitle = t.Edition.Name,
                                    PriceInPotatoes = t.PriceInPotatoes,
                                    Contents = t.Contents
                                })
                        .ToList();
                foreach (var edition in result)
                {
                    var hasPendingOrders = db.OrderItems.Any(x => x.GameEditionId.Equals(edition.Id));
                    edition.isPending = hasPendingOrders;
                }
            }
            return result;
        }

        public GameEditionViewModel GetGameEditionByID(int id)
        {
            var result = new GameEditionViewModel();
            using (var db = new ApplicationDbContext())
            {
                var temp = db.GameEditions.FirstOrDefault(t => t.Id == id);
                result.Id = temp.Id;
                result.GameId = temp.GameId;
                result.EditionId = temp.EditionId;
                result.EditionTitle = temp.Edition.Name;
                result.PriceInPotatoes = temp.PriceInPotatoes;
                result.Contents = temp.Contents;
            }
            return result;
        }
    }
}