using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using Yad2Proj.Data;
using Yad2Proj.Models;
using Microsoft.Owin;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Yad2Proj.Controllers
{
   [Authorize] //no controller can be accessed if the user isn't authenticated
   public class HomeController : Controller
   {
      private readonly ILogger<HomeController> _logger;
      private readonly IRepositoryOf<int, Product> _products;
      private readonly IRepositoryOf<int, User> _users;

      public HomeController(ILogger<HomeController> logger, IRepositoryOf<int, Product> products, IRepositoryOf<int, User> users)
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
      public IActionResult Index()
      {
         ViewBag.MainName = "Main Page";
         return View();
      }

      [AllowAnonymous]
      public IActionResult ShowAll()
      {
         ViewBag.MainName = "All Products List";
         var products = _products.GetAll();
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

               //return LocalRedirect(model.ReturnUrl);

               #region Save user id to cookie
               Response.Cookies.Append("UserId", $"{user.Id}");
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
   }
}
