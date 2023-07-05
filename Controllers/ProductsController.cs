using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using MVCProekt.Data;
using MVCProekt.Interfaces;
using MVCProekt.Models;
using MVCProekt.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Hosting;

namespace MVCProekt.Controllers
{
    public class ProductsController : Controller
    {
        private readonly MVCProektContext _context;
        private readonly IBufferedFileUploadService _bufferedFileUploadService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(MVCProektContext context, IBufferedFileUploadService bufferedFileUploadService, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _bufferedFileUploadService = bufferedFileUploadService;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Products

        /*
        public async Task<IActionResult> Index()
        {
            var mVCProektContext = _context.Product.Include(p => p.Cook);
            return View(await mVCProektContext.ToListAsync());
        }
        */

        public async Task<IActionResult> Index(string searchString, int? id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'MvcProektContext.Movie'  is null.");
            }

            var products = from b in _context.Product
                        select b;
            products = products.Include(p => p.Orders);

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.ProductName!.Contains(searchString));
            }

            //za koga kje se klikne na avtor da se pokazat negovite knigi

            if (id < 1 || id == null)
            {
                products = products.Include(m => m.Cook);
            }
            else
            {
                products = products.Include(m => m.Cook).Where(b => b.CookId == id);
            }
            return View(await products.ToListAsync());
        }

        public async Task<IActionResult> SearchByCategory(int? id)
        {
            IQueryable<ProductCategory> productcategories = _context.ProductCategory.AsQueryable();
            IQueryable<Product> b = _context.Product.AsQueryable();
            if (id == null || id < 1)
            {
                productcategories = productcategories.Include(m => m.Product).ThenInclude(m => m.Cook);
            }
            else
            {
                productcategories = productcategories.Include(m => m.Product).ThenInclude(m => m.Cook).Include(m => m.Category).Where(m => m.CategoryId == id);
            }
            b = productcategories.Select(p => p.Product);
            return View("~/Views/Products/Index.cshtml", await b.ToListAsync());
        }



        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Cook)
                .Include(b => b.Orders)
                .Include(m => m.Categories).ThenInclude(m => m.Category)
                .Include(m => m.UserProducts)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        // GET: Products/Create

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CookId"] = new SelectList(_context.Set<Cook>(), "Id", "FullName");
            ViewData["Categories"] = new MultiSelectList(_context.Set<Category>(), "Id", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductName,Price,ProductImage,CookId")] Product product, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string slika_pateka = await _bufferedFileUploadService.UploadFile(file, _webHostEnvironment);
                    if (slika_pateka != "none")
                    {
                        ViewBag.Message = "File Upload Successful!";
                    }
                    else
                    {
                        ViewBag.Message = "File Upload Failed!";
                    }
                    product.ProductImage = slika_pateka;
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CookId"] = new SelectList(_context.Set<Cook>(), "Id", "FullName", product.CookId);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = _context.Product.Where(m => m.Id == id).Include(m => m.Categories).First();
            /*var product = await _context.Product.FindAsync(id);*/

            if (product == null)
            {
                return NotFound();
            }

            var categories = _context.Category.AsEnumerable();
            categories = categories.OrderBy(s => s.CategoryName);
            ProductCategoriesEditViewModel viewmodel = new ProductCategoriesEditViewModel
            {
                Product = product,
                CategoryList = new MultiSelectList(categories, "Id", "CategoryName"),
                SelectedCategories = product.Categories.Select(sa => sa.CategoryId)
            };

            ViewData["CookId"] = new SelectList(_context.Set<Cook>(), "Id", "FullName", product.CookId);
            return View(viewmodel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductCategoriesEditViewModel viewmodel, IFormFile? file)
        {
            if (id != viewmodel.Product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewmodel.Product);
                    await _context.SaveChangesAsync();
                    IEnumerable<int> newCategoryList = viewmodel.SelectedCategories;
                    IEnumerable<int> prevCategoryList = _context.ProductCategory.Where(s => s.ProductId == id).Select(s => s.CategoryId);
                    IQueryable<ProductCategory> toBeRemoved = _context.ProductCategory.Where(s => s.ProductId == id);
                    if (newCategoryList != null)
                    {
                        toBeRemoved = toBeRemoved.Where(s => !newCategoryList.Contains(s.CategoryId));
                        foreach (int categoryId in newCategoryList)
                        {
                            if (!prevCategoryList.Any(s => s == categoryId))
                            {
                                _context.ProductCategory.Add(new ProductCategory { CategoryId = categoryId, ProductId = id });
                            }
                        }
                    }
                    _context.ProductCategory.RemoveRange(toBeRemoved);
                    if (file != null)
                    {
                        string slika_pateka = await _bufferedFileUploadService.UploadFile(file, _webHostEnvironment);
                        if (slika_pateka != "none")
                        {
                            ViewBag.Message = "File Upload Successful!";
                        }
                        else
                        {
                            ViewBag.Message = "File Upload Failed!";
                        }
                        viewmodel.Product.ProductImage = slika_pateka;
                        _context.Update(viewmodel.Product);
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(viewmodel.Product.Id))
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
            ViewData["CookId"] = new SelectList(_context.Set<Cook>(), "Id", "FullName", viewmodel.Product.CookId);
            return View(viewmodel);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var book = await _context.Product
                .Include(p => p.Cook)
                .Include(b => b.Orders)
                .Include(m => m.Categories).ThenInclude(m => m.Category)
                .Include(m => m.UserProducts)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Products/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'MVCProektContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

