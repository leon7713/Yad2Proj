﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Yad2Proj.Data;
using Yad2Proj.Extension;
using Yad2Proj.Models;
using Yad2Proj.Models.ProductViewModels;

namespace Yad2Proj.Controllers
{
   [Authorize]
   public class ProductController : Controller
   {
      private readonly ILogger<HomeController> _logger;
      private readonly IRepositoryOf<int, Product> _products;
      private readonly IRepositoryOf<int, User> _users;

      public ProductController(ILogger<HomeController> logger,
         IRepositoryOf<int, Product> products,
         IRepositoryOf<int, User> users)
      {
         _logger = logger;
         _products = products;
         _users = users;
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

      [HttpPost]
      public async Task<IActionResult> AddItem(ProductViewModel productViewModel)
      {
         ViewBag.MainName = "Add Item's Page";

         if (ModelState.IsValid)
         {
            var userId = int.Parse(Request.Cookies["UserId"]); //34
            //int userId = int.Parse(User.Claims.FirstOrDefault(x => x.Type == "Authentication").Value);
            var owner = _users.GetById(userId);

            var product = new Product
            {
               //Id = productViewModel.Id,
               Owner = owner,
               User = owner,
               Title = productViewModel.Title,
               ShortDesc = productViewModel.ShortDesc,
               LongDesc = productViewModel.LongDesc,
               Price = productViewModel.Price,
               Timestamp = productViewModel.Timestamp,
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
         }

         return View();
      }

      [AllowAnonymous]
      public IActionResult GetImage1(int id)
      {
         var product = _products.GetById(id);

         if (product == null || product.Image1 == null)
            return base.File("~/Contents/noImg.png", "image/jpg");

         return new FileContentResult(product.Image1, "image/jpg");
      }

      [AllowAnonymous]
      public IActionResult GetImage2(int id)
      {
         var product = _products.GetById(id);

         if (product == null || product.Image2 == null)
            return base.File("~/Contents/noImg.png", "image/jpg");

         return new FileContentResult(product.Image2, "image/jpg");
      }

      [AllowAnonymous]
      public IActionResult GetImage3(int id)
      {
         var product = _products.GetById(id);

         if (product == null || product.Image3 == null)
            return base.File("~/Contents/noImg.png", "image/jpg");

         return new FileContentResult(product.Image3, "image/jpg");
      }

      [HttpGet]
      [AllowAnonymous]
      public IActionResult Details(int id)
      {
         ViewBag.MainName = "More Details";
         var productWithUser = _products
            .GetByIdJoin(p => p.Id == id, u => u.Owner)
            .Where(p => p.Id == id)
            .First();

         if (productWithUser == null)
         {
            NotFound();
         }
         return View(productWithUser);
      }
   }
}