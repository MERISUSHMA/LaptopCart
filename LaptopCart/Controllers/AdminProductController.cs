using LaptopCart.Data;
using LaptopCart.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace LaptopCart.Controllers
{
    public class AdminProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> products = _context.Products.ToList();
            return View(products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {

            {
                //Get the wwwroot path from the environment
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                //clean and generate a unique filename
                string originalFileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName).Replace(" ", "_");
                string extension = Path.GetExtension(product.ImageFile.FileName);
                string uniqueFileName = $"{originalFileName}_{Guid.NewGuid():N}{extension}";

                //ensure the /images folder exista
                string imagesFolder = Path.Combine(wwwRootPath, "images");
                if (!Directory.Exists(imagesFolder))
                {
                    Directory.CreateDirectory(imagesFolder);
                }

                //path to save the image physically
                string filePath = Path.Combine(imagesFolder, uniqueFileName);

                //save file to server
                using (var steam = new FileStream(filePath, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(steam);
                }

                //save relative path(for Razor <img src=....>)
                product.ImagePath = "/images/" + uniqueFileName;

                //[Optional] verify file was saved -useful for debugging
                string confirmPath = Path.Combine(wwwRootPath, product.ImagePath.TrimStart('/'));
                if (!System.IO.File.Exists(confirmPath))
                {
                    throw new FileNotFoundException("Image was not saved correctly", confirmPath);
                }


            }
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);


                await _context.SaveChangesAsync();
                TempData["success"] = "Record Inserted Successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            //If a new image is uploaded, save it
            if(product.ImageFile != null && product.ImageFile.Length > 0)
            { 
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(product.ImageFile.FileName);
            var savePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);

            //Ensure /images folder exists
            var imagesDir = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            if (!Directory.Exists(imagesDir))
            {
                Directory.CreateDirectory(imagesDir);
            }

            //Save the new file
            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await product.ImageFile.CopyToAsync(stream);
            }

            //Save relative path in database
            product.ImagePath = "/images/" + fileName;
        }
           //Update Product in DB
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            TempData["success"] = "Record edited successfully";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                //Optional: Delete image file if needed
                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImagePath.TrimStart('/').Replace("/", "\\"));
                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);
                }
                _context.Products.Remove(product);
                TempData["success"] = "Recoed Deleted successfully";
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
            
    }
}

        