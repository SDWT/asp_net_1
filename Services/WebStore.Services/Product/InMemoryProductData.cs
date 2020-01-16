using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Services.DataBase;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Product
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Section> GetSections() => TestData.Sections;

        public IEnumerable<Domain.Entities.Product> GetProducts(ProductFilter Filter = null)
        {
            var query = TestData.Products;

            if (Filter?.SectionId != null)
                query = query.Where(product => product.SectionId == Filter.SectionId);

            if (Filter?.BrandId != null)
                query = query.Where(product => product.BrandId == Filter.BrandId);

            return query;
        }

        public Domain.Entities.Product GetProductById(int id) => TestData.Products.FirstOrDefault(p => p.Id == id);

        public Task AddProduct(Domain.Entities.Product product)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateProduct(int id, Domain.Entities.Product product)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveProduct(int id, Domain.Entities.Product product)
        {
            throw new System.NotImplementedException();
        }
    }
}
