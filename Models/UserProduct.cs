using System.ComponentModel.DataAnnotations;

namespace MVCProekt.Models
{
    public class UserProduct
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }
        public int ProductId { get; set; }
        public Product? Product { get; set; }
    }
}
