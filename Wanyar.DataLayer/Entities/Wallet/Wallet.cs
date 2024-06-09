using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wanyar.DataLayer.Entities.Users;

namespace Wanyar.DataLayer.Entities.Wallet
{
    public class Wallet
    {
        public Wallet()
        {
            
        }

        [Key]
        public int WalletId { get; set; }

        [Display(Name ="نوع تراکنش")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        public int TypeId { get; set; }

        [Display(Name ="کاربر")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        public int UserId { get; set; }

        [Display(Name ="مبلغ")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        public int Amount { get; set; }

        [Display(Name ="توضیحات")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        [MaxLength(350, ErrorMessage = "تعداد کاراکتر های وارد شده بیش از حد مجاز است")]
        public string Description { get; set; }


        [Display(Name ="تایید شده")]
        public bool IsPay { get; set; }

        [Display(Name ="تاریخ ایجاد")]
        public DateTime CreateDate { get; set; }


        #region Relations
        public virtual User User { get; set; }
        public virtual WalletType WalletType { get; set; }
        #endregion


    }
}
