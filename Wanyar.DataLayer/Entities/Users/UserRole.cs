using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wanyar.DataLayer.Entities.Users
{
    public  class UserRole
    {
        public UserRole()
        {
            
        }

        [Key]
        public int UR_Id { get; set; }
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        public int UserId { get; set; }
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        public int RoleId { get; set; }


        #region Relations
        public Role Role { get; set; }
        public User User { get; set; }
        #endregion

    }
}
