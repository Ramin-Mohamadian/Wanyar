using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wanyar.DataLayer.Entities.Users;

namespace Wanyar.DataLayer.Entities.Course
{
    public class CourseComment
    {
        [Key] 
        public int commentId { get; set; }
        public int CourseId { get; set; }
        public int UserId { get; set; }
        [MaxLength(800)]
        public string Comment { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsAdminReade { get; set; }


        public Course Course { get; set; }
        public User User { get; set; }
    }
}
