using ShoppingCart.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Contracts
{
    public interface IOrderRepo
    {
        Task<Order> GetOrderByIdAsync(int orderid);
        Task<List<Order>> GetAllOrdersAsync(int pageNumber, int pageSize);
        Task<int?> AddNewOrderAsync(Order order);
        Task<int?> DeleteOrderAsync(int orderId);
        Task<bool?> UpdateOrder(int orderId, Order order);
    }
}
