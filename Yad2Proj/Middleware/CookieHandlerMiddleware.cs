using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Yad2Proj.Data.Repository;
using Yad2Proj.Models;
using Yad2Proj.Services;

namespace Yad2Proj.Middleware
{
    public class CookieHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IGuestGenerator _guestGenerator;
        private HttpContext _context;
        KeyValuePair<string, string> _uidCookie;
        ICartProductsService _cart;

        public CookieHandlerMiddleware(RequestDelegate next, IGuestGenerator guestGenerator, ICartProductsService cart)
        {
            _next = next;
            _guestGenerator = guestGenerator;
            _cart = cart;
        }
        public async Task InvokeAsync(HttpContext context, IRepositoryOf<int, User> dbContext)
        {
            _context = context;
            //Check if no cookie with key == "uid"
            if (context.Request.Cookies.Where(x => x.Key == "uid").FirstOrDefault().Value == null && context.Request.Path.Value != "/Home/ShowAll")
            {
                string id = "";
                //If normal user then we update the cookie
                if (context.User.Identity.IsAuthenticated)
                {
                    string userId = context.User.FindFirst(ClaimTypes.Authentication).Value;
                    _uidCookie = new KeyValuePair<string, string>("uid", userId);
                }

                //If guest user we create new guest
                else
                {
                    User guest = _guestGenerator.GenUser(dbContext);
                    _uidCookie = new KeyValuePair<string, string>("uid", guest.Id.ToString());
                    id = guest.Id.ToString();
                }
                //Add the new cookie
                _context.Response.Cookies.Append("uid", id);
            }
            await _next(context);
        }

    }
    public static class CookieMiddlewareExtension
    {
        public static IApplicationBuilder UseCookieHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CookieHandlerMiddleware>();
        }
    }

}
