using System;
using System.Collections.Generic;
using System.Linq;
using PotatoCatalog.Models;

namespace PotatoCatalog.Services
{
    public class GameServices
    {
        //Preliminary Create Game, no image upload
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
        //Delete game by Id <unused atm>
        public void DeleteGame(int Id)
        {
            using (var db = new ApplicationDbContext())
            {
                var gameToDelete = db.Games.FirstOrDefault(g=>g.Id==Id);
                db.Games.Remove(gameToDelete);
                db.SaveChanges();
            }
        }
        //Get game View Model by Id
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
        //Get Complete Game View Model for single Game view
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
        //Get View Models for all Games to populate the Catalog Index
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
        /// <summary>
        /// Get View Models for all Games with specific tag to populate the Catalog Index
        /// </summary>
        /// <param name="searchBy">tag value to search for in tag Names (Contains)</param>
        /// <returns></returns>
        public List<GameViewModel> GetAllGameViewModelsWithTag(string searchBy)
        {
            List<GameViewModel> result;
            using (var db = new ApplicationDbContext())
            {
                var searchByToLower = searchBy.ToLowerInvariant();
                var tagExists = db.Tags.Any(x => x.Name.Contains(searchByToLower));
                if (!tagExists)
                {
                    return new List<GameViewModel>();
                }
                List<int> tagIdList;
                var iQueryableTags = from t in db.Tags
                                      where t.Name.ToLower().Contains(searchByToLower)
                                      select
                                      t.Id;
                tagIdList = iQueryableTags.ToList();
                var gameIds = db.TagItems.Where(x => tagIdList.Contains(x.TagId)).Select(x => x.GameId).ToList();
                var iQueryableGames = from g in db.Games
                                      where gameIds.Contains(g.Id)
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
        /// <summary>
        /// Get View Modelsfor all Games with title containing searchBy to populate the Catalog Index
        /// </summary>
        /// <param name="searchBy">String to search for in game titles</param>
        /// <returns></returns>
        public List<GameViewModel> GetAllGameViewModelsWithTitle(string searchBy)
        {
            List<GameViewModel> result;
            var searchByToLower = searchBy.ToLower();
            using (var db = new ApplicationDbContext())
            {
                var iQueryableGames = from g in db.Games
                                      where g.Title.ToLower().Contains(searchByToLower)
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
        /// <summary>
        /// Populate Manage Games table with Simple Game View Models search by Title
        /// </summary>
        /// <param name="searchString">string to search for in game titles</param>
        /// <returns></returns>
        public List<SimpleGameViewModel> GetSimpleGamesListWithTitle(string searchString)
        {
            var searchStringToLower = searchString.ToLower();
            List<SimpleGameViewModel> gamesList = new List<SimpleGameViewModel>();
            using (var db = new ApplicationDbContext())
            {
                var result = from g in db.Games
                             where g.Title.ToLower().Contains(searchStringToLower)
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
        /// <summary>
        /// Populate Manage Games table with Simple Game View Models search by Developer
        /// </summary>
        /// <param name="searchString">string to search for in Game.Developer</param>
        /// <returns></returns>
        public List<SimpleGameViewModel> GetSimpleGamesListWithDeveloper(string searchString)
        {
            var searchStringToLower = searchString.ToLower();
            List<SimpleGameViewModel> gamesList = new List<SimpleGameViewModel>();
            using (var db = new ApplicationDbContext())
            {
                var result = from g in db.Games
                             where g.Developer.ToLower().Contains(searchStringToLower)
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
        /// <summary>
        /// Populate Manage Games table with Simple Game View Models search by Publisher
        /// </summary>
        /// <param name="searchString">search string to be contained in Game.Publisher</param>
        /// <returns></returns>
        public List<SimpleGameViewModel> GetSimpleGamesListWithPublisher(string searchString)
        {
            var searchStringToLower = searchString.ToLower();
            List<SimpleGameViewModel> gamesList = new List<SimpleGameViewModel>();
            using (var db = new ApplicationDbContext())
            {
                var result = from g in db.Games
                             where g.Publisher.ToLower().Contains(searchStringToLower)
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
        /// <summary>
        /// Populate Manage Games table with Simple Game View Models
        /// In case of empty search string
        /// </summary>
        /// <returns></returns>
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
        //Update Game from View Model, including image path
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
        //Check if Game has any Editions
        public bool HasEditions(int Id)
        {
            bool hasEditions = true;
            using (var db = new ApplicationDbContext())
            {
                hasEditions = db.GameEditions.Any(e => e.GameId == Id);
            }
            return hasEditions;
        }
        //Check if Game has any Orders, regardless of Order status
        public bool HasOrders(int Id)
        {
            bool hasOrders = true;
            using (var db = new ApplicationDbContext())
            {
                hasOrders = db.OrderItems.Any(o=>o.GameEdition.GameId == Id);
            }
            return hasOrders;
        }
        //Try to delete Game and return false if the game has editions
        //We do not check for Orders, because it is the Game Editions that are ordered
        public bool TryDeleteGame(GameViewModel game)
        {
            bool isDeleted = true;
            using (var db = new ApplicationDbContext())
            {
                var hasEditions = db.GameEditions.Any(e => e.GameId == game.Id);
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
        //Get Game Title by Id
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