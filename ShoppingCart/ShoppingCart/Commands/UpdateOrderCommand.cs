using MediatR;
using ShoppingCart.Data.Contracts;
using ShoppingCart.Entities;
using ShoppingCart.Request;

namespace ShoppingCart.Commands
{
    public class UpdateOrderCommand: IRequest<Response>
    {
        public int OrderId { get; set; }
        public List<OrderDetailsRequest> orderDetails { get; set; }
        public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Response>
        {
            private readonly IOrderRepo _iOrderRepo;
            private readonly IOrderDetailsRepo _iOrderDetailsRepo;
            private readonly IProductRepo _iProductRepo;
            public UpdateOrderCommandHandler(IOrderDetailsRepo iOrderDetailsRepo, IProductRepo iProductRepo,IOrderRepo iOrderRepo)
            {
                _iOrderDetailsRepo = iOrderDetailsRepo;
                _iProductRepo = iProductRepo;
                _iOrderRepo = iOrderRepo;
            }
            public async Task<Response> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
            {
                var order = _iOrderRepo.GetOrderByIdAsync(request.OrderId).Result;
                if (order == null)
                {
                    return new Response(false, "Order not found");
                }
                else
                {
                    foreach (var orderDetailsRequest in request.orderDetails)
                    {
                        var existingOrderDetails = order.OrderDetailsList.Where(x => x.ProductId == orderDetailsRequest.ProductId).FirstOrDefault();
                        if(existingOrderDetails == null)
                        {
                            if (!_iProductRepo.CheckProductStock(orderDetailsRequest.ProductId, orderDetailsRequest.Quantity).Result)
                            {
                                return (new Response(false, "Product is out of stock for product ID : " + orderDetailsRequest.ProductId));
                            }
                            else
                            {
                                var orderItem = new OrderDetails
                                {
                                    OrderId = request.OrderId,
                                    ProductId = orderDetailsRequest.ProductId,
                                    ProductQuantity = orderDetailsRequest.Quantity
                                };
                                var orderItemID = await _iOrderDetailsRepo.AddNewOrderItemAsync(orderItem);
                                var product = new Product();
                                product = await _iProductRepo.GetProductByID(orderDetailsRequest.ProductId);
                                product.ItemsLeft = (product.ItemsLeft - orderDetailsRequest.Quantity);
                                await _iProductRepo.UpdateProduct(orderDetailsRequest.ProductId, product);
                            }
                        }
                        else
                        {
                            //TODO : move the common logic to out of if else.
                            if(orderDetailsRequest.Quantity > existingOrderDetails.ProductQuantity)
                            {
                                if (!_iProductRepo.CheckProductStock(orderDetailsRequest.ProductId, (orderDetailsRequest.Quantity-existingOrderDetails.ProductQuantity)).Result)
                                {
                                    return (new Response(false, "Product is out of stock for product ID : " + orderDetailsRequest.ProductId));
                                }
                                else
                                {
                                    existingOrderDetails.ProductQuantity = orderDetailsRequest.Quantity;
                                    await _iOrderDetailsRepo.UpdateOrderDetails (existingOrderDetails.OrderDetailsId, existingOrderDetails);
                                    var product = await _iProductRepo.GetProductByID(orderDetailsRequest.ProductId);
                                    product.ItemsLeft = (product.ItemsLeft - (orderDetailsRequest.Quantity - existingOrderDetails.ProductQuantity));
                                    await _iProductRepo.UpdateProduct(orderDetailsRequest.ProductId, product);
                                }
                            }
                            else
                            {
                                existingOrderDetails.ProductQuantity = orderDetailsRequest.Quantity;
                                await _iOrderDetailsRepo.UpdateOrderDetails(existingOrderDetails.OrderDetailsId, existingOrderDetails);
                                var product = await _iProductRepo.GetProductByID(orderDetailsRequest.ProductId);
                                product.ItemsLeft = (product.ItemsLeft + (existingOrderDetails.ProductQuantity - orderDetailsRequest.Quantity));
                                await _iProductRepo.UpdateProduct(orderDetailsRequest.ProductId, product);
                            }
                            
                        }

                    }
                }
                return (new Response(true, "Successfully updated the order :  " + request.OrderId.ToString()));
            }
        }
    }
}
