using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Yad2Proj.Data;
using Yad2Proj.Models;
using Yad2Proj.Utilities;

namespace Yad2Proj.Controllers
{
   [Authorize]
   public class HomeController : Controller
   {
      private readonly ILogger<HomeController> _logger;
      private readonly IRepositoryOf<int, Product> _products;
      private readonly IRepositoryOf<int, User> _users;
      private readonly ICartProductsService _cart;
      private readonly IGuestGenerator _guestGen;

      public HomeController(ILogger<HomeController> logger,
          IRepositoryOf<int, Product> products, IRepositoryOf<int, User> users,
          IMemoryCache memoryCache, ICartProductsService cartProductsService,
          IGuestGenerator guestGenerator)
      {
         _logger = logger;
         _products = products;
         _users = users;
         _cart = cartProductsService;
         _guestGen = guestGenerator;
      }


      public IActionResult Index()
      {
         ViewBag.MainName = "Main Page";
         return RedirectToAction(nameof(ShowAll));
      }

      [AllowAnonymous]
      public IActionResult ShowAll(int orderBy)
      {
         ViewBag.MainName = "All Products List";
         if (!User.Identity.IsAuthenticated && Request.Cookies.Where(x => x.Key == "uid").FirstOrDefault().Value == null)
         {
            User newGuest = _guestGen.GenUser(_users);
            Response.Cookies.Append("uid", $"{newGuest.Id}");
         }
         List<Product> products = _products.GetAll().ToList<Product>();
         foreach (Product item in _cart.GetAll)
         {
            products.RemoveAll(x => x.Id == item.Id);
         }
         switch (orderBy)
         {
            case 1:
               products.OrderBy(p => p.Timestamp);
               break;
            default:
               products = products.OrderBy(p => p.Title).ToList<Product>();
               break;
         }
         return View(products);
      }


      public IActionResult Privacy()
      {
         ViewBag.MainName = "Main Page";
         return View();
      }

      [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
      public IActionResult Error()
      {
         return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
      }

      [HttpGet]
      [AllowAnonymous]
      public IActionResult Register()
      {
         ViewBag.MainName = "Register";

         if (User.Identity.IsAuthenticated)
         {
            return RedirectToAction("Index");
         }
         return View(new User());
      }

      [HttpPost]
      [AllowAnonymous]
      public async Task<IActionResult> Register(User user)
      {
         ViewBag.MainName = "Register";

         if (ModelState.IsValid)
         {
            var existUser = _users.GetAll().FirstOrDefault(u => u.UserName == user.UserName);

            if (existUser == null)
            {
               _users.Create(user);
               ModelState.Clear();
               var claims = new List<Claim>
               {
                   new Claim(ClaimTypes.NameIdentifier, user.UserName),
                   new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                   new Claim(ClaimTypes.NameIdentifier, user.Password),
               };

               var identity = new ClaimsIdentity(claims,
                   CookieAuthenticationDefaults.AuthenticationScheme);
               var principal = new ClaimsPrincipal(identity);

               await HttpContext.SignInAsync(
                   CookieAuthenticationDefaults.AuthenticationScheme,
                   principal,
                   new AuthenticationProperties { IsPersistent = true });

               #region Save user id to cookie
               Response.Cookies.Append("uid", $"{user.Id}");
               #endregion

               return RedirectToAction("ShowAll", "Home");
            }
            else
            {
               ViewBag.ErrorMessage = "User with this username already exists!";
            }
         }
         return View(user);
      }

      [AllowAnonymous]
      public IActionResult AboutUs()
      {
         ViewBag.MainName = "About Us";
         return View();
      }
      //private User GenerateGuestUser()
      //{
      //    return _guestGen.GenUser();
      //}
   }
}
