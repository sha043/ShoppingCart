using MediatR;
using ShoppingCart.Data.Contracts;

namespace ShoppingCart.Commands
{
    public class UpdateShippingAddressCommand: IRequest<Response>
    {
        public int OrderId { get; set; }
        public string ShippingAddress { get; set; }
        public class UpdateShippingAddressCommandHandler : IRequestHandler<UpdateShippingAddressCommand,  Response>
        {
            private readonly IOrderRepo _iOrderRepo;
            public UpdateShippingAddressCommandHandler(IOrderRepo iOrderRepo)
            {
                _iOrderRepo = iOrderRepo;
            }
            public async Task< Response> Handle(UpdateShippingAddressCommand request, CancellationToken cancellationToken)
            {
                var order= _iOrderRepo.GetOrderByIdAsync(request.OrderId).Result;
                if(order == null)
                {
                    return (new Response( false, "Order item not found"));
                }
                else
                {
                    order.ShippingAddress = request.ShippingAddress;
                    await _iOrderRepo.UpdateOrder(request.OrderId, order);
                    return (new Response(true, "Successfully updated the shipping address for order :  " + request.OrderId.ToString()));
                }
            }
        }

    }
}
