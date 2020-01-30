using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;
using WebStore.Domain.ViewModels;
using WebStore.Domain.Entities;
using WebStore.Services.Map;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductData _ProductData;

        public HomeController(IProductData ProductData) => _ProductData = ProductData;

        public IActionResult Index(int? SectionId = null, int? BrandId = null)
        {
            var products = _ProductData.GetProducts(new ProductFilter
            {
                SectionId = SectionId,
                BrandId = BrandId
            });

            return View(new HomeIndexViewModel
            {
                Catalog = new CatalogViewModel
                {
                    SectionId = SectionId,
                    BrandId = BrandId,
                    Products = products.Select(ProductMapper.ToViewModel).OrderBy(p => p.Order)
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