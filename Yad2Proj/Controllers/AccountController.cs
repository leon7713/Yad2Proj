using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Yad2Proj.Data;
using Yad2Proj.Models;

namespace Yad2Proj.Controllers
{
   public class AccountController : Controller
   {
      private readonly ILogger<AccountController> _logger;
      private readonly IRepositoryOf<int, Product> _products;
      private readonly IRepositoryOf<int, User> _users;
      LoginModel model;

      public AccountController(ILogger<AccountController> logger, IRepositoryOf<int, Product> products, IRepositoryOf<int, User> users)
      {
         _logger = logger;
         _products = products;
         _users = users;
         model = new LoginModel();
      }

      [AllowAnonymous]
      public IActionResult Login(string returnUrl = "/")
      {
         //return View(new LoginModel { ReturnUrl = returnUrl });
         return PartialView("_Login");
      }

      [HttpPost]
      [AllowAnonymous]
      public async Task<IActionResult> Login(string Username, string Password)
      {
         var user = _users.GetAll().SingleOrDefault(u => u.UserName == Username &&
         u.Password == Password);

         if (user == null)
         {
            var redir = RedirectToAction();
            redir.ActionName = "ShowAll"; // or can use nameof("") like  nameof(YourAction);
            redir.ControllerName = "Home"; // or can use nameof("") like  nameof(YourCtrl);
            return redir;
         }
         //return Unauthorized();

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
             new AuthenticationProperties { IsPersistent = model.RememberLogin });

         //return LocalRedirect(model.ReturnUrl);

         var redirect = RedirectToAction();
         redirect.ActionName = "ShowAll"; // or can use nameof("") like  nameof(YourAction);
         redirect.ControllerName = "Home"; // or can use nameof("") like  nameof(YourCtrl);
         return redirect;
      }
   }
}
