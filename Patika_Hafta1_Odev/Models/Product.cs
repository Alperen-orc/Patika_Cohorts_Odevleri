using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Patika_Hafta1_Odev.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        [Required]
        public decimal Price { get; set; }
        public string Description { get; set; }=string.Empty;

    }
}
