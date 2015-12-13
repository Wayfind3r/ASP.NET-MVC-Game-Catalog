using System.ComponentModel.DataAnnotations;

namespace PotatoCatalog.Models
{
    public class Edition
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}