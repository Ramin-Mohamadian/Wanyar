using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wanyar.DataLayer.Entities.Permissions
{
    public  class Permission
    {
        [Key]
        public int PermissionID { get; set; }
        [Display(Name ="عنوان دسترسی")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        [MaxLength(350, ErrorMessage = "تعداد کاراکتر های وارد شده بیش از حد مجاز است")]
        public string PermissionTitle { get; set; }
        public int? ParentID { get; set; }

        [ForeignKey("ParentID")]
        public List<Permission> permissions { get; set; }
        public List<RolePermission> rolePermissions { get; set; }
    }
}
