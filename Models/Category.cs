using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MVCProekt.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        public ICollection<ProductCategory>? Categories { get; set; }
    }
}
