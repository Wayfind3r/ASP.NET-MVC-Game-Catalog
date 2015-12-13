using System.ComponentModel.DataAnnotations;
using PotatoCatalog.Enums;

namespace PotatoCatalog.Models
{
    public class OrderViewModel
    {
         public int Id { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "Price in Potatoes")]
        public decimal PriceInPotatoes { get; set; }
        [Display(Name = "Order Status")]
        public OrderStatus OrderStatus { get; set; }
        [Display(Name = "User E-mail")]
        public string UserEmail { get; set; }
    }
}