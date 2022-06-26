using ShoppingCart.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Entities;

namespace ShoppingCart.Data
{
    public class ProductRepo : IProductRepo
    {
        private readonly ShoppingCartDbContext _context;
        public ProductRepo(ShoppingCartDbContext context)
        {
            this._context = context;
        }
        public async Task<bool> CheckProductStock(int productId, int requiredCount)
        {
            var availableStock = await _context.Product
               .Where(p => p.ProductId == productId)
               .Select(t => t.ItemsLeft).FirstOrDefaultAsync();

            if (availableStock >= requiredCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<Product> GetProductByID(int productId)
        {
            return await _context.Product
                .Where(x => x.ProductId == productId)
                .FirstOrDefaultAsync();
        }
        public async Task<int?> UpdateProduct(int productId, Product product)
        {
            var existingProduct = _context.Product.Where(p => p.ProductId == productId).FirstOrDefault();
            if (existingProduct != null)
            {
                existingProduct.ItemsLeft = product.ItemsLeft;
                await _context.SaveChangesAsync();
                return existingProduct.ProductId;
            }
            return null;
        }
    }
}
