using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Wanyar.Core.DTOs;
using Wanyar.Core.Services.Interfaces;

namespace Wanyar_Web.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    [Authorize]
    public class MyOrderController : Controller
    {
        private IOrderService _orderService;
        public MyOrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        public IActionResult Index()
        {
            return View(_orderService.GetUserOrders(User.Identity.Name));
        }


       
        public IActionResult ShowOrder(int id,bool finaly=false,string type="")
        {
            ViewBag.ResultDiscountType=type;
            var order=_orderService.GetOrderForUserPanel(User.Identity.Name,id);
            if(order == null)
            {
                return NotFound();  
            }
            ViewBag.finaly=finaly;
            return View(order);
        }


        public IActionResult FinalyOrder(int id)
        {

            if(_orderService.FinalyOrder(User.Identity.Name,id))
            {
                return Redirect("/UserPanel/MyOrder/ShowOrder/"+id+"?finaly=true");
            }
            return BadRequest();
           
        }



        public IActionResult UseDiscount(int orderId,string code)
        {
            DiscountUseType type=_orderService.UseDiscount(orderId,code);

            return Redirect("/UserPanel/MyOrder/ShowOrder/"+orderId+"?type="+type.ToString());

        }
    }
}
