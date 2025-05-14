using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Agri_Energy_Connect.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agri_Energy_Connect.Data;
using System;

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
            int farmerId = HttpContext.Session.GetInt32("UserId").Value;
            var products = await _context.Products.Where(p => p.UserId == farmerId).ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> ViewFarmers()
        {
            var farmers = await _context.Users
                .Where(u => u.role == "User")
                .ToListAsync();

            if (farmers == null || farmers.Count == 0)
            {
                return NotFound("No farmers found.");
            }
            return View(farmers);
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
                    UserId = farmerId,
                    Name = product.Name,
                    Description = product.Description,
                    ProductionDate = product.ProductionDate,
                    Category = product.Category,
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ProductionDate,Category")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var farmerId = HttpContext.Session.GetInt32("UserId").Value;
                product.UserId = farmerId;
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
        public async Task<IActionResult> ViewProducts(int farmerId, DateTime? filterDate, string filterType)
        {
            // 1. Get the farmer's products from the database.
            var farmerProducts = await _context.Products
                .Where(p => p.UserId == farmerId)
                .ToListAsync();

            // 2. Check if farmer exists
            var farmer = await _context.Users.FirstOrDefaultAsync(u => u.UserId == farmerId);
            if (farmer == null)
            {
                return NotFound($"Farmer with ID {farmerId} not found.");
            }

            // 3. Apply filters
            if (!string.IsNullOrEmpty(filterType))
            {
                farmerProducts = farmerProducts.Where(p => p.Category == filterType).ToList();
            }

            if (filterDate.HasValue)
            {
                farmerProducts = farmerProducts.Where(p => p.ProductionDate == DateOnly.FromDateTime(filterDate.Value)).ToList();
            }

            if (farmerProducts == null || farmerProducts.Count == 0)
            {
                if (string.IsNullOrEmpty(filterType) && !filterDate.HasValue)
                    return NotFound($"No products found for farmer with ID {farmerId}.");
                else
                    return NotFound($"No products found for farmer with ID {farmerId} matching the criteria.");
            }

            // 4. Pass the data to the view.
            ViewData["FarmerName"] = farmer.Name;
            ViewData["FarmerId"] = farmerId;
            ViewData["FilterType"] = filterType; 
            ViewData["FilterDate"] = filterDate;
            return View(farmerProducts);
        }
    }
}
