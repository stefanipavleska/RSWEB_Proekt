using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCProekt.Data;
using MVCProekt.Models;
using MVCProekt.Areas.Identity.Data;


namespace MVCProekt.Controllers
{
    public class UserProductsController : Controller
    {
        private readonly MVCProektContext _context;
        private readonly UserManager<MVCProektUser> _userManager;

        public UserProductsController(MVCProektContext context, UserManager<MVCProektUser> usermanager)
        {
            _context = context;
            _userManager = usermanager;
        }

        private Task<MVCProektUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddToMyProducts(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var MVCUserProductContext = _context.UserProduct.Where(m => m.ProductId == id);
            var user = await GetCurrentUserAsync();
            if (ModelState.IsValid)
            {
                UserProduct userProduct = new UserProduct();
                userProduct.ProductId = (int)id;
                userProduct.Username = user.UserName;
                _context.UserProduct.Add(userProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MoiProdukti));
            }
            if (MVCUserProductContext != null)
            {
                return View(await MVCUserProductContext.ToListAsync());
            }
            else
            {
                return Problem("Entity set 'MVCProductContext.UserProduct' is null!");
            }
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> MoiProdukti()
        {
            var user = await GetCurrentUserAsync();
            var MVCUserProductContext = _context.UserProduct.AsQueryable().Include(m => m.Product).ThenInclude(m => m.Cook).Where(m => m.Username == user.UserName);
            var MyProductsList = _context.Product.AsQueryable();
            MyProductsList = MVCUserProductContext.Select(m => m.Product);
            if (MVCUserProductContext != null)
            {
                return View("~/Views/UserProducts/MyProducts.cshtml", await MyProductsList.ToListAsync());
            }
            else
            {
                return Problem("Entity set 'MVCProektContext.UserProduct' is null!");
            }
        }



        // GET: UserProducts
        public async Task<IActionResult> Index()
        {
            var mVCProektContext = _context.UserProduct.Include(u => u.Product);
            return View(await mVCProektContext.ToListAsync());
        }

        // GET: UserProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserProduct == null)
            {
                return NotFound();
            }

            var userProduct = await _context.UserProduct
                .Include(u => u.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userProduct == null)
            {
                return NotFound();
            }

            return View(userProduct);
        }

        // GET: UserProducts/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "ProductImage");
            return View();
        }

        // POST: UserProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,ProductId")] UserProduct userProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "ProductImage", userProduct.ProductId);
            return View(userProduct);
        }

        // GET: UserProducts/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserProduct == null)
            {
                return NotFound();
            }

            var userProduct = await _context.UserProduct.FindAsync(id);
            if (userProduct == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "ProductImage", userProduct.ProductId);
            return View(userProduct);
        }

        // POST: UserProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,ProductId")] UserProduct userProduct)
        {
            if (id != userProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserProductExists(userProduct.Id))
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
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "ProductImage", userProduct.ProductId);
            return View(userProduct);
        }

        // GET: UserProducts/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserProduct == null)
            {
                return NotFound();
            }

            var userProduct = await _context.UserProduct
                .Include(u => u.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userProduct == null)
            {
                return NotFound();
            }

            return View(userProduct);
        }

        // POST: UserProducts/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserProduct == null)
            {
                return Problem("Entity set 'MVCProektContext.UserProduct'  is null.");
            }
            var userProduct = await _context.UserProduct.FindAsync(id);
            if (userProduct != null)
            {
                _context.UserProduct.Remove(userProduct);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserProductExists(int id)
        {
          return (_context.UserProduct?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
