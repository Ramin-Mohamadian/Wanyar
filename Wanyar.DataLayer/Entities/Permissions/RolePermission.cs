using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wanyar.DataLayer.Entities.Users;

namespace Wanyar.DataLayer.Entities.Permissions
{
    public  class RolePermission
    {
        [Key]
        public int RP_ID { get; set; }
        public int RoleId { get; set; }
        public int PermissionID { get; set; }

        public Role Role { get; set; }
        public Permission Permission { get; set; }

    }
}
