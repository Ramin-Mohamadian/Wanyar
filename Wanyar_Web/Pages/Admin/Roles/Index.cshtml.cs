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
    public class IndexModel : PageModel
    {

        private IPermisionService _permisionService;
        public IndexModel(IPermisionService permisionService)
        {
            _permisionService = permisionService;   
        }


        [BindProperty]
        public List<Role> AllRole { get; set; }
        public void OnGet(string filterName="")
        {
            AllRole=_permisionService.GetRoles(filterName);
        }



    }
}
