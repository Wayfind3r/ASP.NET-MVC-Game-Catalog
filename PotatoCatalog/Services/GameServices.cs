using System;
using System.Collections.Generic;
using System.Linq;
using PotatoCatalog.Models;

namespace PotatoCatalog.Services
{
    public class GameServices
    {
        //No Image services yet
        public int CreateGame(GameViewModel model)
        {
            int iD = 0;
            using (var db = new ApplicationDbContext())
            {
                var newGame = new Game
                {
                    Title = model.Title,
                    Publisher = model.Publisher,
                    Developer = model.Developer,
                    ReleaseDate = model.ReleaseDate,
                    Description = model.Description,
                    ImgPath = model.ImagePath,
                    NumberOfTags = 0
                };
                db.Games.Add(newGame);
                db.SaveChanges();
                iD = newGame.Id;
            }
            return iD;
        }

        public void DeleteGame(int Id)
        {
            using (var db = new ApplicationDbContext())
            {
                var gameToDelete = db.Games.FirstOrDefault(g=>g.Id==Id);
                db.Games.Remove(gameToDelete);
                db.SaveChanges();
            }
        }

        public GameViewModel GetGameViewModelById(int Id)
        {
            GameViewModel game;
            using (var db = new ApplicationDbContext())
            {
                var dbGame = db.Games.FirstOrDefault(g => g.Id == Id);
                game = new GameViewModel
                {
                    Id = dbGame.Id,
                    Title = dbGame.Title,
                    Publisher = dbGame.Publisher,
                    Developer = dbGame.Developer,
                    Description = dbGame.Description,
                    ReleaseDate = dbGame.ReleaseDate,
                    NumberOfTags = dbGame.NumberOfTags,
                    ImagePath = dbGame.ImgPath
                };
                var tagServ = new TagServices();
                game.Tags = tagServ.GetTagItemViewModelsByGameID(game.Id);
            }
            return game;
        }
        public CompleteGameViewModel GetCompleteGameViewModelById(int Id)
        {
            CompleteGameViewModel game;
            using (var db = new ApplicationDbContext())
            {
                var dbGame = db.Games.FirstOrDefault(g => g.Id == Id);
                game = new CompleteGameViewModel
                {
                    Id = dbGame.Id,
                    Title = dbGame.Title,
                    Publisher = dbGame.Publisher,
                    Developer = dbGame.Developer,
                    Description = dbGame.Description,
                    ReleaseDate = dbGame.ReleaseDate,
                    NumberOfTags = dbGame.NumberOfTags,
                    ImagePath = dbGame.ImgPath
                };
                var tagServ = new TagServices();
                var edServ = new GameEditionServices();
                game.Tags = tagServ.GetTagItemViewModelsByGameID(game.Id);
                game.Editions = edServ.GetGameEditions(game.Id);
            }
            return game;
        }

        public List<GameViewModel> GetAllGameViewModels()
        {
            List<GameViewModel> result;
            using (var db = new ApplicationDbContext())
            {
                var iQueryableGames = from g in db.Games
                        select
                        new GameViewModel
                        {
                            Id = g.Id,
                            Title = g.Title,
                            Publisher = g.Publisher,
                            Developer = g.Developer,
                            Description = g.Description,
                            ReleaseDate = g.ReleaseDate,
                            ImagePath = g.ImgPath
                        };
                result = iQueryableGames.ToList();
                var tagServ = new TagServices();
                foreach (var game in result)
                {
                    game.Tags = tagServ.GetTagItemViewModelsByGameID(game.Id);
                }
                //Initial order
                result = result.OrderBy(x => x.Title).ToList();
            }
            return result;
        } 

        public void UpdateGameFromViewModel(GameViewModel model)
        {
            using (var db = new ApplicationDbContext())
            {
                var game = db.Games.FirstOrDefault(g => g.Id == model.Id);
                game.Title = model.Title;
                game.Publisher = model.Publisher;
                game.Developer = model.Developer;
                game.ReleaseDate = model.ReleaseDate;
                game.Description = model.Description;
                game.NumberOfTags = model.NumberOfTags;
                if (model.ImagePath != null)
                {
                    game.ImgPath = model.ImagePath;
                }
                db.SaveChanges();
            }
        }

        public List<Game> GetGamesList()
        {
            List<Game> gameList;
            using (var db = new ApplicationDbContext())
            {
                gameList = db.Games.AsEnumerable().ToList();
            }
            return gameList;
        }

        public List<SimpleGameViewModel> GetSimpleGamesList()
        {
            List<SimpleGameViewModel> gamesList = new List<SimpleGameViewModel>();
            using (var db = new ApplicationDbContext())
            {
                var result = from g in db.Games
                             select
                                 new SimpleGameViewModel
                                 {
                                     Id = g.Id,
                                     Developer = g.Developer,
                                     Publisher = g.Publisher,
                                     ReleaseDate = g.ReleaseDate,
                                     Title = g.Title
                                 };
                gamesList = result.ToList();
                gamesList.OrderBy(g => g.Title);
            }
            return gamesList;
        }

        public bool HasEditions(int Id)
        {
            bool hasEditions = true;
            using (var db = new ApplicationDbContext())
            {
                hasEditions = db.GameEditions.Any(e => e.GameId == Id);
            }
            return hasEditions;
        }

        public bool TryDeleteGame(GameViewModel game)
        {
            bool isDeleted = true;
            using (var db = new ApplicationDbContext())
            {
                var hasEditions = db.GameEditions.Any(e => e.GameId == game.Id);
                //var isOrdered = db.OrderItems.Any(o => o.GameEdition.GameId == game.Id);
                if (hasEditions)
                {
                    isDeleted = false;
                }
                else
                {
                    var gameTodelete = db.Games.FirstOrDefault(g => g.Id == game.Id);
                    var tagItemsToDelete = from i in db.TagItems where i.GameId == game.Id select i;
                    foreach (var tagItem in tagItemsToDelete)
                    {
                        db.TagItems.Remove(tagItem);
                    }
                    db.Games.Remove(gameTodelete);
                    db.SaveChanges();
                }
            }
                return isDeleted;
        }

        public string GetGameTitle(int Id)
        {
            string title;
            using (var db = new ApplicationDbContext())
            {
                title = db.Games.FirstOrDefault(x => x.Id.Equals(Id)).Title;
            }
            return title;
        }
    }
}