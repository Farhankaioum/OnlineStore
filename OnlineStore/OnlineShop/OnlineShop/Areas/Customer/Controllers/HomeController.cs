using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            this._db = db;
        }

        public IActionResult Index()
        {
            var productData = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag).ToList();
            return View(productData);
        }
        //Product details start here
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productDetails = _db.Products.Include(c => c.ProductTypes).Include(c => c.SpecialTag)
                                                                .FirstOrDefault(c => c.Id == id);
           
            if (productDetails == null)
            {
                return NotFound();
            }
            return View(productDetails);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
