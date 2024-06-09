using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wanyar.DataLayer.Entities.Users;

namespace Wanyar.DataLayer.Entities.Order
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]

        public int UserId { get; set; }

        [Required]

        public bool IsFinaly { get; set; }
        [Required]

        public DateTime CreateDate { get; set; }
        [Required]

        public int OrderSum { get; set; }


        #region Relations
        public  virtual User User { get; set; }
        public virtual List<OrderDetail> OrderDetails { get; set; }

        #endregion
    }
}
