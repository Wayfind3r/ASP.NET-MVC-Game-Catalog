using System.ComponentModel.DataAnnotations;

namespace PotatoCatalog.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }
        public int GameEditionId { get; set; }
        public virtual GameEdition GameEdition { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int Quantity { get; set; }
        public decimal PriceInPotatoes { get; set; }
    }
}