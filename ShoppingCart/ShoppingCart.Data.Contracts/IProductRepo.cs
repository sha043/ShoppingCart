using ShoppingCart.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Contracts
{
    public interface IProductRepo
    {
        Task<bool> CheckProductStock(int productID, int requiredCount);
        Task<Product> GetProductByID(int productID);
        Task<int?> UpdateProduct(int productId, Product product);
    }
}
