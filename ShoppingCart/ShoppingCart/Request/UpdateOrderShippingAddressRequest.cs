namespace ShoppingCart.Request
{
    public class UpdateOrderShippingAddressRequest
    {
        public int OrderId { get; set; }
        public string ShippingAddress { get; set; }
    }
}
