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
    [PermissionChecker(4)]
    public class EditRoleModel : PageModel
    {
        private IPermisionService _permisionService;
        public EditRoleModel(IPermisionService permisionService)
        {
            _permisionService = permisionService;
        }
        [BindProperty]
        public Role Role { get; set; }
        public void OnGet(int id)
        {
            Role=_permisionService.GetRoleById(id);
            ViewData["Permission"]=_permisionService.GetAllPermision();
            ViewData["SelectedPermission"]=_permisionService.PermissionsRole(id);
        }


        public IActionResult OnPost(List<int> SelectedPermission)
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            _permisionService.UpdateRole(Role);
            _permisionService.UpdatePermission(Role.RoleId,SelectedPermission);

            return RedirectToPage("Index");
        }
    }
}
