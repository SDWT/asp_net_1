using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Map;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductData _ProductData;
        private readonly IConfiguration _Configuration;

        public HomeController(IProductData ProductData, IConfiguration Configuration)
        {
            _ProductData = ProductData;
            _Configuration = Configuration;
        }

        public IActionResult Index(int? SectionId = null, int? BrandId = null, int Page = 1)
        {
            var page_size = int.TryParse(_Configuration["PageSize"], out var size) ? size : (int?)null;

            var products = _ProductData.GetProducts(new ProductFilter
            {
                SectionId = SectionId,
                BrandId = BrandId,
                Page = Page,
                PageSize = page_size
            });

            return View(new HomeIndexViewModel
            {
                Catalog = new CatalogViewModel
                {
                    SectionId = SectionId,
                    BrandId = BrandId,
                    Products = products.Products.Select(ProductMapper.ToViewModel).OrderBy(p => p.Order),
                    PageViewModel = new PageViewModel
                    {
                        PageSize = page_size ?? 0,
                        PageNumber = Page,
                        TotalItems = products.TotalCount
                    }
                }
            });
        }

        public IActionResult Blog() => View();

        public IActionResult BlogSingle() => View();

        public IActionResult ContactUs() => View();

        public IActionResult Error404() => View();

        public IActionResult ErrorStatus(string Id)
        {
            switch (Id)
            {
                default: return Content($"Статусный код {Id}");
                case "404":
                    return RedirectToAction(nameof(Error404));
            }
        }

        public IActionResult ThrowException() => throw new ApplicationException("Тестовая ошибка в программе");

    }
}