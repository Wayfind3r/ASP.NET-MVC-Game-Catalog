using System.ComponentModel.DataAnnotations;

namespace PotatoCatalog.Models
{
    public class GameEdition
    {

        [Key]
        public int Id { get; set; }
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
        public int EditionId { get; set; }
        public virtual Edition Edition { get; set; }
        public decimal PriceInPotatoes { get; set; }
        public string Contents { get; set; }
    }
}