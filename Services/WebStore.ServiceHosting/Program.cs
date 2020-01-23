using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace WebStore.ServiceHosting
{
    /// <summary>Класс программы</summary>
    public class Program
    {
        /// <summary> Точка входа в программу </summary>
        /// <param name="args">Аргументы командной строки</param>
        public static void Main(string[] args) => 
            CreateWebHostBuilder(args)
                .Build()
                .Run();

        /// <summary>
        /// Метод создания генератора WebHost
        /// </summary>
        /// <param name="args">Аргументы</param>
        /// <returns>Экземпляр генератора WebHost</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
