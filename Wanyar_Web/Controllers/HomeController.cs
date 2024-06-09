using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using Wanyar.Core.Services.Interfaces;

namespace Wanyar_Web.Controllers
{
    public class HomeController : Controller
    {
        private IUserService _userService;
        private ICourseSevice _courseSevice;
        public HomeController(IUserService userService,ICourseSevice courseSevice)
        {
            _userService =userService; 
            _courseSevice = courseSevice;
        }

        public IActionResult Index()
        {
            ViewBag.PopularCourse=_courseSevice.GetPopularCourse();
            return View(_courseSevice.GetlistItemCourse());

        }


        [Route("OnlinePayment/{id}")]
        public IActionResult onlinePayment(int id)
        {
            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok"
                && HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"];

                var wallet = _userService.GetWalletByWalletId(id);

                var payment = new ZarinpalSandbox.Payment(wallet.Amount);
                var res = payment.Verification(authority).Result;
                if (res.Status == 100)
                {
                    ViewBag.code = res.RefId;
                    ViewBag.IsSuccess = true;
                    wallet.IsPay = true;
                    _userService.UpdateWallet(wallet);
                }
               
            }
            return View();

        }



        public IActionResult GetSubGroup(int id)
        {
            List<SelectListItem> List = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text="انتخاب کتید",
                    Value=""
                }
            };
            List.AddRange(_courseSevice.GetSubGroupForAddCourse(id));
            return Json(new SelectList(List, "Value", "Text"));
        }



        [HttpPost]
        [Route("file-upload")]
        public IActionResult UploadImage(IFormFile upload, string CKEditorFuncNum, string CKEditor, string langCode)
        {
            if (upload.Length <= 0) return null;

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();



            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/MyImages",
                fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);

            }



            var url = $"{"/MyImages/"}{fileName}";


            return Json(new { uploaded = true, url });
        }
    }
}
