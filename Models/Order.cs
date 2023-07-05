using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MVCProekt.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(450)]
        public string Username { get; set; }

        [Display(Name = "Phone Number")]
        public int? PhoneNumber { get; set; }

        [Required]
        [MaxLength(450)]
        public string Address { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Date Order")]
        public DateTime? DateOrder { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
