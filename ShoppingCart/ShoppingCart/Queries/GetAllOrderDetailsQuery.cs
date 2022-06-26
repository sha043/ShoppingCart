using MediatR;
using ShoppingCart.Data.Contracts;
using ShoppingCart.Entities;

namespace ShoppingCart.Queries
{
    public class GetAllOrderDetailsQuery:IRequest<List<Order>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public class GetAllOrdersQueryQueryHandler : IRequestHandler<GetAllOrderDetailsQuery, List<Order>>
        {
            private readonly IOrderRepo _iOrderRepo;
            public GetAllOrdersQueryQueryHandler(IOrderRepo iOrderRepo)
            {
                _iOrderRepo = iOrderRepo;
            }
            public async Task<List<Order>> Handle(GetAllOrderDetailsQuery request, CancellationToken cancellationToken)
            {

                return await _iOrderRepo.GetAllOrdersAsync(request.PageNumber, request.PageSize);
            }
        }
    }
}
