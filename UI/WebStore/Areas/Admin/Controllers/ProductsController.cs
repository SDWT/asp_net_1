using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Interfaces.Services;

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
            return View(_ProductData.GetProducts());
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
        public async Task<IActionResult> Create([Bind("Order,SectionId,BrandId,ImageUrl,Price,Name,Id")] Product product)
        {
            if (ModelState.IsValid)
            {
                await _ProductData.AddProduct(product);
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_ProductData.GetBrands(), "Id", "Name", product.BrandId);
            ViewData["SectionId"] = new SelectList(_ProductData.GetSections(), "Id", "Name", product.SectionId);
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
            ViewData["BrandId"] = new SelectList(_ProductData.GetBrands(), "Id", "Name", product.BrandId);
            ViewData["SectionId"] = new SelectList(_ProductData.GetSections(), "Id", "Name", product.SectionId);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Order,SectionId,BrandId,ImageUrl,Price,Name,Id")] Product product)
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
            ViewData["BrandId"] = new SelectList(_ProductData.GetBrands(), "Id", "Name", product.BrandId);
            ViewData["SectionId"] = new SelectList(_ProductData.GetSections(), "Id", "Name", product.SectionId);
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
            var product = _ProductData.GetProductById(id);
            await _ProductData.RemoveProduct(id, product);
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
