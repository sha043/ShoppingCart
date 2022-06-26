using MediatR;
using ShoppingCart.Data.Contracts;
using ShoppingCart.Entities;

namespace ShoppingCart.Queries
{
    public class GetOrderDetailsByIdQuery: IRequest<Order>
    {
        public int OrderId { get; set; }
        public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderDetailsByIdQuery, Order>
        {
            private readonly IOrderRepo _iOrderRepo;

            public GetOrderByIdQueryHandler(IOrderRepo iOrderRepo)
            {
                _iOrderRepo = iOrderRepo;
            }
            public async Task<Order> Handle(GetOrderDetailsByIdQuery request, CancellationToken cancellationToken)
            {
                return await _iOrderRepo.GetOrderByIdAsync(request.OrderId);
            }
        }
    }
}
