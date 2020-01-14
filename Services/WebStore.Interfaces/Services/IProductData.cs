using System.Collections.Generic;
using System.Threading.Tasks;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();

        IEnumerable<Brand> GetBrands();

        IEnumerable<Product> GetProducts(ProductFilter Filter = null);

        Product GetProductById(int id);

        Task AddProduct(Product product);

        Task UpdateProduct(int id, Product product);

        Task RemoveProduct(int id, Product product);
    }
}
