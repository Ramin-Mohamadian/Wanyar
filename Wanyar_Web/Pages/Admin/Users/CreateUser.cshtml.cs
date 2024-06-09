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
    [PermissionChecker(7)]
    public class CreateUserModel : PageModel
    {
        private IPermisionService _permisionService;
        private IUserService _userService;
        public CreateUserModel(IPermisionService permisionService, IUserService userService)
        {
            _permisionService = permisionService;
            _userService=userService;

        }

        [BindProperty]
        public CreateUserViewModel  CreateUserViewModel { get; set; }
        public void OnGet()
        {
            ViewData["Roles"]=_permisionService.GetRoles();
        }


        public IActionResult OnPost(List<int> SelectedRole)
        {
            if(!ModelState.IsValid)                 
            return Page();


            int userid = _userService.AddUserAdmin(CreateUserViewModel);

            //AddRoles
            _permisionService.AddRoleToUser(SelectedRole, userid);


            return Redirect("/Admin/Users");
        }
    }
}
