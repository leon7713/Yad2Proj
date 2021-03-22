using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceScopeFactory _scopeFactory;

        //Injecting IServiceScopeFactory to get IRepositoryOf
        //because it is IRepositoryOf is scoped and ICartProductsService is singleton
        public CartProductsService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _data = new List<Product>();
            Init();
        }
        private void Init()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                //Deleting old guests
                var usersDb = scope.ServiceProvider.GetRequiredService<IRepositoryOf<int, User>>();
                var prodsDb = scope.ServiceProvider.GetRequiredService<IRepositoryOf<int, Product>>();
                List<User> oldGuests = usersDb.GetAll().Where(x => x.UserType == UserType.Guest).ToList<User>();
                foreach (User user in oldGuests)
                {
                    var guestProducts = prodsDb.GetAll().Where(x => x.User == user).ToList<Product>();
                    foreach (Product product in guestProducts)
                    {
                        product.User = null;
                        prodsDb.Update(product.Id, product);
                    }
                    usersDb.Delete(user.Id);
                }
                //Exporting all products in carts from DB on start
                _data.AddRange(prodsDb.GetAll().Where(x => x.User != null).ToList<Product>());
            }
        }

        public void Add(Product product)
        {
            _data.Add(product);
        }
        public void Remove(int productId)
        {
            Product p = _data.Where(x => x.Id == productId).FirstOrDefault();
            _data.Remove(p);
        }
    }
}
