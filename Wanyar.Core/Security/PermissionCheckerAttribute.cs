using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wanyar.Core.Services.Interfaces;

namespace Wanyar.Core.Security
{
    public class PermissionCheckerAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private IPermisionService _permisionService;
        private int _permissionId = 0;
        public PermissionCheckerAttribute(int permissionId)
        {
            _permissionId = permissionId;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _permisionService=(IPermisionService)context.HttpContext.RequestServices.GetService(typeof(IPermisionService));
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                string username = context.HttpContext.User.Identity.Name;

                if (!_permisionService.CheckPermission(_permissionId, username))
                {
                    context.Result=new RedirectResult("/Login");
                }

            }
            else
            {
                context.Result=new RedirectResult("/Login");
            }
        }
    }
}
