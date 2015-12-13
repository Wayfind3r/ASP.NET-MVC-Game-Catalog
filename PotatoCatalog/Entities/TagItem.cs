using System.ComponentModel.DataAnnotations;

namespace PotatoCatalog.Models
{
    public class TagItem
    {

        [Key]
        public int Id { get; set; }
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}