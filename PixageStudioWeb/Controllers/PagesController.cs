using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PixageStudioWeb.Data;
using PixageStudioWeb.Models;

namespace PixageStudioWeb.Controllers
{
    public class PagesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PagesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.Images = _context.ImagePools.ToList();
            ViewBag.Category = _context.Categories.Where(x => x.Name != "HomePage").OrderBy(x=>x.Name).ToList();
            return View();
        }
        public IActionResult GetGenre(int id)
        {
            ViewBag.Category = _context.Categories.Where(x=>x.Name!="HomePage").OrderBy(x=>x.Name).ToList();
           
            ViewBag.Genres = _context.ImagePools.Where(x=>x.CategoryId==id);
            return View();
        }
        
    }
}