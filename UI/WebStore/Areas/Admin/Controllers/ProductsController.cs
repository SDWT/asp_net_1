using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Map;

namespace WebStore.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = Role.Administrator)]
    public class ProductsController : Controller
    {
        private readonly IProductData _ProductData;

        public ProductsController(IProductData ProductData)
        {
            _ProductData = ProductData;
        }

        // GET: Admin/Products
        public IActionResult Index()
        {
            int? page_size = null;
            var Page = 0;

            var products = _ProductData.GetProducts(new ProductFilter
            {
                Page = Page,
                PageSize = page_size
            });

            var model = new CatalogViewModel
            {
                Products = products.Products.Select(ProductMapper.ToViewModel).OrderBy(p => p.Order),
                PageViewModel = new PageViewModel
                {
                    PageSize = page_size ?? 0,
                    PageNumber = Page,
                    TotalItems = products.TotalCount
                }
            };
            return base.View(model);
        }

        // GET: Admin/Products/Details/5
        public IActionResult Details(int? id)
        {
            if (id is null)
                return NotFound();

            var product = _ProductData.GetProductById((int)id);

            if (product is null)
                return NotFound();

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_ProductData.GetBrands(), "Id", "Name");
            ViewData["SectionId"] = new SelectList(_ProductData.GetSections(), "Id", "Name");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDTO product)
        {
            if (ModelState.IsValid)
            {
                await _ProductData.AddProduct(product);
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_ProductData.GetBrands(), "Id", "Name", product.Brand.Id);
            ViewData["SectionId"] = new SelectList(_ProductData.GetSections(), "Id", "Name", product.Section.Id);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var product = _ProductData.GetProductById((int)id);
            if (product is null)
            {
                return NotFound();
            }
            ViewData["BrandId"] = new SelectList(_ProductData.GetBrands(), "Id", "Name", product.Brand.Id);
            ViewData["SectionId"] = new SelectList(_ProductData.GetSections(), "Id", "Name", product.Section.Id);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductDTO product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _ProductData.UpdateProduct(id, product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_ProductData.GetBrands(), "Id", "Name", product.Brand.Id);
            ViewData["SectionId"] = new SelectList(_ProductData.GetSections(), "Id", "Name", product.Section.Id);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var product = _ProductData.GetProductById((int)id);
            if (product is null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _ProductData.RemoveProduct(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            var product = _ProductData.GetProductById((int)id);
            if (product is null)
                return false;
            return true;
        }
    }
}
