using System.ComponentModel.DataAnnotations;

namespace PotatoCatalog.Models
{
    public class UserTableViewModel
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Email")]
         public string Email { get; set; }
        [Display(Name = "Potatoes")]
         public decimal Potatoes { get; set; }
        [Display(Name = "Number of Orders")]
         public int NumberOfOrders { get; set; }
    }
}