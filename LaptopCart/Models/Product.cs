using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopCart.Models
{
    public class Product
    {
        [key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Product Name is Required")]
        public string? Name {  get; set; }
        [Required(ErrorMessage = "Product Description is required")]
        public string? Description  { get; set; }
        
        public decimal? Price { get; set; }

        public String? ImagePath { get; set; }
        public DateTime CreatedAt {  get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }

    internal class keyAttribute : Attribute
    {
    }
}
