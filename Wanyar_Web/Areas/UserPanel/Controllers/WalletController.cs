using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wanyar.Core.DTOs;
using Wanyar.Core.Services.Interfaces;
using Wanyar.DataLayer.Entities.Wallet;

namespace Wanyar_Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class WalletController : Controller
    {
        private IUserService _userService;
        public WalletController(IUserService userService)
        {
            _userService = userService;
        }


        [Route("UserPanel/Wallet")]
        public IActionResult Index()
        {
            ViewBag.wallet=_userService.ShowWallet(User.Identity.Name);
            return View();
        }

        [Route("UserPanel/Wallet")]
        [HttpPost]
        public IActionResult Index(ChargeWalletViewModel charge)
        {
           if(!ModelState.IsValid)
            {
                ViewBag.wallet=_userService.ShowWallet(User.Identity.Name);
                return View(charge);
            }

          int walletid= _userService.ChargeWallet(User.Identity.Name, charge.Amont,"شارژ حساب",false);

            #region Online Payment
            var payment = new ZarinpalSandbox.Payment(charge.Amont);

            var res = payment.PaymentRequest("شارژ کیف پول", "https://localhost:44344/OnlinePayment/" + walletid, "Info@topLearn.Com", "09197070750");

            if (res.Result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + res.Result.Authority);
            }
            #endregion
            return null;
        }
    }
}
