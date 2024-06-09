using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wanyar.DataLayer.Entities.Permissions;

namespace Wanyar.DataLayer.Entities.Users
{
    public  class Role
    {
        public Role()
        {
            
        }



        [Key]
        public int RoleId { get; set; }
        [Display(Name ="عنوان نقش")]
        [MaxLength(350, ErrorMessage = "تعداد کاراکتر های وارد شده بیش از حد مجاز است")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        public string RoleTitle { get; set; }

        public bool IsDelete { get; set; }
        #region Relation
        public List<UserRole> UserRoles { get; set; }
        public List<RolePermission> rolePermissions { get; set; }
        #endregion


    }
}
