using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WebStore.Clients.Base;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Clients.Products
{
    public class ProductsClient : BaseClient, IProductData
    {
        public ProductsClient(IConfiguration config) : base(config, "api/Products") { }

        public IEnumerable<Brand> GetBrands() => Get<List<Brand>>($"{_ServiceAddress}/brands");

        public IEnumerable<Section> GetSections() => Get<List<Section>>($"{_ServiceAddress}/sections");

        public ProductDTO GetProductById(int id) => Get<ProductDTO>($"{_ServiceAddress}/{id}");

        public IEnumerable<ProductDTO> GetProducts(ProductFilter Filter = null) =>
            Post(_ServiceAddress, Filter is null ? new ProductFilter() : Filter)
            .Content
            .ReadAsAsync<List<ProductDTO>>()
            .Result;


        public async Task AddProduct(ProductDTO product) => await PutAsync(_ServiceAddress, product);

        public async Task RemoveProduct(int id) => await DeleteAsync($"{_ServiceAddress}/{id}");

        public async Task UpdateProduct(int id, ProductDTO product) => await PutAsync($"{_ServiceAddress}/{id}", product);

        public Section GetSectionById(int id) => Get<Section>($"{_ServiceAddress}/sections/{id}");

        public Brand GetBrandById(int id) => Get<Brand>($"{_ServiceAddress}/brands/{id}");
    }
}
