using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCart.Entities
{
    [Table("Product")]
    public class Product
    {
        public int ProductId { get; set; }
        public string? Name { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal Price { get; set; }
        public int ItemsLeft { get; set; }

    }
}