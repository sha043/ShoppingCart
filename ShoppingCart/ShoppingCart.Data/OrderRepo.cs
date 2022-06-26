using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Data.Contracts;
using ShoppingCart.Entities;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCart.Data
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ShoppingCartDbContext _context;
        public OrderRepo(ShoppingCartDbContext context)
        {
            _context = context;
        }
        public async Task<Order> GetOrderByIdAsync(int orderID)
        {
            return await _context.Order.Where(x=>x.OrderId==orderID)
                .Include(cust => cust.Customer)
                .Include(order => order.OrderDetailsList)
                .ThenInclude(prod => prod.Product).FirstOrDefaultAsync();
            
        }
        public async Task<List<Order>> GetAllOrdersAsync(int pageNumber, int pageSize)
        {
            return await _context.Order
                .Include(cust => cust.Customer)
                .Include(order => order.OrderDetailsList)
                .ThenInclude(prod => prod.Product)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();  
        }

        public async Task<int?> AddNewOrderAsync(Order order)
        {
            var orderId=await _context.Order.AddAsync(order);
            await _context.SaveChangesAsync();
            return orderId.Entity.OrderId;
        }

        public async Task<int?> DeleteOrderAsync(int orderId)
        {
            var order = _context.Order.Where(t => t.OrderId == orderId).FirstOrDefault();
            if (order != null)
            {
                _context.Order.Remove(order);
                await _context.SaveChangesAsync();
                return order.OrderId;
            }
            return null;
        }

        public async Task<bool?> UpdateOrder(int orderId, Order order)
        {
            var existingOrder = _context.Order.Where(x => x.OrderId == orderId).FirstOrDefault();
            if(existingOrder != null)
            {
                existingOrder.ShippingAddress = order.ShippingAddress;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
