using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MVCProekt.Models
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        [Display(Name = "Product Name")]
        public string? ProductName { get; set; }
        public int? Price { get; set; }

        [MaxLength(100)]
        [Required]
        [Display(Name = "Product Image")]
        public string? ProductImage { get; set; }

        public int CookId { get; set; }
        public Cook? Cook { get; set; }

        public ICollection<ProductCategory>? Categories { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<UserProduct>? UserProducts { get; set; }
    }
}
