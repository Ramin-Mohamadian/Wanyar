using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wanyar.DataLayer.Entities;
using Wanyar.DataLayer.Entities.Course;
using Wanyar.DataLayer.Entities.Order;
using Wanyar.DataLayer.Entities.Wallet;

namespace Wanyar.DataLayer.Entities.Users
{
    public class User
    {

        public User()
        {

        }


        [Key]
        public int UserId { get; set; }

        [Display(Name = "نام کاربری")]
        [MaxLength(350, ErrorMessage = "تعداد کاراکتر های وارد شده بیش از حد مجاز است")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string UserName { get; set; }


        [Display(Name = "ایمیل")]
        [MaxLength(350, ErrorMessage = "تعداد کاراکتر های وارد شده بیش از حد مجاز است")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Email { get; set; }

        [Display(Name = "کلمه عبور")]
        [MaxLength(350, ErrorMessage = "تعداد کاراکتر های وارد شده بیش از حد مجاز است")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Password { get; set; }


        [Display(Name = "کد فعال سازی")]
        [MaxLength(350, ErrorMessage = "تعداد کاراکتر های وارد شده بیش از حد مجاز است")]

        public string ActiveCode { get; set; }

        [Display(Name = "وضعیت")]

        public bool IsActive { get; set; }


        [Display(Name = "تصویر کاربر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string UserAvatar { get; set; }


        [Display(Name = "تاریخ ثبت نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public DateTime RegisterDate { get; set; }

        [Display(Name ="حذف")]
        public bool IsDelete { get; set; }

        #region Relation
        public virtual List<UserRole> UserRoles { get; set; }
        public virtual List<Wallet.Wallet> Wallet { get; set; }

        public virtual List<Course.Course> Courses { get; set; }

        public virtual List<Order.Order> Order { get; set; }
        public List<UserCourse> UserCourse { get; set; }
        public List<UserDiscountCode> UserDiscountCodes { get; set; }
        public List<CourseComment>  CourseComments { get; set; }

        #endregion
    }
}
