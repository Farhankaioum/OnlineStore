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
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _he;

        public ProductController(ApplicationDbContext db, IHostingEnvironment he)
        {
            this._db = db;
            this._he = he;
        }

        //Product retrive 
        public IActionResult Index()
        {
            var products = _db.Products.Include( c => c.ProductTypes).Include( c => c.SpecialTag).ToList();
            return View(products);
        }

        //Product adding start here
        [HttpGet]
        public ActionResult Create()
        {
            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTagList.ToList(), "Id", "TagName");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Products products, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name , FileMode.Create));
                    products.Image = "Images/" + image.FileName;
                }
                else
                {
                    products.Image = "Images/no-image.jpg";
                }
                _db.Products.Add(products);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        //Edit product data start
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productData = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag)
                                                            .FirstOrDefault(c => c.Id == id);

            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTagList.ToList(), "Id", "TagName");
            if (productData == null)
            {
                return NotFound();
            }
            return View(productData);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Products products, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    products.Image = "Images/" + image.FileName;
                }
                else
                {
                    products.Image = "Images/no-image.jpg";
                }

                _db.Update(products);
               await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(products);
        }

        //Product see details page start here
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productDetails = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag)
                                                            .FirstOrDefault(c => c.Id == id);

            ViewData["productTypeId"] = new SelectList(_db.ProductTypes.ToList(), "Id", "ProductType");
            ViewData["TagId"] = new SelectList(_db.SpecialTagList.ToList(), "Id", "TagName");
            if (productDetails == null)
            {
                return NotFound();
            }
            return View(productDetails);
        }

        // product data delete page start here
        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productDetails = _db.Products.FirstOrDefault( c => c.Id == id);
            if (productDetails == null)
            {
                return NotFound();
            }
            _db.Remove(productDetails);
           await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}