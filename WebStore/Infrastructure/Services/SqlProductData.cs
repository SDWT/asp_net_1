using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Infrastructure.Interfaces;

namespace WebStore.Infrastructure.Services
{
    public class SqlProductData : IProductData // Unit of Work
    {
        private WebStoreContext _db;

        public SqlProductData(WebStoreContext db) => _db = db;

        public IEnumerable<Brand> GetBrands() => _db.Brands
            .Include(brand => brand.Products)
            .AsEnumerable();

        public IEnumerable<Section> GetSections() => _db.Sections
            .Include(section => section.Products)
            .AsEnumerable();

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Product> query = _db.Products;

            if (Filter?.BrandId != null)
                query = query.Where(product => product.BrandId == Filter.BrandId);

            if (Filter?.SectionId != null)
                query = query.Where(product => product.SectionId == Filter.SectionId);

            // query.ToArray();
            return query.AsEnumerable();
        }

        public Product GetProductById(int id) => _db.Products
           .Include(p => p.Brand)
           .Include(p => p.Section)
           .FirstOrDefault(p => p.Id == id);

    }
}
