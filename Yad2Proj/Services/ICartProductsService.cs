using System.Collections.Generic;
using Yad2Proj.Models;

namespace Yad2Proj.Services
{
    public interface ICartProductsService
    {
        List<Product> GetAll { get; }
        void Add(Product product);
        void Remove(int productId);
    }
}