using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wanyar.DataLayer.Entities.Course
{
    public  class UserCourse
    {
        [Key] 
        public int UC_Id { get; set; }
        public int UserId { get; set; }
        public int CourseId { get; set; }


        public Course Course { get; set; }
        public Users.User User { get; set; }
    }
}
