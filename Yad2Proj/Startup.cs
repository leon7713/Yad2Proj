using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Yad2Proj.Data;
using Yad2Proj.Models;

namespace Yad2Proj
{
   public class Startup
   {
      public Startup(IConfiguration configuration)
      {
         Configuration = configuration;

      }

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         //services.AddControllersWithViews(o => o.Filters.Add(new AuthorizeFilter()));
         services.AddControllersWithViews();
         services.AddScoped<IDbContextProvider, DbContextProvider>();
         services.AddScoped<IRepositoryOf<int, Product>, EFRepositoryOf<int, Product>>();
         services.AddScoped<IRepositoryOf<int, User>, EFRepositoryOf<int, User>>();
         services.AddDbContext<ProgramDbContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("myDb")));
         services.AddSingleton<ICartProductsService, CartProductsService>();
         services.AddMvc();
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

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

         app.UseRouting();

         app.UseAuthentication();
         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=ShowAll}/{id?}");
         });
      }
   }
}
