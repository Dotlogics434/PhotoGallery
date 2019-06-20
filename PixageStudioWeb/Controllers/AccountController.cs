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
   
        [Authorize]
        public class AccountController : Controller
        {
            private UserManager<ApplicationUser> userManager;
            private SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext _context;

            public AccountController(UserManager<ApplicationUser> userMgr, SignInManager<ApplicationUser> signinMgr, ApplicationDbContext context)
            {
                userManager = userMgr;
                signInManager = signinMgr;
            _context = context;
            }

            public IActionResult Index()
            {
                return View();
            }
            [HttpGet]
            [AllowAnonymous]
            public IActionResult Login(string returnUrl)
            {
                Login login = new Login();
                login.ReturnUrl = returnUrl;
                return View(login);
            }
      
            [HttpPost]
            [AllowAnonymous]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Login(MultiModel multimodel )
        {
            ViewBag.Category = _context.Categories.Where(x=>x.Name!="HomePage").OrderBy(x=>x.Name).ToList();
            if (ModelState.IsValid)
                {
                    ApplicationUser appUser = await userManager.FindByEmailAsync(multimodel.login.Email);
                    if (appUser != null)
                    {
                   
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(appUser, multimodel.login.Password, false, false);
                        if (result.Succeeded)
                    {
                        if(User.IsInRole("Administrator") || User.IsInRole("Editor"))
                        {
                            return RedirectToAction("Dashboard", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                            
                    }
               
                    ModelState.AddModelError(nameof(multimodel.login.Email), "Login Failed: Invalid Email or password");
                }
                return View(multimodel.login);
            }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Login model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }


        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

    }
}