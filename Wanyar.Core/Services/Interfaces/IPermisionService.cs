using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wanyar.DataLayer.Entities.Permissions;
using Wanyar.DataLayer.Entities.Users;

namespace Wanyar.Core.Services.Interfaces
{
    public  interface IPermisionService
    {
        #region Roles
        List<Role> GetRoles(string filterName = "", int pageId = 1);

        void AddRoleToUser(List<int> rolesIds, int userId);
        void UpdateUserRole(List<int> rolesIds, int userId);

        int AddRole(Role role);
        Role GetRoleById(int roleId);

        void UpdateRole(Role role);

        void DeleteRole(int roleId);
        #endregion


        #region Permission
        List<Permission> GetAllPermision();
        void AddPermissionToRole(int  roleId, List<int> permission);
        List<int> PermissionsRole(int roleId);
        void UpdatePermission(int roleId, List<int> permission);

        bool CheckPermission(int permissionId,string userName);

        #endregion
    }
}
