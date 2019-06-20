using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PixageStudioWeb.Data;

namespace PixageStudioWeb.Controllers
{
    public class ExampleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExampleController(ApplicationDbContext context)
        {
            _context = context;

        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Images()
        {
            var img = _context.ImagePools.ToList();
            ViewBag.ImagesList = img.ToList();
            return View();
        }
        public IActionResult LightBox()
        {
            var img = _context.ImagePools.ToList();
            ViewBag.Images = img.ToList();
            return View();
        }
       
    }
}