using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SpecialTagController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public SpecialTagController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult Index()
        {
            return View(_applicationDbContext.SpecialTags.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialTag specialTag)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.SpecialTags.Add(specialTag);
                await _applicationDbContext.SaveChangesAsync();
                TempData["save"] = "Special Tag has been saved";
                return RedirectToAction(nameof(Index));
            }
            return View(specialTag);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTag = _applicationDbContext.SpecialTags.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SpecialTag specialTag)
        {
            if (ModelState.IsValid)
            {
                _applicationDbContext.SpecialTags.Update(specialTag);
                await _applicationDbContext.SaveChangesAsync();
                TempData["edit"] = "Special Tag has been edited";
                return RedirectToAction(nameof(Index));
            }
            return View(specialTag);
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTag = _applicationDbContext.SpecialTags.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTag = _applicationDbContext.SpecialTags.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var specialTags = _applicationDbContext.SpecialTags.Find(id);
            if (specialTags == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _applicationDbContext.Remove(specialTags);
                await _applicationDbContext.SaveChangesAsync();
                TempData["delete"] = "Special Tag has been deleted";
                return RedirectToAction(nameof(Index));
            }
            return View(specialTags);
        }
    }
}