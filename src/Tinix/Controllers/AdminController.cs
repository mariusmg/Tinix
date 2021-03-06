using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tinix.Context;
using Tinix.Models;
using Microsoft.Extensions.Logging;

namespace Tinix.Controllers
{
    public class AdminController : Controller
    {
        private IAdminService adminService;
        private IBlog blogService;
        private ILogger<AdminController> log;

        public AdminController(IAdminService admin, IBlog blog, ILogger<AdminController> log)
        {
            this.adminService = admin;
            this.blogService = blog;
            this.log = log;
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> LogOff()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            if (adminService.ValidateCredentials(model.UserName, model.Password))
            {
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, model.UserName));

                ClaimsPrincipal principle = new ClaimsPrincipal(identity);
                AuthenticationProperties properties = new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);

                return RedirectToAction("NewPost");
            }

            return View(model);

        }



        [Authorize]
        public async Task<IActionResult> Posts()
        {
            AdminViewModel vm = new AdminViewModel
            {
                Items = await blogService.GetPosts()
            };

            return View(vm);
        }



        [Authorize, HttpPost]
        public ActionResult Delete(string postId)
        {
            try
            {
                blogService.Delete(postId);
            }
            catch (Exception ex)
            {
                log.LogError(ex, ex.Message);
            }

            return RedirectToAction("Posts", "Admin");
        }



        [Authorize]
        public IActionResult NewPost()
        {
            NewPostViewModel viewModel = new NewPostViewModel();
            return View(viewModel);
        }


        [Authorize, HttpPost]
        public async Task<IActionResult> NewPost(NewPostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("NewPost", model);
            }

            try
            {
                await blogService.SavePost(model.Content, model.Title);
            }
            catch(Exception ex)
            {
                log.LogError(ex, ex.Message);
            }

            return Redirect("NewPost");

        }


    }
}