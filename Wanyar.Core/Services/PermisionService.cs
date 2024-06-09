using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wanyar.Core.Services.Interfaces;
using Wanyar.DataLayer.Context;
using Wanyar.DataLayer.Entities.Permissions;
using Wanyar.DataLayer.Entities.Users;

namespace Wanyar.Core.Services
{
    public class PermisionService : IPermisionService
    {
        private WanyarContext _context;
        public PermisionService(WanyarContext context)
        {
            _context = context;
        }
        #region Roles

        public int AddRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role.RoleId;
        }

        public void AddRoleToUser(List<int> rolesIds, int userId)
        {
            foreach (var roleId in rolesIds)
            {
                _context.UserRoles.Add(new UserRole
                {
                    RoleId = roleId,
                    UserId = userId
                });
            }
            _context.SaveChanges();
        }

        public void DeleteRole(int roleId)
        {
            var role = GetRoleById(roleId);
            role.IsDelete=true;
            UpdateRole(role);
        }

    

        public Role GetRoleById(int roleId)
        {
            return _context.Roles.Find(roleId);
        }

        public List<Role> GetRoles(string filterName = "", int pageId = 1)
        {
            IQueryable<Role> AllRole = _context.Roles;
            if (!string.IsNullOrEmpty(filterName))
            {
                AllRole=_context.Roles.Where(r => r.RoleTitle.Contains(filterName));
            }



            return AllRole.ToList();
        }

        public void UpdateRole(Role role)
        {
            _context.Roles.Update(role);
            _context.SaveChanges();

        }

        public void UpdateUserRole(List<int> rolesIds, int userId)
        {
            _context.UserRoles.Where(r => r.UserId == userId).ToList().ForEach(r => _context.UserRoles.Remove(r));

            AddRoleToUser(rolesIds, userId);
        }
        #endregion

        #region Permission

        public List<Permission> GetAllPermision()
        {
            return _context.Permission.ToList();
        }

        public void AddPermissionToRole(int roleId, List<int> permission)
        {
            foreach (var p in permission)
            {
                _context.RolePermission.Add(new RolePermission()
                {
                    RoleId = roleId,
                    PermissionID =p
                });
            }
            _context.SaveChanges();
        }

        public List<int> PermissionsRole(int roleId)
        {
            return _context.RolePermission
                .Where(r => r.RoleId == roleId)
                .Select(r => r.PermissionID).ToList();
        }

        public void UpdatePermission(int roleId, List<int> permission)
        {
            _context.RolePermission.Where(p => p.RoleId==roleId)
                  .ToList().ForEach(p => _context.RolePermission.Remove(p));

            AddPermissionToRole(roleId, permission);
        }

        public bool CheckPermission(int permissionId, string userName)
        {
            int userid = _context.Users.Single(u=>u.UserName==userName).UserId;

          List<int>userRole=_context.UserRoles.Where(ur=>ur.UserId == userid).Select(ur=>ur.RoleId).ToList();

            if(!userRole.Any())
            {
                return false;
            }

            List<int>RolePermission=_context.RolePermission.Where(rp=>rp.PermissionID==permissionId).Select(rp=>rp.RoleId).ToList();

            return RolePermission.Any(p=>userRole.Contains(p));
        }

        #endregion
    }
}
