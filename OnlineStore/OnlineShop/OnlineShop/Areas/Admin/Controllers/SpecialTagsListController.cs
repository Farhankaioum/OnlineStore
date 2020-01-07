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
    public class SpecialTagsListController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SpecialTagsListController(ApplicationDbContext db)
        {
            this._db = db;
        }
        //All Tag list retrive and show in view
        [HttpGet]
        public IActionResult Index()
        {
            var data = _db.SpecialTagList.ToList();
            return View(data);
        }

        // Create special tags start
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialTagList specialTagList)
        {
            if (ModelState.IsValid)
            {
                _db.SpecialTagList.Add(specialTagList);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(specialTagList);
        }

        //Special tag list Edit start here
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTag = _db.SpecialTagList.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SpecialTagList specialTagList)
        {
            if (ModelState.IsValid)
            {
                _db.Update(specialTagList);
               await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(specialTagList);
        }

        //See details special tag list
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTag = _db.SpecialTagList.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            return View(specialTag);
        }

        //Delete special tag list
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var specialTag = _db.SpecialTagList.Find(id);
            if (specialTag == null)
            {
                return NotFound();
            }
            _db.SpecialTagList.Remove(specialTag);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
    }
}