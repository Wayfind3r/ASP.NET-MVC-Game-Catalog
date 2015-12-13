using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace PotatoCatalog.Models
{
       
    public class SimpleGameViewModel
    {
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Publisher")]
        public string Publisher { get; set; }
        [Required]
        [Display(Name = "Developer")]
        public string Developer { get; set; }
        [Required]
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }
    }

    public class GameViewModel : IComparable<GameViewModel>
    {
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Publisher")]
        public string Publisher { get; set; }

        [Required]
        [Display(Name = "Developer")]
        public string Developer { get; set; }

        [Required]
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int NumberOfTags { get; set; } = 0;
        [Display(Name = "Box Art")]
        public string ImagePath { get; set; } = null;

        [Display(Name = "Tags")]
        public List<TagItemViewModel> Tags { get; set; }

        public int CompareTo(GameViewModel model)
        {
            int result = String.Compare(this.Title, model.Title, StringComparison.OrdinalIgnoreCase);
            return result;
        }
    }

    public class CompleteGameViewModel
    {
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Publisher")]
        public string Publisher { get; set; }

        [Required]
        [Display(Name = "Developer")]
        public string Developer { get; set; }

        [Required]
        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-mm-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int NumberOfTags { get; set; } = 0;
        [Display(Name = "Box Art")]
        public string ImagePath { get; set; } = null;

        [Display(Name = "Tags")]
        public List<TagItemViewModel> Tags { get; set; }
        [Display(Name = "Available Editions")]
        public List<GameEditionViewModel> Editions { get; set; }
    }
}