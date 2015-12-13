using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PotatoCatalog.Models
{
    public class Game
    {
        public Game()
        {
            TagItems = new HashSet<TagItem>();
        }
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public string Publisher { get; set; }
        public string Developer { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Description { get; set; }
        public int NumberOfTags { get; set; }
        public virtual ICollection<TagItem> TagItems { get; set; }
        public string ImgPath { get; set; }

    }
}