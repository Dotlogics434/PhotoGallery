using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PixageStudioWeb.Data;
using PixageStudioWeb.Models;

namespace PixageStudioWeb.Controllers
{
    
    public class HomeController : Controller
    {
        ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        
     
        public IActionResult Index()
        {
            var cat = _context.Categories.Where(x=>x.Name!="HomePage").OrderBy(x=>x.Name).ToList();
            var Img = from images in _context.ImagePools
                      join category in _context.Categories on images.CategoryId equals category.Id
                      where (category.Name.Equals("HomePage"))
                      select images;
            ViewBag.Category = cat.ToList();
            ViewBag.Carousel = Img.ToList();
            return View();
        }
        public IActionResult GetGenres(int id)
        {
            ViewBag.Category = _context.Categories.Where(x=>x.Name!="HomePage").OrderBy(x=>x.Name).ToList();
            ViewBag.Images = _context.ImagePools.Where(x=>x.CategoryId == id).ToList();
            return PartialView("~/Views/Home/_PortfolioPartial.cshtml");
        }
        
        public IActionResult About()
        {
           
            ViewBag.Category = _context.Categories.Where(x=>x.Name!="HomePage").OrderBy(x=>x.Name).ToList();
            ViewBag.AboutImage = from image in _context.ImagePools 
                                 join  category in _context.Categories on image.CategoryId equals category.Id
                                 where (category.Name.Equals("AboutUs") && image.Status ==true)
                                 select image;
            return View();
        }

        public IActionResult Contact(ContactViewModel vm)
        {
           
            ViewBag.Category = _context.Categories.Where(x=>x.Name!="HomePage").OrderBy(x=>x.Name).ToList();
            if (ModelState.IsValid)
            {
                try
                {
                    MailMessage msz = new MailMessage();
                    msz.From = new MailAddress(vm.Email);//Email which you are getting 
                                                         //from contact us page 
                    msz.To.Add("prabhakaraninbox@gmail.com");//Where mail will be sent 
                    msz.Subject = vm.Subject;
                    msz.Body = vm.Message;
                    SmtpClient smtp = new SmtpClient();

                    smtp.Host = "smtp.gmail.com";

                    smtp.Port = 587;

                    smtp.Credentials = new System.Net.NetworkCredential
                    ("prabharkaraninbox@gmail.com", "password");

                    smtp.EnableSsl = true;

                    smtp.Send(msz);

                    ModelState.Clear();
                    ViewBag.Message = "Thank you for Contacting us ";
                }
                catch (Exception ex)
                {
                    ModelState.Clear();
                    ViewBag.Message = $" Sorry we are facing Problem here {ex.Message}";
                }
            }

            return View();
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
