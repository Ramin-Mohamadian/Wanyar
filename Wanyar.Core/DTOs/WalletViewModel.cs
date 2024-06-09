using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wanyar.Core.DTOs
{
    public class ChargeWalletViewModel
    {
        [Display(Name = "مبلغ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Amont { get; set; }
    }

    public class WalletViewModel
    {
        public int Amount { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public int TypeId { get; set; }


    }
}
