using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Yad2Proj.Data;
using Yad2Proj.Models;

namespace Yad2Proj.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EFRepositoryOf<int, Product> _products;
        private readonly EFRepositoryOf<int, User> _users;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            _products = new EFRepositoryOf<int, Product>();
            _users = new EFRepositoryOf<int, User>();
        }

        public IActionResult Index()
        {
            return View();
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
    }
}
