using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MVCProekt.Models
{
    public class Cook
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(50)]
        public string? Gender { get; set; }

        public string FullName
        {
            get { return String.Format("{0} {1}", FirstName, LastName); }
        }

        // Not needed, check MSFT ASP.Net Core 6 documentation
        public ICollection<Product>? Products { get; set; }
    }
}
