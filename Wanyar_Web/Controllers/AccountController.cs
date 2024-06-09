using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Wanyar.Core.Convertors;
using Wanyar.Core.DTOs;
using Wanyar.Core.Generator;
using Wanyar.Core.Security;
using Wanyar.Core.Sender;
using Wanyar.Core.Services.Interfaces;
using Wanyar.DataLayer.Entities.Users;

namespace Wanyar_Web.Controllers
{
    public class AccountController : Controller
    {
        private IUserService _userService;
        private IViewRenderService _viewRender;
        public AccountController(IUserService userService, IViewRenderService viewRender)
        {
            _userService = userService;
            _viewRender=viewRender;

        }

        #region Register
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }



        [HttpPost]
        [Route("Register")]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            if (_userService.IsExistUser(register.UserName))
            {
                ModelState.AddModelError("UserName", "نام کاربری قبلا وارد شده است");
                return View(register);
            }

            if (_userService.IsExistEmail(FixedText.FixedEmail(register.Email)))
            {
                ModelState.AddModelError("Email", " ایمیل قبلا وارد شده است");
                return View(register);
            }

            User user = new User()
            {
                UserName = register.UserName,
                Email=FixedText.FixedEmail(register.Email),
                Password=PasswordHelper.EncodePasswordMd5(register.Password),
                ActiveCode=NameGenerator.GenerateUniqeCode(),
                IsActive=false,
                UserAvatar="Defult.jpg",
                RegisterDate=DateTime.Now,
            };
            _userService.AddUser(user);


            #region Send Active Email
            var body = _viewRender.RenderToStringAsync("ActiveEmail", user);
            SendEmail.Send(user.Email, "فعالسازی", body);
            #endregion


            return View("SuccessRegister", user);
        }
        #endregion

        #region Login
        [Route("Login")]
        public IActionResult Login(bool EditProfile=false)
        {
            ViewBag.EditProfile = EditProfile;
            return View();
        }


        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }


            var user = _userService.LoginUser(login);
            if (user==null)
            {
                ModelState.AddModelError("Email", "کاربری با این مشخصات یافت نشد.");

            }
            else
            {
                if (user.IsActive==true)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName)
                     };

                    var Identity=new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal=new ClaimsPrincipal(Identity);
                    var propertis = new AuthenticationProperties
                    {
                        IsPersistent=login.RememberMe
                    };

                    HttpContext.SignInAsync(principal, propertis);
                    ViewBag.IsActive = user.IsActive;
                    return View("Login");
                }
                else
                {
                    ModelState.AddModelError("Email", "حساب کاربری شما فعال نمی باشد لطفا به ایمیل خود مراجعه کنید");
                }
            }


            return View();
        }
        #endregion

        #region Logout
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }

        #endregion

        #region Active Account
        public IActionResult ActiveAccount(string id)
        {

            ViewBag.ActiveAccount = _userService.ActiveAccount(id);
            return View();
        }
        #endregion

        #region ForgotPassword
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword()
        {
          return View(); 
        }


        [Route("ForgotPassword")]
        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel forgotPassword)
        {
            if (!ModelState.IsValid)            {
                
                return View(forgotPassword);
            }

            User user = _userService.GetUserByEmail(forgotPassword.Email);
            if(user == null)
            {
				ModelState.AddModelError("Email", "ایمیل وارد شده وجود ندارد");
                return View(forgotPassword);
			}
            var bodyemail = _viewRender.RenderToStringAsync("ResetPassword", user);
            SendEmail.Send(user.Email, "بازیابی کلمه عبور", bodyemail);
            ViewBag.ForgotPassword = true;
            return View();
        }
        #endregion

        #region ResetPassword
        

        public IActionResult ResetPasswordc(string id)
        {

            return View(new ResetPasswordViewModel()
            {
                ActiveCode = id
            });
        }



        [HttpPost]
        public IActionResult ResetPasswordc(ResetPasswordViewModel resetPassword)
        {
            if(!ModelState.IsValid)
            {
                return View(resetPassword);
            }
               
            User user= _userService.GetUserByActiveCode(resetPassword.ActiveCode);

            if(user == null)            
                return NotFound();

            
            var hashNewPassword=PasswordHelper.EncodePasswordMd5(resetPassword.Password);
            user.Password = hashNewPassword;
            _userService.UpdateUser(user);
            ViewBag.Resetpass=true;
            return Redirect("/Login");
        }
        #endregion
    }
}
