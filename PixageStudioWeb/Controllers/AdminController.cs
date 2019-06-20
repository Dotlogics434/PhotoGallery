using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PixageStudioWeb.Data;
using PixageStudioWeb.Models;

namespace PixageStudioWeb.Controllers

{
    //[Authorize(Roles="Administrator, Editor")]
    public class AdminController : Controller
    {
        private  UserManager<ApplicationUser> _userManager;
        private IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<ApplicationUser> usrMgr, IPasswordHasher<ApplicationUser> passwordHasher, ApplicationDbContext context )
        {
            _userManager = usrMgr;
            _passwordHasher = passwordHasher;
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Users = _userManager.Users.ToList();
            return View();
        }
        public IActionResult UserRole()
        {
            return View();
        }
        public ViewResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser appUser = new ApplicationUser
                {
                    UserName = user.Name,
                    Email = user.Email,
                    
                };
                try
                {
                    IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                    {
                        //foreach (IdentityError error in result.Errors)
                        //    ModelState.AddModelError("", error.Description);
                        TempData["Message"] = "There was an error creating the user.";
                        return RedirectToAction("Index");

                    }
                }
                catch
                {
                    TempData["Message"] = "An Error Occured";
                }
            }
            return View(user);
        }
        public async Task<IActionResult> Update(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, string email, string password)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(email))
                    user.Email = email;
                else
                    ModelState.AddModelError("", "Email cannot be empty");

                if (!string.IsNullOrEmpty(password))
                    user.PasswordHash = _passwordHasher.HashPassword(user, password);
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("Index", _userManager.Users);
        }
       
        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult Upload()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }
    }
}
