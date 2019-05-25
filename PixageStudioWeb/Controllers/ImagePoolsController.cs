using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PixageStudioWeb.Data;
using PixageStudioWeb.Models;

namespace PixageStudioWeb.Controllers
{
    public class ImagePoolsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ImagePoolsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ImagePools
        public async Task<IActionResult> Index()
        {
            return View(await _context.ImagePool.ToListAsync());
        }



        // GET: ImagePools/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagePool = await _context.ImagePool
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imagePool == null)
            {
                return NotFound();
            }

            return View(imagePool);
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            ImagePool Images = new ImagePool();
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(Directory.GetCurrentDirectory(),
                                    "wwwroot/Uploads",
                                    file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                Images.AltName = file.FileName;
                Images.ImagePath = path;
                _context.Add(Images);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Download(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot/Uploads", filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
        {
            throw new NotImplementedException();
        }

        // GET: ImagePools/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ImagePools/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AltName,ImgData,Genre")] ImagePool imagePool)
        {
            if (ModelState.IsValid)
            {
                _context.Add(imagePool);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(imagePool);
        }

        // GET: ImagePools/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagePool = await _context.ImagePool.FindAsync(id);
            if (imagePool == null)
            {
                return NotFound();
            }
            return View(imagePool);
        }

        // POST: ImagePools/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AltName,ImgData,Genre")] ImagePool imagePool)
        {
            if (id != imagePool.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imagePool);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImagePoolExists(imagePool.Id))
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
            return View(imagePool);
        }

        // GET: ImagePools/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagePool = await _context.ImagePool
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imagePool == null)
            {
                return NotFound();
            }

            return View(imagePool);
        }

        // POST: ImagePools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imagePool = await _context.ImagePool.FindAsync(id);
            _context.ImagePool.Remove(imagePool);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImagePoolExists(int id)
        {
            return _context.ImagePool.Any(e => e.Id == id);
        }
    }
}
