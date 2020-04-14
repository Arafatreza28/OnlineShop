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
            if (ModelState.IsValid)
            {
                if(image!=null)
                {
                    var name = Path.Combine(_hostingEnvironment.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    product.Image = "images/" + image.FileName;
                }
                _applicationDbContext.Products.Add(product);
                await _applicationDbContext.SaveChangesAsync();
                TempData["save"] = "Product has been saved";
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var specialTag = _applicationDbContext.SpecialTags.Find(id);
        //    if (specialTag == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(specialTag);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(SpecialTag specialTag)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _applicationDbContext.SpecialTags.Update(specialTag);
        //        await _applicationDbContext.SaveChangesAsync();
        //        TempData["edit"] = "Special Tag has been edited";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(specialTag);
        //}
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var specialTag = _applicationDbContext.SpecialTags.Find(id);
        //    if (specialTag == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(specialTag);
        //}
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var specialTag = _applicationDbContext.SpecialTags.Find(id);
        //    if (specialTag == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(specialTag);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Delete(int? id, SpecialTag specialTag)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    if (id != specialTag.Id)
        //    {
        //        return NotFound();
        //    }

        //    var specialTags = _applicationDbContext.SpecialTags.Find(id);
        //    if (specialTags == null)
        //    {
        //        return NotFound();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        _applicationDbContext.Remove(specialTags);
        //        await _applicationDbContext.SaveChangesAsync();
        //        TempData["delete"] = "Special Tag has been deleted";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(specialTags);
        //}
    }
}