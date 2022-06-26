using MediatR;
using ShoppingCart.Data.Contracts;
using ShoppingCart.Entities;

namespace ShoppingCart.Commands
{
    public class DeleteOrderDetailsCommand: IRequest<(int?, Response)>
    {
        public int OrderDetailsId { get; set; }
        public class DeleteOrderDetailsCommandHandler : IRequestHandler<DeleteOrderDetailsCommand, (int?, Response)>
        {
            private readonly IOrderDetailsRepo _iOrderDetailsRepo;
            private readonly IProductRepo _iProductRepo;
            public DeleteOrderDetailsCommandHandler(IOrderDetailsRepo iOrderDetailsRepo, IProductRepo iProductRepo)
            {
                _iOrderDetailsRepo = iOrderDetailsRepo;
                _iProductRepo = iProductRepo;
            }
            public async Task<(int?, Response)> Handle(DeleteOrderDetailsCommand request, CancellationToken cancellationToken)
            {
                var orderItemDetails = _iOrderDetailsRepo.GetOrderDetailsByIdAsync(request.OrderDetailsId).Result;

                if (orderItemDetails == null)
                {
                    return (null, new Response( false, "Order details not found"));
                }
                else if (orderItemDetails.Order.IsShipped)
                {
                    return (null, new Response(false, "Order has been shipped already. Cannot be canclled now."));
                }
                else
                {
                    int? orderItemID = await _iOrderDetailsRepo.DeleteOrderDetailsAsync(request.OrderDetailsId);
                    Product product = new Product();
                    product = await _iProductRepo.GetProductByID(orderItemDetails.ProductId);
                    product.ItemsLeft = (product.ItemsLeft + orderItemDetails.ProductQuantity);
                    await _iProductRepo.UpdateProduct(product.ProductId, product);
                    return (orderItemID, new Response( true, null));
                }
            }
        }
    }
}
