using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    /// <summary>API контроллер товаров</summary>
    //[Route("api/[controller]")]
    [Route("api/Products")]
    [ApiController]
    public class ProductsApiController : ControllerBase, IProductData
    {
        private readonly IProductData _ProductData;

        /// <summary>Конструктор контролера</summary>
        /// <param name="ProductData">Интерфейс взаимодействия с сервисом хранения товаров</param>
        public ProductsApiController(IProductData ProductData) => _ProductData = ProductData;

        /// <summary>Получение брендов из системы</summary>
        /// <returns>Перечисление брендов</returns>
        [HttpGet("brands")]
        public IEnumerable<Brand> GetBrands()
        {
            return _ProductData.GetBrands();
        }

        /// <summary>Получение секций из системы</summary>
        /// <returns>Перечисление секций</returns>
        [HttpGet("sections")]
        public IEnumerable<Section> GetSections()
        {
            return _ProductData.GetSections();
        }

        /// <summary> Получение отфильтрованных товаров </summary>
        /// <param name="Filter">Фильтр, может отсуствовать</param>
        /// <returns>Перечисление товаров</returns>
        [HttpPost, ActionName("Post")]
        public PagedProductDTO GetProducts([FromBody] ProductFilter Filter = null)
        {
            return _ProductData.GetProducts(Filter);
        }

        /// <summary>Получение товара по идентификатору</summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Товар</returns>
        [HttpGet("{id}"), ActionName("Get")]
        public ProductDTO GetProductById(int id)
        {
            return _ProductData.GetProductById(id);
        }

        /// <summary>Добавление товара</summary>
        /// <param name="product">Модель товара</param>
        [HttpPut, ActionName("Put")]
        public Task AddProduct(ProductDTO product)
        {
            return _ProductData.AddProduct(product);
        }

        /// <summary>Удаление товара по идентификатору</summary>
        /// <param name="id">Идентификатор</param>
        [HttpDelete("{id}")]
        public Task RemoveProduct(int id)
        {
            return _ProductData.RemoveProduct(id);
        }

        /// <summary>Изменеие товара по идентификатору</summary>
        /// <param name="id">Идентификатор</param>
        /// <param name="product">Изменнёный товар</param>
        [HttpPut("{id}"), ActionName("Put")]
        public Task UpdateProduct(int id, [FromBody] ProductDTO product)
        {
            return _ProductData.UpdateProduct(id, product);
        }

        /// <summary>
        /// Получение секции по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Секция с заданным идентификатором</returns>
        [HttpGet("sections/{id}")]
        public Section GetSectionById(int id) => _ProductData.GetSectionById(id);

        /// <summary>
        /// Получение бренда по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns>Бренд с заданным идентификатором</returns>
        [HttpGet("brands/{id}")]
        public Brand GetBrandById(int id) => _ProductData.GetBrandById(id);
    }
}