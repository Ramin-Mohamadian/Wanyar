using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wanyar.Core.DTOs;
using Wanyar.Core.Services.Interfaces;

namespace Wanyar_Web.Areas.UserPanel.Controllers
{
    [Authorize]
    [Area("UserPanel")]
    public class HomeController : Controller
    {
        private IUserService _userService;
        public HomeController(IUserService userService)
        {
            _userService = userService;
        }


        public IActionResult Index()
        {
            return View(_userService.GetUserInformation(User.Identity.Name));
        }


        #region EditeProfile
        [Route("UserPanel/EditeProfile")]
        public IActionResult EditeUserProfile()
        {
            return View(_userService.GetDataUserProfile(User.Identity.Name));
        }


        [HttpPost]
        [Route("UserPanel/EditeProfile")]
        public IActionResult EditeUserProfile(EditeUserProfileViewModel profile)
        {
            if (!ModelState.IsValid)
            {
                return View(profile);
            }

            _userService.EditeUserProfile(User.Identity.Name, profile);
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            ViewBag.Profile = true;
            return Redirect("/Login?EditProfile=true");
        }

        #endregion

        #region ChangePassword
        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Route("UserPanel/ChangePassword")]
        public IActionResult ChangePassword(ChagePasswordViewModel change)
        {
            if (!ModelState.IsValid)
            {               
                return View(change);
            }
            var currntUser=User.Identity.Name;
            if(!_userService.CompareOldPassword(currntUser,change.OldPassword))
            {
                ModelState.AddModelError("OldPassword", "کلمه عبور قبلی صحیح نمیباشد");
                return View(change);
            }


            _userService.ChangePassword(currntUser, change.Password);
            ViewBag.IsSuccess=true;
            return View();
        }
        #endregion

    }
}
