using MediatR;
using ShoppingCart.Data.Contracts;
using ShoppingCart.Entities;
using ShoppingCart.Request;

namespace ShoppingCart.Commands
{
    public class AddNewOrderCommand : IRequest<(int?,Response)>
    {
        public virtual IList<OrderDetailsRequest> OrderDetails { get; set; }
        public int CustomerId { get; set; }
        public string ShippingAddress { get; set; }
        public class AddNewOrderCommandHandler:IRequestHandler<AddNewOrderCommand, (int?,Response)>
        {
            private readonly IOrderRepo _iOrderRepo;
            private readonly ICustomerRepo _iCustomerRepo;
            public AddNewOrderCommandHandler(IOrderRepo iOrderRepo, ICustomerRepo iCustomerRepo)
            {
                _iOrderRepo = iOrderRepo;
                _iCustomerRepo = iCustomerRepo;
            }

            public async Task<(int?,Response)> Handle(AddNewOrderCommand request, CancellationToken cancellationToken)
            {
                if (request.CustomerId<=0 || request.OrderDetails.Count<=0 )
                {
                    return (null, new Response(false, "Invalid customer id or order details are not complete"));
                }
                else if(!await _iCustomerRepo.CheckIfValidCustomerByIdAsync(request.CustomerId))
                {
                    return (null, new Response(false, "Customer is not registered."));
                }
                else
                {
                    var order = new Order { Date = DateTime.Now, CustomerId = request.CustomerId,ShippingAddress=request.ShippingAddress };
                    int? orderId = await _iOrderRepo.AddNewOrderAsync(order);
                    return (orderId, new Response(true, "Successfully added the order"));
                }
                
            }
        }
    }
}
