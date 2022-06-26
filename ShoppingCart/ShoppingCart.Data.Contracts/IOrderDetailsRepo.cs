using ShoppingCart.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Contracts
{
    public interface IOrderDetailsRepo
    {
        Task<int?> AddNewOrderItemAsync(OrderDetails orderItem);
        Task<List<OrderDetails>> GetAllOrderDetailsByIDsAsync(int orderId);
        Task<OrderDetails> GetOrderDetailsByIdAsync(int orderdetailsId);
        Task<int?> DeleteOrderDetailsAsync(int orderdetailsId);
        Task<bool?> UpdateOrderDetails(int orderDetailsId, OrderDetails orderDetails);
    }
}
