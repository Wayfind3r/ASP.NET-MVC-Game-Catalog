using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using PotatoCatalog.Models;
using PotatoCatalog.Services;

namespace PotatoCatalog.Models
{
    public class GameEditionViewModel
    {
        [Display(Name = "ID")]
        public int Id { get; set; }
        public int GameId { get; set; }
        public int EditionId { get; set; }
        [Display(Name = "Edition Type")]
        public string EditionTitle { get; set; }
        [Required]
        [Display(Name = "Price In Potatoes")]
        public decimal PriceInPotatoes { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Contents")]
        public string Contents { get; set; }
        [Display(Name = "Has pending orders?")]
        public bool isPending { get; set; }
    }

    public class CreateGameEditionViewModel
    {
        public int GameId { get; set; }
        [Required]
        [Display(Name = "Edition Type")]
        public int EditionId { get; set; }
        [Required]
        [Display(Name = "Price In Potatoes")]
        public decimal PriceInPotatoes { get; set; }
        [Required]
        [Display(Name = "Contents")]
        [DataType(DataType.MultilineText)]
        public string Contents { get; set; }
        public SelectList EditionsList { get; set; }

    }

    public class GameEditionCartViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Game Title")]
        public string GameTitle { get; set; }
        [Display(Name = "Edition")]
        public string EditionName { get; set; }
        [Display(Name = "Price In Potatoes")]
        public decimal PriceInPotatoes { get; set; }
        [Display(Name = "Contents")]
        [DataType(DataType.MultilineText)]
        public string Contents { get; set; }
    }
    public class GameEditionViewModelBag
    {
        public List<GameEditionViewModel> List { get; set; }

        public int GameId { get; set; }
        public string GameTitle { get; set; }
        private GameEditionViewModelBag()
        {
        }

        public GameEditionViewModelBag(int id)
        {
            var serv = new GameEditionServices();
            var gameServ = new GameServices();
            GameId = id;
            GameTitle = gameServ.GetGameTitle(id);
            List = serv.GetGameEditions(id);
        }
    }
}