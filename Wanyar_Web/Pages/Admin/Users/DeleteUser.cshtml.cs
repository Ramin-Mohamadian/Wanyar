using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wanyar.Core.DTOs;
using Wanyar.Core.Security;
using Wanyar.Core.Services.Interfaces;

namespace Wanyar_Web.Pages.Admin.Users
{
    [PermissionChecker(1)]
    [PermissionChecker(6)]
    [PermissionChecker(9)]
    public class DeleteUserModel : PageModel
    {

        private IUserService _userService;
        public DeleteUserModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public UserInformationViewModel UserInformationViewModel { get; set; }
        public void OnGet(int id)
        {
            UserInformationViewModel=_userService.GetUserInformationById(id);
        }


        public IActionResult OnPost(int id)
        {
            _userService.DeleteUser(id);

            return RedirectToPage("Index");
        }
    }
}
