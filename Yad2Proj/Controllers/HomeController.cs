using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Diagnostics;
using Yad2Proj.Data;
using Yad2Proj.Models;

namespace Yad2Proj.Controllers
{
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
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ShowAll()
        {
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
      public IActionResult PersonalDetails()
      {
         ViewBag.MainName = "Personal Details";
         User user = new User();
         return View(user);
      }

      [HttpPost]
      public IActionResult PersonalDetails(User user)
      {
         ViewBag.MainName = "Personal Details";
         _users.Create(user);
         return View();
      }
   }
}
