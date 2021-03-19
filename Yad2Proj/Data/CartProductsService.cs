using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yad2Proj.Models;

namespace Yad2Proj.Data
{
    public class CartProductsService : ICartProductsService
    {
        private readonly List<Product> _data;
        public List<Product> GetAll => _data;
        public CartProductsService()
        {
            _data = new List<Product>();
        }

        public void Add(Product product)
        {
            _data.Add(product);
        }
        public void Remove(Product product)
        {
            _data.Remove(product);
        }
    }
}
