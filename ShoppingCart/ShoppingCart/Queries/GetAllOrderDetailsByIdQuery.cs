using MediatR;
using ShoppingCart.Data.Contracts;
using ShoppingCart.Entities;

namespace ShoppingCart.Queries
{
    public class GetAllOrderDetailsByIdQuery: IRequest<List<OrderDetails>>
    {
        public int OrderId { get; set; }
        public class GetAllOrderDetailsByIdQueryHandler : IRequestHandler<GetAllOrderDetailsByIdQuery, List<OrderDetails>>
        {
            private readonly IOrderDetailsRepo _iOrderDetailsRepo;

            public GetAllOrderDetailsByIdQueryHandler(IOrderDetailsRepo iOrderDetailsRepo)
            {
                _iOrderDetailsRepo = iOrderDetailsRepo;
            }
            public async Task<List<OrderDetails>> Handle(GetAllOrderDetailsByIdQuery request, CancellationToken cancellationToken)
            {
                return await _iOrderDetailsRepo.GetAllOrderDetailsByIDsAsync(request.OrderId);
            }
        }

    }
}
