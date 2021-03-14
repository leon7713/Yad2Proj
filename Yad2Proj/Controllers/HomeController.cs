﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Yad2Proj.Data;
using Yad2Proj.Models;
using System.Linq;
using System;

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
            var p = products.GetById(4);
            
            FileStream fs = new FileStream(@"C:\Users\yotam\Pictures\pic.jpg", FileMode.Open, FileAccess.Read);
            MemoryStream ms = new MemoryStream();

            p.Image1 = new byte[fs.Length];
            fs.Read(p.Image1, 0, (int)fs.Length);
            ms.Write(p.Image1, 0, (int)fs.Length);
            _products.Update(4, p);
            _products.Update(5, p);
        }

        [AllowAnonymous]
        //[Authorize] //no controller can be accessed if the user isn't authenticated
        public IActionResult Index()
        {
            ViewBag.MainName = "Main Page";

            return View();

            //var redirect = RedirectToAction();
            //redirect.ActionName = "Login"; // or can use nameof("") like  nameof(YourAction);
            //redirect.ControllerName = "Account"; // or can use nameof("") like  nameof(YourCtrl);
            //return redirect;
        }

        [AllowAnonymous]
        public IActionResult ShowAll()
        {
            //var u = _users.GetById(1);
            //_products.Create(new Product()
            //{ Title = "Product", Price = 100, ShortDesc = "New item", LongDesc = "Long description", Timestamp = BitConverter.GetBytes(DateTime.Now.Ticks), Owner = u });
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
        public IActionResult PersonalDetails()
        {
            ViewBag.MainName = "Personal Details";
            User user = new User();
            return View(user);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult PersonalDetails(User user)
        {
            ViewBag.MainName = "Personal Details";
            _users.Create(user);
            return View();
        }
    }
}
