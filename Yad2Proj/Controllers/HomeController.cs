using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Yad2Proj.Models;

namespace Yad2Proj.Controllers
{
   public class HomeController : Controller
   {
      private readonly ILogger<HomeController> _logger;

      public HomeController(ILogger<HomeController> logger)
      {
         _logger = logger;
      }

      public IActionResult Index()
      {
         ViewBag.MainName = "Main Page";
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
         return View();
      }
   }
}
