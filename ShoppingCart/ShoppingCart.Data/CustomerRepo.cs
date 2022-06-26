using Microsoft.EntityFrameworkCore;
using ShoppingCart.Data.Contracts;
using ShoppingCart.Entities;

namespace ShoppingCart.Data
{
    public class CustomerRepo : ICustomerRepo
    {

        private readonly ShoppingCartDbContext _context;
        public CustomerRepo(ShoppingCartDbContext context)
        {
            this._context = context;
        }
        public async Task<bool> CheckIfValidCustomerByIdAsync(int customerID)
        {
            return await _context.Customer.AnyAsync(x => x.CustomerId == customerID);               
        }
    }
}