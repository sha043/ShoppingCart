namespace ShoppingCart.Request
{
    public class UpdateOrderRequest
    {
        public int OrderId { get; set; }
        public List<OrderDetailsRequest> orderDetails { get; set; }
    }
}
