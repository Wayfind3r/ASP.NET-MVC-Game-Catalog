using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PotatoCatalog.Enums;

namespace PotatoCatalog.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string Address { get; set; }
        public decimal PriceInPotatoes { get; set; }
        [Required]
        public virtual int OrderStatusId
        {
            get
            {
                return (int)this.OrderStatus;
            }
            set
            {
                OrderStatus = (OrderStatus)value;
            }
        }
        [EnumDataType(typeof(OrderStatus))]
        public OrderStatus OrderStatus { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}