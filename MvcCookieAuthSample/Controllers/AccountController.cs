using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcCookieAuthSample.Models;

namespace MvcCookieAuthSample.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private IIdentityServerInteractionService interaction;

        //private readonly TestUserStore _users;

        //public AccountController(TestUserStore users)
        //{
        //    this._users = users;
        //}

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IIdentityServerInteractionService interaction)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.interaction = interaction;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                ViewData["returnUrl"] = returnUrl;
                var identityUser = new ApplicationUser
                {
                    Email = registerViewModel.Email,
                    UserName = registerViewModel.Email,
                    NormalizedUserName = registerViewModel.Email
                };
                var identityResult = await userManager.CreateAsync(identityUser, registerViewModel.Password);
                if (identityResult.Succeeded)
                {
                    await signInManager.SignInAsync(identityUser, new AuthenticationProperties { IsPersistent = true });
                }
                else
                {
                    AddErrors(identityResult);
                }
            }
            return View();
        }

        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel,string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                ViewData["ReturnUrl"] = returnUrl;
                var user = await userManager.FindByEmailAsync(loginViewModel.Email);
                if (user == null)
                {
                    ModelState.AddModelError(nameof(loginViewModel.Email), "Email not exists");
                }
                if(await userManager.CheckPasswordAsync(user,loginViewModel.Password))
                {
                    AuthenticationProperties props = null;
                    if (loginViewModel.RememberMe)
                    {
                        props = new AuthenticationProperties()
                        {
                            IsPersistent = true,
                            ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(30))
                        };
                    }

                    //await Microsoft.AspNetCore.Http.AuthenticationManagerExtensions.SignInAsync(
                    //    HttpContext,
                    //    user.SubjectId,
                    //    user.Username,
                    //    props);

                    await signInManager.SignInAsync(user, props);

                    if (interaction.IsValidReturnUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    //return RedirectToLoacl(returnUrl);
                    return Redirect("~/");
                }
                else
                {
                    ModelState.AddModelError(nameof(loginViewModel.Password), "password wrong");
                }
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            //await HttpContext.SignOutAsync();
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        private IActionResult RedirectToLoacl(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}