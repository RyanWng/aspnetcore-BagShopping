using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QualityBags.Data;
using QualityBags.Models;

namespace QualityBags.Controllers
{
    public class CustomerBagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerBagsController(ApplicationDbContext context)
        {
            _context = context;    
        }

        // GET: CustomerBags
        public IActionResult Index(string catFilter = null)
        {
            /*View by category*/
            //ViewData["CatFilter"] = catFilter;  //Store the current selection of category
            //var Bags = from bags in _context.Bags
            //           select bags;//_context.Bags;  //Retrieve all bags 
            //if (!String.IsNullOrEmpty(catFilter))   //If filter exists, retrieve those bags
            //{
            //    Bags = GetBagsByCat(Bags, catFilter);
            //}


            //var applicationDbContext = _context.Bags.Include(b => b.Category).Include(b => b.Supplier);
            return View("Error");
        }



        // GET: CustomerBags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var bag = await _context.Bags.SingleOrDefaultAsync(m => m.BagID == id);
            if (bag == null)
            {
                return View("Error");
            }

            return View("Error");
        }

        // GET: CustomerBags/Create
        //public IActionResult Create()
        //{
        //    ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID");
        //    ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Email");
        //    return View();
        //}

        // POST: CustomerBags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("BagID,BagName,CategoryID,Description,ImagePath,Price,SupplierID")] Bag bag)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(bag);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", bag.CategoryID);
        //    ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Email", bag.SupplierID);
        //    return View(bag);
        //}

        //// GET: CustomerBags/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return View("Error");
        //    }

        //    var bag = await _context.Bags.SingleOrDefaultAsync(m => m.BagID == id);
        //    if (bag == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", bag.CategoryID);
        //    ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Email", bag.SupplierID);
        //    return View(bag);
        //}

        // POST: CustomerBags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("BagID,BagName,CategoryID,Description,ImagePath,Price,SupplierID")] Bag bag)
        //{
        //    if (id != bag.BagID)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(bag);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BagExists(bag.BagID))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction("Index");
        //    }
        //    ViewData["CategoryID"] = new SelectList(_context.Categories, "CategoryID", "CategoryID", bag.CategoryID);
        //    ViewData["SupplierID"] = new SelectList(_context.Suppliers, "SupplierID", "Email", bag.SupplierID);
        //    return View(bag);
        //}

        //// GET: CustomerBags/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var bag = await _context.Bags.SingleOrDefaultAsync(m => m.BagID == id);
        //    if (bag == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(bag);
        //}

        //// POST: CustomerBags/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var bag = await _context.Bags.SingleOrDefaultAsync(m => m.BagID == id);
        //    _context.Bags.Remove(bag);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

        //private bool BagExists(int id)
        //{
        //    return _context.Bags.Any(e => e.BagID == id);
        //}
    }
}
