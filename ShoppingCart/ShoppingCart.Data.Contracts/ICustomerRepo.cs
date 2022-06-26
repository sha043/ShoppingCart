using ShoppingCart.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Contracts
{
    public interface ICustomerRepo
    {
        Task<bool> CheckIfValidCustomerByIdAsync(int customerID);
    }
}
