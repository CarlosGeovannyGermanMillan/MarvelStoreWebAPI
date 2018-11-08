using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TiendaMarvel.Data;
using TiendaMarvel.Models;
using TiendaMarvel.Services;
using TiendaMarvel.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace TiendaMarvel
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
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<ICategoriasRepository,AzureCategoriasRepository>();
            services.AddTransient<IProductosRepository,AzureProductosRepository>();
            services.AddTransient<IPedidosRepository, AzPedidosRepository>();

            services.AddTransient<CarritoService>(sr => {
                var ctx = sr.GetService<IHttpContextAccessor>();
                var session = ctx.HttpContext.Session;
                var pedidos = sr.GetService<IPedidosRepository>();

                var usuario = ctx.HttpContext.User?.Identity?.Name ?? string.Empty;
                return new CarritoService(session, pedidos, usuario);
            });
            
            services.AddMvc()
                .AddSessionStateTempDataProvider();
            services.AddSession();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env
            , UserManager<ApplicationUser> userManager
            , RoleManager<IdentityRole> roleManager
            , ApplicationDbContext dbContext
        )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


            
            dbContext.Database.EnsureCreated();
            IdentityInitializer.SeedRoles(roleManager);
            IdentityInitializer.SeedUsers(userManager);
        }
    }
}
