using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wanyar.DataLayer.Entities.Wallet
{
    public class WalletType
    {
        public WalletType()
        {
            
        }

        [Key,DatabaseGenerated(DatabaseGeneratedOption.None)] 
        public int TypeId { get; set; }
        

        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        [MaxLength(350, ErrorMessage = "تعداد کاراکتر های وارد شده بیش از حد مجاز است")]
        public string TypeTitle { get; set; }


        #region Relations
        public virtual List<Wallet> Wallet { get; set; }
        #endregion

    }
}
