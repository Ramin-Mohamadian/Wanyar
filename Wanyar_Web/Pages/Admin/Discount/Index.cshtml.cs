using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Wanyar.Core.Security;
using Wanyar.Core.Services.Interfaces;
using Wanyar.DataLayer.Entities.Order;

namespace Wanyar_Web.Pages.Admin.Discount
{
    [PermissionChecker(1)]
    [PermissionChecker(17)]

    public class IndexModel : PageModel
    {
        private IOrderService _orderService;
        public IndexModel(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [BindProperty]
        public List< Wanyar.DataLayer.Entities.Order.Discount> Discount { get; set; }
        public void OnGet()
        {
            Discount=_orderService.GetAllDiscount();
        }
    }
}
