using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Yad2Proj.Data.Context;
using Yad2Proj.Data.Providers;
using Yad2Proj.Data.Repository;
using Yad2Proj.Middleware;
using Yad2Proj.Models;
using Yad2Proj.Services;

namespace Yad2Proj
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddScoped<IDbContextProvider, DbContextProvider>();
            services.AddScoped<IRepositoryOf<int, Product>, EFRepositoryOf<int, Product>>();
            services.AddScoped<IRepositoryOf<int, User>, EFRepositoryOf<int, User>>();

            services.AddDbContext<ProgramDbContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("myDb")));

            services.AddSingleton<ICartProductsService, CartProductsService>();
            services.AddSingleton<IGuestGenerator, GuestGenerator>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
               .AddCookie();

            services.AddRazorPages()
               .AddRazorRuntimeCompilation();

            services.AddRazorPages() //for nullable date
            .AddMvcOptions(options =>
            {
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
            _ => "Field is required.");
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            //Custom middleware for cookie handling
            app.UseCookieHandlerMiddleware();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

    }
}
