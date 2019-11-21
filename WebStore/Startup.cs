using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using WebStore.Infrastructure.Conventions;
using WebStore.Infrastructure.Interfaces;
using WebStore.Infrastructure.Services;

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
            services.AddSingleton<IEmployeesData, InMemoryEmployeesData>();

            //services.AddSingleton<TInterface, TImplementation>(); // Единый объект на всё время жизни приложения с момента первого обращения к нему
            //services.AddTransient<>(); // Один объект на каждый запрос экземпляра сервиса
            //services.AddScoped<>(); // Один объект на время обработки одного входящего запроса (на время действия области)


            services.AddMvc(
                opt =>
                {
                    //opt.Conventions.Add(new CustomControllerConvention());
                });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }

            // For all types files
            app.UseStaticFiles(/*new StaticFileOptions { ServeUnknownFileTypes = true}*/);
            app.UseDefaultFiles();
            app.UseCookiePolicy();

            #region Middleware - примеры

            //app.UseAuthentication();
            //app.UseResponseCompression();

            app.UseWelcomePage("/Welcome");
            #endregion
            //app.Run(async context => await context.Response.WriteAsync("Hello World!")); // Безусловное выполнение (замыкает конвейер)
            app.Map("/Hello", application => application.Run(async ctx => await ctx.Response.WriteAsync("World!")));

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync(Configuration["CustomData"]);
            //});

            //app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>
            {
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
