using LaptopCart.Data;
using LaptopCart.Models;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

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
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (product.ImageFile != null && product.ImageFile.Length > 0)
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
                if(!System.IO.File.Exists(confirmPath))
                {
                    throw new FileNotFoundException("Image was not saved correctly", confirmPath);
                }

                
            }
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }
    }
}

        