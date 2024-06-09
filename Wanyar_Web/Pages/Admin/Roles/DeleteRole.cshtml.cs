using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wanyar.Core.Security;
using Wanyar.Core.Services.Interfaces;
using Wanyar.DataLayer.Entities.Users;

namespace Wanyar_Web.Pages.Admin.Roles
{
    [PermissionChecker(1)]
    [PermissionChecker(2)]
    [PermissionChecker(5)]
    public class DeleteRoleModel : PageModel
    {
        private IPermisionService _permisionService;
        public DeleteRoleModel(IPermisionService permisionService)
        {
            _permisionService = permisionService;
        }

        [BindProperty]
        public Role role { get; set; }
        public void OnGet(int id)
        {
            role=_permisionService.GetRoleById(id);
        }

        public IActionResult OnPost(int id)
        {
           _permisionService.DeleteRole(id);
            return RedirectToPage("Index");
        }
    }
}
