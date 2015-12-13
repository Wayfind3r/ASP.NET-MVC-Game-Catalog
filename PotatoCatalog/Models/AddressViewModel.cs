using System.ComponentModel.DataAnnotations;

namespace PotatoCatalog.Models
{
    public class AddressViewModel
    {
        [Required(ErrorMessage = "Please enter an imaginary address")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Imaginary Address")]
        public string Address { get; set; }
    }
}