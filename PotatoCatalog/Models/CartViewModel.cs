namespace PotatoCatalog.Models
{
    public class CartViewModel
    {
         public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class Potatoes
    {
        public decimal Amount { get; set; }
    }
}