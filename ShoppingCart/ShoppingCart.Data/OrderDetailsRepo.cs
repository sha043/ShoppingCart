using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data.Contracts;
using ShoppingCart.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data
{
    public class OrderDetailsRepo : IOrderDetailsRepo
    {
        private readonly ShoppingCartDbContext _context;
        public OrderDetailsRepo(ShoppingCartDbContext context)
        {
            _context = context;
        }
        public async Task<int?> AddNewOrderItemAsync(OrderDetails orderItem)
        {
            var orderItemSaved = await _context.OrderDetails.AddAsync(orderItem);
            await _context.SaveChangesAsync();
            return orderItemSaved.Entity.OrderDetailsId;
        }

        public async Task<int?> DeleteOrderDetailsAsync(int orderdetailsId)
        {
            var orderItemDetails = _context.OrderDetails.Where(t => t.OrderDetailsId == orderdetailsId).FirstOrDefault();
            if (orderItemDetails != null)
            {
                _context.OrderDetails.Remove(orderItemDetails);
                await _context.SaveChangesAsync();
                return orderItemDetails.OrderDetailsId;
            }
            return null;
        }

        public async Task<List<OrderDetails>> GetAllOrderDetailsByIDsAsync(int orderId)
        {
            return await _context.OrderDetails
                .Where(otm => otm.Order.OrderId == orderId)
                .Include(product => product.Product)
               .ToListAsync();
        }

        public async Task<OrderDetails> GetOrderDetailsByIdAsync(int orderdetailsId)
        {
            return await _context.OrderDetails
              .Where(otm => otm.OrderDetailsId == orderdetailsId)
              .Include(product => product.Product)
               .Include(order => order.Order)
             .FirstOrDefaultAsync();
        }

        public async Task<bool?> UpdateOrderDetails(int orderDetailsId, OrderDetails orderDetails)
        {
            var existingOrderDetails = _context.OrderDetails.Where(x => x.OrderDetailsId == orderDetailsId).FirstOrDefault();
            if (existingOrderDetails != null)
            {
                existingOrderDetails.ProductQuantity = orderDetails.ProductQuantity;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
