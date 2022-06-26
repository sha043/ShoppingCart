using MediatR;
using ShoppingCart.Data.Contracts;
using ShoppingCart.Entities;

namespace ShoppingCart.Commands
{
    public class AddNewOrderDetailsCommand : IRequest<(int?,Response)>
    {
        public int ProductId { get; set; }
        public int ProductQuantity { get; set; }
        public int OrderId { get; set; }
        public class AddNewOrderDetailsCommandHandler : IRequestHandler<AddNewOrderDetailsCommand, (int?,Response)>
        {
            private readonly IOrderDetailsRepo _iOrderDetailsRepo;
            private readonly IProductRepo _iProductRepo;
            public AddNewOrderDetailsCommandHandler(IOrderDetailsRepo iOrderDetailsRepo, IProductRepo iProductRepo)
            {
                _iOrderDetailsRepo = iOrderDetailsRepo;
                _iProductRepo = iProductRepo;
            }

            public async Task<(int?,Response)> Handle(AddNewOrderDetailsCommand request, CancellationToken cancellationToken)
            {
                int? orderItemID = null;
                if (request.OrderId <= 0 || request.ProductId <= 0 || request.ProductQuantity <= 0)
                {
                    return (null, new Response(false, "Incomplete order details"));
                }
                else if (!_iProductRepo.CheckProductStock(request.ProductId, request.ProductQuantity).Result)
                {
                    return (null, new Response(false, "Product is unavailable"));
                }
                else
                {
                    var orderItem = new OrderDetails
                    {
                        OrderId = request.OrderId,
                        ProductId = request.ProductId,
                        ProductQuantity = request.ProductQuantity
                    };
                    orderItemID = await _iOrderDetailsRepo.AddNewOrderItemAsync(orderItem);
                    var product = new Product();
                    product = await _iProductRepo.GetProductByID(request.ProductId);
                    product.ItemsLeft = (product.ItemsLeft - request.ProductQuantity);
                    await _iProductRepo.UpdateProduct(request.ProductId, product);
                    return (orderItemID, new Response( true, "Succesfully added the order details"));
                }                
            }
        }
    }
}
