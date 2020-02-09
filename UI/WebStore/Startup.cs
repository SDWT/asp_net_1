using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using WebStore.Clients.Employees;
//using WebStore.Infrastructure.Conventions;
using WebStore.Clients.Identity;
using WebStore.Clients.Orders;
using WebStore.Clients.Products;
using WebStore.Clients.Values;
using WebStore.Domain.Entities.Identity;
using WebStore.Infrastructure.Middleware;
using WebStore.Interfaces.Api;
using WebStore.Interfaces.Services;
using WebStore.Logger;
using WebStore.Services.Product;

namespace WebStore
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration Config)
        {
            Configuration = Config;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression(
                opt =>
                {
                    opt.EnableForHttps = true;
                    opt.ExcludedMimeTypes = new[] { "application/jpg" };
                    //opt.Providers.Add<>();
                });

            services.AddSingleton<IEmployeesData, EmployeesClient>();
            services.AddScoped<IProductData, ProductsClient>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICartStore, CookiesCartStore>();
            services.AddScoped<IOrderService, OrdersClient>();

            services.AddTransient<IValuesService, ValuesClient>();

            #region Custom implementation identity storages
            services.AddTransient<IUserStore<User>, UsersClient>();
            services.AddTransient<IUserRoleStore<User>, UsersClient>();
            services.AddTransient<IUserClaimStore<User>, UsersClient>();
            services.AddTransient<IUserPasswordStore<User>, UsersClient>();
            services.AddTransient<IUserEmailStore<User>, UsersClient>();
            services.AddTransient<IUserPhoneNumberStore<User>, UsersClient>();
            services.AddTransient<IUserTwoFactorStore<User>, UsersClient>();
            services.AddTransient<IUserLoginStore<User>, UsersClient>();
            services.AddTransient<IUserLockoutStore<User>, UsersClient>();

            services.AddTransient<IRoleStore<Role>, RolesClient>();
            #endregion

            services.AddIdentity<User, Role>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opt =>
            {
                // Упрощения правил пароля для упрощения тестирования
                opt.Password.RequiredLength = 3;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredUniqueChars = 3;

                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);

                // Список доступных символов
                //opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABC123";
                opt.User.RequireUniqueEmail = false; // Грабли - на этапе отладки при попытке регистрации двух пользователей без email
            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "WebStore_Identity";
                opt.Cookie.HttpOnly = true;
                opt.Cookie.Expiration = TimeSpan.FromDays(100);

                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";

                opt.SlidingExpiration = true;
            });

            //services.AddSingleton<TInterface, TImplementation>(); // Единый объект на всё время жизни приложения с момента первого обращения к нему
            //services.AddTransient<>(); // Один объект на каждый запрос экземпляра сервиса
            //services.AddScoped<>(); // Один объект на время обработки одного входящего запроса (на время действия области)


            services.AddSession();

            services.AddMvc(
                opt =>
                {
                    //opt.Conventions.Add(new CustomControllerConvention());
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory log)
        {
            log.AddLog4Net();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            app.UseResponseCompression();

            // For all types files
            app.UseStaticFiles(/*new StaticFileOptions { ServeUnknownFileTypes = true}*/);
            app.UseDefaultFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseSession();
            app.UseErrorHandlingMiddleware();

            #region Middleware - примеры

            //app.UseAuthentication();
            //app.UseResponseCompression();

            app.UseWelcomePage("/Welcome");
            #endregion
            //app.Run(async context => await context.Response.WriteAsync("Hello World!")); // Безусловное выполнение (замыкает конвейер)
            app.Map("/Hello", application => application.Run(async ctx => await ctx.Response.WriteAsync("World!")));

            //app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                    );
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                       name: "employees",
                       template: "Employees/Index");
                // ? - опционально
                // ! - обязательно
                // = по умолчанию
            });
        }
    }
}
