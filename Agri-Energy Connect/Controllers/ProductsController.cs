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
        public async Task<IActionResult> AddProduct(Product product, IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                ModelState.AddModelError("", "File not selected");
                return RedirectToAction("Index", "Home");
            }

            var permittedExtensions = new[] { ".jpg", ".png", ".gif" };
            var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

            if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
            {
                ModelState.AddModelError("", "Invalid image type.");
                return RedirectToAction("Index", "Home");
            }

            var mimeType = image.ContentType;
            var permittedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif" };
            if (!permittedMimeTypes.Contains(mimeType))
            {
                ModelState.AddModelError("", "Invalid MIME type.");
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                var uploadFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "productImages");
                if (!Directory.Exists(uploadFolderPath))
                {
                    Directory.CreateDirectory(uploadFolderPath);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filePath = Path.Combine(uploadFolderPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    await image.CopyToAsync(stream);
                }

                var products = new Product
                {
                    Name = product.Name,
                    Description = product.Description,
                    ProductionDate = product.ProductionDate,
                    ImageUrl = "/productImages/" + uniqueFileName,
                    Image = ConvertToByteArray(filePath), // Set the image byte array as needed
                    Type = product.Type,
                };
                _context.Products.Add(products);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Products");
        }

        private byte[] ConvertToByteArray(string filePath)
        {
            byte[] fileData;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    fileData = reader.ReadBytes((int)fs.Length);
                }
            }
            return fileData;
        }

        private string GetMimeType(byte[] imageData)
        {
            if (imageData.Length >= 2 && imageData[0] == 0xFF && imageData[1] == 0xD8)
            {
                return "image/jpeg";
            }
            else if (imageData.Length >= 4 && imageData[0] == 0x89 && imageData[1] == 0x50 && imageData[2] == 0x4E && imageData[3] == 0x47)
            {
                return "image/png";
            }
            return "application/octet-stream";
        }

        public IActionResult DisplayImage(int id)
        {
            var image = _context.Products.Find(id);

            if (image == null || image.Image == null)
            {
                return NotFound();
            }
            return File(image.Image, GetMimeType(image.Image));
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
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
    public async Task<IActionResult> Edit(int id, [Bind("ProductID,ProductName,Description,ProductionDate,ImageUrl,Image,Type")] Product product, IFormFile image)
    {
        if (id != product.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                if (image != null && image.Length > 0)
                {
                    var permittedExtensions = new[] { ".jpg", ".png", ".gif" };
                    var extension = Path.GetExtension(image.FileName).ToLowerInvariant();

                    if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("image", "Invalid image type.");
                        return View(product);
                    }

                    var uploadFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "productImages");
                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                    var filePath = Path.Combine(uploadFolderPath, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        await image.CopyToAsync(stream);
                    }
                    product.ImageUrl = uniqueFileName;
                    product.Image = ConvertToByteArray(filePath);
                }
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
        return View(product);
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
    }
}