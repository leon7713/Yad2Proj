﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Yad2Proj.Data;
using Yad2Proj.Extension;
using Yad2Proj.Models;
using Yad2Proj.Models.ProductViewModels;

namespace Yad2Proj.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepositoryOf<int, Product> _products;
        private readonly IRepositoryOf<int, User> _users;
        private readonly ICartProductsService _cart;

        public ProductController(ILogger<HomeController> logger,
           IRepositoryOf<int, Product> products,
           IRepositoryOf<int, User> users,
           ICartProductsService cart)
        {
            _logger = logger;
            _products = products;
            _users = users;
            _cart = cart;
            //var p = products.GetById(4);

            //FileStream fs = new FileStream(@"C:\Users\yotam\Pictures\pic.jpg", FileMode.Open, FileAccess.Read);
            //MemoryStream ms = new MemoryStream();


            //p.Image1 = new byte[fs.Length];
            //fs.Read(p.Image1, 0, (int)fs.Length);
            //ms.Write(p.Image1, 0, (int)fs.Length);
            //_products.Update(4, p);
        }

        [Authorize]
        public IActionResult AddItem()
        {
            ViewBag.MainName = "Add Item's Page";

            Product product = new Product();
            return View(product);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddItem(ProductViewModel productViewModel)
        {
            ViewBag.MainName = "Add Item's Page";

            if (ModelState.IsValid)
            {
                var userId = 0;

                if (User.HasClaim(p => p.Type == ClaimTypes.Authentication))
                {
                    userId = int.Parse(User.FindFirst(ClaimTypes.Authentication).Value);
                }

                var owner = _users.GetById(userId);

                var product = new Product
                {
                    //Id = productViewModel.Id,
                    Owner = owner,
                    User = null,
                    Title = productViewModel.Title,
                    ShortDesc = productViewModel.ShortDesc,
                    LongDesc = productViewModel.LongDesc,
                    Price = productViewModel.Price,
                    Timestamp = DateTime.Now,
                    InCart = productViewModel.InCart,
                    State = productViewModel.State,
                };

                if (productViewModel.Image1 != null)
                    product.Image1 = await productViewModel.Image1.GetBytes();
                if (productViewModel.Image2 != null)
                    product.Image2 = await productViewModel.Image2.GetBytes();
                if (productViewModel.Image3 != null)
                    product.Image3 = await productViewModel.Image3.GetBytes();

                _products.Create(product);
                //ShowSuccessPopup();

                TempData["Message"] = "Product success added.";

                return RedirectToAction(nameof(AddItem));
            }

            return View(new Product());
        }

        public IActionResult GetImage(int id, int size)
        {
            var product = _products.GetById(id);

            if (product == null)
                return NotFound();

            byte[] image = null;

            if (size == 1) image = product.Image1;
            else if (size == 2) image = product.Image2;
            else if (size == 3) image = product.Image3;

            if (image == null)
                return base.File("~/Contents/noImg.png", "image/png");

            //Dictionary<string, string> extensionsWithMiMeType = new Dictionary<string, string>()
            //{
            //   {".jpeg", "image/jpeg"},
            //   {".jpg", "image/jpeg"},
            //   {".png", "image/png"},
            //   {".gif", "image/gif"}
            //};
            //var fileExtension = System.IO.Path.GetExtension("filename.jpg").ToLower();

            return new FileContentResult(image, "image/jpeg");
        }

        //[AllowAnonymous]
        //public IActionResult GetImage2(int id)
        //{
        //   var product = _products.GetById(id);

        //   if (product == null || product.Image2 == null)
        //      return base.File("~/Contents/noImg.png", "image/jpg");

        //   return new FileContentResult(product.Image2, "image/jpg");
        //}

        //[AllowAnonymous]
        //public IActionResult GetImage3(int id)
        //{
        //   var product = _products.GetById(id);

        //   if (product == null || product.Image3 == null)
        //      return base.File("~/Contents/noImg.png", "image/jpg");

        //   return new FileContentResult(product.Image3, "image/jpg");
        //}

        [HttpGet]
        public IActionResult Details(int id)
        {
            ViewBag.MainName = "More Details";
            var productWithUser = _products.GetByIdJoin(p => p.Id == id, u => u.Owner).FirstOrDefault();
            if (productWithUser == null)
            {
                NotFound();
            }
            return View(productWithUser);
        }

        // ERROR FOR UNAUTHORIZED USERS !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        
        public IActionResult Cart(int id)
        {
            ViewBag.MainName = "More Details";
            User user = GetCurrentUser();
            List<Product> products = new List<Product>();
            if (user != null)
            {
                products = _products.GetAll().Where(p => p.User == user).ToList();
            }
            return View(products);
        }
        public IActionResult AddToCart(int id)
        {
            Product p = _products.GetById(id);
            if (_cart.GetAll.Where(x => x.Id == id).FirstOrDefault() != null)
            {
                return RedirectToAction("ShowAll", "Home", "The selected item is no longer available");
            }
            p.User = GetCurrentUser();
            _products.Update(id, p);
            _cart.Add(p);
            return RedirectToAction("ShowAll", "Home");
        }
        public IActionResult RemoveFromCart(int productId, int userId)
        {
            Product p = _products.GetByIdJoin(p => p.Id == productId, u => u.User).FirstOrDefault();
            _cart.Remove(p.Id);
            p.User = null;
            _products.Update(productId, p);
            return RedirectToAction(nameof(Cart));
        }
        public IActionResult Purchase(int userId)
        {
            User user = _users.GetById(userId);
            List<Product> userProducts = _products.GetAll().Where(x => x.User == user).ToList();
            foreach (Product item in userProducts)
            {
                _products.Delete(item.Id);
                _cart.Remove(item.Id);
            }
            return View();
        }
        private User GetCurrentUser()
        {
            int userId = 0;
            if (User.HasClaim(p => p.Type == ClaimTypes.Authentication))
            {
                userId = int.Parse(User.FindFirst(ClaimTypes.Authentication).Value);
            }
            else
            {
                userId = int.Parse(Request.Cookies.Where(x => x.Key == "uid").FirstOrDefault().Value);
            }
            return _users.GetById(userId);
        }
    }
}
