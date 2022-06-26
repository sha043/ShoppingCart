using MediatR;
using ShoppingCart.Data.Contracts;

namespace ShoppingCart.Commands
{
    public class DeleteOrderCommand: IRequest<(int?, Response)>
    {
        public int OrderId { get; set; }
        public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand,(int?,Response)>
        {
            private readonly IOrderRepo _iOrderRepo;
            public DeleteOrderCommandHandler(IOrderRepo iOrderRepo)
            {
                _iOrderRepo = iOrderRepo;
            }

            public async Task<(int?, Response)> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
            {
                var order = _iOrderRepo.GetOrderByIdAsync(request.OrderId).Result;

                if (order == null)
                {
                    return (null, new Response( false, "Order not found"));
                }               
                else
                {
                    int? orderId = await _iOrderRepo.DeleteOrderAsync(request.OrderId);
                    return (orderId, new Response(true, null));
                }
            }
        }
    }
}
