using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Agri_Energy_Connect.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agri_Energy_Connect.Data;

namespace Agri_Energy_Connect.Controllers
{

    using Microsoft.EntityFrameworkCore.Metadata.Internal;

    public class ProductsController : Controller
    {
        private readonly AECContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(AECContext context, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }

        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            int farmerId = HttpContext.Session.GetInt32("UserId").Value;
            if (ModelState.IsValid)
            {
                var products = new Product
                {
                    farmerId = farmerId,
                    Name = product.Name,
                    Description = product.Description,
                    ProductionDate = product.ProductionDate,
                    Type = product.Type,
                };
                _context.Products.Add(products);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Products");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.FindAsync(id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ProductionDate,Type")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var farmerId = HttpContext.Session.GetInt32("UserId").Value;
                product.farmerId = farmerId;
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
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
            return RedirectToAction("Index", "Products");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        // POST: MyWorks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var myWork = await _context.Products.FindAsync(id);
            if (myWork != null)
            {
                _context.Products.Remove(myWork);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ViewAllProducts(DateOnly? filterDate, string filterType)
        {

            var products = _context.Products.AsQueryable();

            if (filterDate.HasValue)
            {
                products = products.Where(p => p.ProductionDate == filterDate.Value);
            }

            if (!string.IsNullOrEmpty(filterType))
            {
                products = products.Where(p => p.Type == filterType);
            }

            products = products.OrderByDescending(p => p.ProductionDate);

            var filteredProducts = await products.ToListAsync();
            return View(filteredProducts);
        }


    }
}