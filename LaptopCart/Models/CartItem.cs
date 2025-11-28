using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopCart.Models
{
    public class CartItem
    {
        [key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        
        public Product? Product { get; set; }
        public string? UserId {  get; set; }
        public int Quantity {  get; set; }

    }
}
