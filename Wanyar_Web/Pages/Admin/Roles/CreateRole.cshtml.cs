using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Wanyar.Core.Security;
using Wanyar.Core.Services.Interfaces;
using Wanyar.DataLayer.Entities.Users;

namespace Wanyar_Web.Pages.Admin.Roles
{
    [PermissionChecker(1)]
    [PermissionChecker(2)]
    [PermissionChecker(3)]

    public class CreateRoleModel : PageModel
    {
        private IPermisionService _permisionService;
        public CreateRoleModel(IPermisionService permisionService)
        {
            _permisionService = permisionService;
        }
        [BindProperty]
        public Role Role { get; set; }
        public void OnGet()
        {
            ViewData["Permission"]=_permisionService.GetAllPermision();
        }

        public IActionResult OnPost(List<int> SelectedPermission)
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            var roleid=_permisionService.AddRole(Role);


            _permisionService.AddPermissionToRole(roleid, SelectedPermission);


            return RedirectToPage("Index");
        }
    }
}
