using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IHostingEnvironment _hostingEnvironment;


        public ProductController(ApplicationDbContext applicationDbContext, IHostingEnvironment hostingEnvironment)
        {
            _applicationDbContext = applicationDbContext;
            _hostingEnvironment = hostingEnvironment;
        }
        public IActionResult Index()
        {
             return View(_applicationDbContext.Products.Include(c=>c.ProductTypes).Include(s=>s.SpecialTag).ToList());
        }
        [HttpPost]
        public IActionResult Index(decimal? lowAmount,decimal? largeAmount)
        {
            var products = _applicationDbContext.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).Where(c=>c.Price>=lowAmount&&c.Price<=largeAmount);
            if(lowAmount == null || largeAmount == null)
            {
                var product = _applicationDbContext.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList();
                return View(product);
            }
            return View(products);
        }
        public ActionResult Create()
        {
            ViewData["productTypesId"] = new SelectList(_applicationDbContext.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["specialTagId"] = new SelectList(_applicationDbContext.SpecialTags.ToList(), "Id", "SpecialTags");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product,IFormFile image)
        {
            var searchProduct = _applicationDbContext.Products.Where(c=>c.Name==product.Name);
            if (searchProduct.Count() != 0)
            {
                ViewBag.message = "This Product is already exist!";
                ViewData["productTypesId"] = new SelectList(_applicationDbContext.ProductTypes.ToList(), "Id", "ProductType");
                ViewData["specialTagId"] = new SelectList(_applicationDbContext.SpecialTags.ToList(), "Id", "SpecialTags");
                return View(product);
            }
            if (ModelState.IsValid)
            {
                if(image!=null)
                {
                    var name = Path.Combine(_hostingEnvironment.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "images/" + image.FileName;
                }
                if(image==null)
                {
                    product.Image = "images/no-image.png";
                }
                _applicationDbContext.Products.Add(product);
                await _applicationDbContext.SaveChangesAsync();
                TempData["save"] = "Product has been saved";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        public ActionResult Edit(int? id)
        {
            ViewData["productTypesId"] = new SelectList(_applicationDbContext.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["specialTagId"] = new SelectList(_applicationDbContext.SpecialTags.ToList(), "Id", "SpecialTags");
            if (id == null)
            {
                return NotFound();
            }
            var product = _applicationDbContext.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product, IFormFile image)
        {
            if (image != null)
            {
                var name = Path.Combine(_hostingEnvironment.WebRootPath + "/images", Path.GetFileName(image.FileName));
                await image.CopyToAsync(new FileStream(name, FileMode.Create));
                product.Image = "images/" + image.FileName;
            }
            if (image == null)
            {
                //var pro = _applicationDbContext.Products.Where(c=>c.Id == product.Id).LastOrDefault();
                //product.Image = pro.Image;
                product.Image = "images/no-image.png";
            }
            if (ModelState.IsValid)
            {
                _applicationDbContext.Products.Update(product);
                await _applicationDbContext.SaveChangesAsync();
                TempData["edit"] = "Product has been edited";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _applicationDbContext.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = _applicationDbContext.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).FirstOrDefault(c => c.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = _applicationDbContext.Products.Find(id);
            if (products == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _applicationDbContext.Remove(products);
                await _applicationDbContext.SaveChangesAsync();
                TempData["delete"] = "Product has been deleted";
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }
    }
}