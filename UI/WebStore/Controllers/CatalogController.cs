using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Map;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;

        public CatalogController(IProductData ProductData) => _ProductData = ProductData;

        public IActionResult Shop(int? SectionId, int? BrandId)
        {
            var products = _ProductData.GetProducts(new ProductFilter
            {
                SectionId = SectionId,
                BrandId = BrandId
            });

            return View(new CatalogViewModel
            {
                SectionId = SectionId,
                BrandId = BrandId,
                Products = products.Products.Select(ProductMapper.ToViewModel).OrderBy(p => p.Order)
            });
        }

        public IActionResult Details(int id, [FromServices] ILogger<CatalogController> Logger)
        {
            var product = _ProductData.GetProductById(id);

            if (product is null)
            {
                Logger.LogWarning("Запрошенный товар {0} отсутствует в каталоге", id);
                return NotFound();
            }

            Logger.LogInformation("Товар {0} найден: {1}", id, product.Name);

            return View(product.ToViewModel());
        }
    }
}