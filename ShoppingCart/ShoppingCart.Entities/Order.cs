using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Entities
{
    [Table("Order")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public bool IsShipped { get; set; }
        public string ShippingAddress { get; set; }

        public DateTime Date { get; set; }
        public virtual IList<OrderDetails>? OrderDetailsList { get; set; }
        public Customer? Customer { get; set; }
    }
}
