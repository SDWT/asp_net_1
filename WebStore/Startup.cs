using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // For all types files
            app.UseStaticFiles(/*new StaticFileOptions { ServeUnknownFileTypes = true}*/);
            app.UseStaticFiles();
            app.UseDefaultFiles();

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
