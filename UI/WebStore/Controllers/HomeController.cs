using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;
using WebStore.Domain.ViewModels;
using WebStore.Domain.Entities;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductData _ProductData;

        public HomeController(IProductData ProductData) => _ProductData = ProductData;

        public IActionResult Index(int? SectionId, int? BrandId)
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
                    Products = products.Select(p => new ProductViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Order = p.Order,
                        ImageUrl = p.ImageUrl,
                        Price = p.Price
                    }).OrderBy(p => p.Order)
                }
            });
        }

        public IActionResult Blog() => View();

        public IActionResult BlogSingle() => View();

        public IActionResult ContactUs() => View();

        public IActionResult Error404() => View();

        public IActionResult ThrowException() => throw new ApplicationException("Тестовая ошибка в программе");

    }
}