using ASP.Data;
using ASP.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<IdentityUser, IdentityRole>()
               .AddDefaultUI()
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddDefaultTokenProviders();

            services.AddDbContext<BooksContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("BooksConnection")));

            services.AddControllersWithViews().AddRazorRuntimeCompilation(); ;
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider services)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            CreateUserRoles(services).Wait();
        }
        private async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string[] roleNames = { "Admin", "User", "Employee" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            IdentityUser user = await UserManager.FindByEmailAsync("fa.andrzejczak@gmail.com");
            if (user == null)
            {
                user = new IdentityUser()
                {
                    UserName = "fa.andrzejczak@gmail.com",
                    Email = "fa.andrzejczak@gmail.com",
                };
                await UserManager.CreateAsync(user, "ZAQ!2wsx");
            }
            await UserManager.AddToRoleAsync(user, "Admin");
            IdentityUser user1 = await UserManager.FindByEmailAsync("speedman09@o2.pl");
            if (user1 == null)
            {
                user1 = new IdentityUser()
                {
                    UserName = "speedman09@o2.pl",
                    Email = "speedman09@o2.pl",
                };
                await UserManager.CreateAsync(user1, "ZAQ!2wsx");
            }
            await UserManager.AddToRoleAsync(user1, "Employee");
            IdentityUser user2 = await UserManager.FindByEmailAsync("usertest@gmail.com");
            if (user2 == null)
            {
                user2 = new IdentityUser()
                {
                    UserName = "usertest@gmail.com",
                    Email = "usertest@gmail.com",
                };
                await UserManager.CreateAsync(user2, "ZAQ!2wsx");
            }
            await UserManager.AddToRoleAsync(user2, "User");
        }
    }
}
