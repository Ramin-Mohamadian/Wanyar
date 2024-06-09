using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wanyar.Core.DTOs;
using Wanyar.Core.Security;
using Wanyar.Core.Services.Interfaces;

namespace Wanyar_Web.Pages.Admin.Users
{
    [PermissionChecker(1)]
    [PermissionChecker(2)]
    [PermissionChecker(5)]
    public class ListDeleteUsersModel : PageModel
    {
        private IUserService _userService;
        public ListDeleteUsersModel(IUserService userService)
        {
            _userService = userService;
        }

        public UserForAdminViewModel UserForAdminViewModel { get; set; }
        public void OnGet(int pageId = 1, string FilterUserName = "", string FilterEmail = "")
        {
            UserForAdminViewModel=_userService.GetDeleteUserForAdmin(pageId, FilterUserName, FilterEmail);
        }
    }
}
