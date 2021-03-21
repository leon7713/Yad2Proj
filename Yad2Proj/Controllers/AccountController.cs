using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public LoginModel model;

        public AccountController(ILogger<AccountController> logger, IRepositoryOf<int, Product> products, IRepositoryOf<int, User> users)
        {
            _logger = logger;
            _products = products;
            _users = users;
            model = new LoginModel();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return RedirectToAction("ShowAll", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string Username, string Password)
        {
            var user = _users.GetAll().SingleOrDefault(u => u.UserName == Username &&
            u.Password == Password);

            if (user == null)
            {
                TempData["Message"] = "Username or password is incorrect!";
                return RedirectToAction("Login", "Account");
            }
            //return Unauthorized();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Authentication, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Password),
            };

            var identity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = model.RememberLogin });

            #region Save user id to cookie & delete current guest
            int.TryParse(Request.Cookies.Where(x => x.Key == "UserId").FirstOrDefault().Value, out int guestId);
            _users.Delete(guestId);
            Response.Cookies.Append("UserId", $"{user.Id}");
            #endregion

            return RedirectToAction("ShowAll", "Home");
        }

        [HttpGet]
        public IActionResult PersonalDetails()
        {
            ViewBag.MainName = "Personal Details";

            var user = _users.GetAll().FirstOrDefault(x => x.UserName == @User.Claims.FirstOrDefault().Value);

            return View(user);
        }

        [HttpPost]
        public IActionResult PersonalDetails(User user)
        {
            ViewBag.MainName = "Personal Details";

            if (ModelState.IsValid)
            {
                _users.Update(user.Id, user);

                TempData["Message"] = "User updated.";

                return RedirectToAction(nameof(PersonalDetails));
            }

            return View(user);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var redirect = RedirectToAction();
            redirect.ActionName = "ShowAll";
            redirect.ControllerName = "Home";
            return redirect;
        }
    }
}


