using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Map;

namespace WebStore.Services.Product
{
    public class SqlProductData : IProductData // Unit of Work
    {
        private readonly WebStoreContext _db;

        public SqlProductData(WebStoreContext db) => _db = db;

        public IEnumerable<Brand> GetBrands() => _db.Brands
            //.Include(brand => brand.Products)
            .AsEnumerable();

        public IEnumerable<Section> GetSections() => _db.Sections
            //.Include(section => section.Products)
            .AsEnumerable();

        public PagedProductDTO GetProducts(ProductFilter Filter = null)
        {
            IQueryable<Domain.Entities.Product> query = _db.Products;

            if (Filter?.BrandId != null)
                query = query.Where(product => product.BrandId == Filter.BrandId);

            if (Filter?.SectionId != null)
                query = query.Where(product => product.SectionId == Filter.SectionId);

            var total_count = query.Count();

            if (Filter?.PageSize != null)
                query = query
                    .Skip((Filter.Page - 1) * (int)Filter.PageSize)
                    .Take((int)Filter.PageSize);

            return new PagedProductDTO
            {
                Products = query
                    .Include(p => p.Brand)
                    .Include(p => p.Section)
                    .Select(ProductMapper.ToDTO)
                    .AsEnumerable(),
                TotalCount = total_count
            };
        }

        public ProductDTO GetProductById(int id) => _db.Products
            .Include(p => p.Brand)
            .Include(p => p.Section)
            .FirstOrDefault(p => p.Id == id)
            .ToDTO();

        public async Task AddProduct(ProductDTO product)
        {
            var p = new Domain.Entities.Product
            {
                Id = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Order = product.Order,
                Price = product.Price,
                Brand = _db.Brands.FirstOrDefault(b => b.Id == product.Brand.Id),
                Section = _db.Sections.FirstOrDefault(s => s.Id == product.Section.Id),
            };
            _db.Add(p);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateProduct(int id, ProductDTO product)
        {
            var p = new Domain.Entities.Product
            {
                Id = product.Id,
                Name = product.Name,
                ImageUrl = product.ImageUrl,
                Order = product.Order,
                Price = product.Price,
                Brand = _db.Brands.FirstOrDefault(b => b.Id == product.Brand.Id),
                Section = _db.Sections.FirstOrDefault(s => s.Id == product.Section.Id),
            };
            _db.Update(p);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveProduct(int id)
        {
            var product = _db.Products.FirstOrDefault(p => p.Id == id);
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }

        public Section GetSectionById(int id) => _db.Sections.FirstOrDefault(section => section.Id == id);

        public Brand GetBrandById(int id) => _db.Brands.FirstOrDefault(brand => brand.Id == id);
    }
}
