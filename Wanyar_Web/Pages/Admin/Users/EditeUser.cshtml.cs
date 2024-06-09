using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Wanyar.Core.DTOs;
using Wanyar.Core.Security;
using Wanyar.Core.Services.Interfaces;

namespace Wanyar_Web.Pages.Admin.Users
{
    [PermissionChecker(1)]
    [PermissionChecker(6)]
    [PermissionChecker(8)]
    public class EditeUserModel : PageModel
    {

        private IUserService _userService;
        private IPermisionService _permisionService;
        public EditeUserModel(IUserService userService,IPermisionService permisionService)
        {
            _userService = userService;
            _permisionService = permisionService;
        }


        [BindProperty] 
        public EditeUserViewModel  editeUserViewModel { get; set; }
        public void OnGet(int id)
        {
            editeUserViewModel=_userService.GetUserForShowInEditeMode(id);
            
            ViewData["Roles"]=_permisionService.GetRoles();
        }


        public IActionResult OnPost(List<int> SelectedRole)
        {
            _userService.UpdateUserAdmin(editeUserViewModel);
            _permisionService.UpdateUserRole(SelectedRole, editeUserViewModel.UserId);
            

            return RedirectToPage("Index");
        }
    }
}
