using System.Collections.Generic;
using Yad2Proj.Models;

namespace Yad2Proj.Data
{
    public interface ICartProductsService
    {
        List<Product> GetAll { get; }
        void Add(Product product);
        void Remove(int productId);
    }
}