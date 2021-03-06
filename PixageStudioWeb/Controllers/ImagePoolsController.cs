﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PixageStudioWeb.Data;
using PixageStudioWeb.Models;

namespace PixageStudioWeb.Controllers
{
    //[Authorize(Roles="Administrator, Editor")]
    public class ImagePoolsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _he;
        public ImagePoolsController(ApplicationDbContext context, IHostingEnvironment he)
        {
            _he = he;
            _context = context;
        }

        // GET: ImagePools
        public async Task<IActionResult> Index()
        {
            ViewBag.ImageList = await _context.ImagePools.OrderBy(x=>x.CategoryId).OrderBy(x=>x.AltName).ToListAsync();
            return View();
        }



        // GET: ImagePools/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imagePool = await _context.ImagePools
                .FirstOrDefaultAsync(m => m.Id == id);
            if (imagePool == null)
            {
                return NotFound();
            }

            return View(imagePool);
        }
        public ActionResult SetAboutPicture(int id, ImagePool imagePool)
        {
            if (id != imagePool.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(imagePool.Status==false)
                    {
                        imagePool.Status = true;
                      
                        
                    }
                    else
                    {
                        imagePool.Status = false;
                    }

                    _context.Update(imagePool);
                   
                     _context.SaveChanges();
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

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file, int genre)
        {
            ImagePool Images = new ImagePool();
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var path = Path.Combine(Directory.GetCurrentDirectory(),
                                    "wwwroot/Images",
                                    file.FileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                Images.AltName = file.FileName;
                Images.ImagePath = path;
                Images.CategoryId = genre;
                _context.Add(Images);
                _context.SaveChanges();
            }

            return RedirectToAction("Upload", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> UploadMultiple(ICollection<IFormFile> files, int genre)
        {
            
            
            foreach (var file in files)

            {
                ImagePool Images = new ImagePool();

                var path = Path.Combine(Directory.GetCurrentDirectory(),
                                    "wwwroot/Images",
                                    file.FileName);
                if (file.Length > 0)
                {
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        Images.AltName = file.FileName;
                        Images.ImagePath = path;
                        Images.CategoryId = genre;
                        _context.Add(Images);
                        _context.SaveChanges();
                    }
                }
            }
           
            return RedirectToAction("Upload", "Admin");
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

            var imagePool = await _context.ImagePools.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,AltName,ImgData,Genre,Status")] ImagePool imagePool)
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

            var imagePool = await _context.ImagePools
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
            ViewBag.deleteSuccess = "false";

            var imagePool = await _context.ImagePools
               .FirstOrDefaultAsync(m => m.Id == id);
            var fullPath = imagePool.ImagePath.ToString();

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
                ViewBag.deleteSuccess = "true";
            }
            var imagePools = await _context.ImagePools.FindAsync(id);
            _context.ImagePools.Remove(imagePools);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        

        private bool ImagePoolExists(int id)
        {
            return _context.ImagePools.Any(e => e.Id == id);
        }
    }
}
