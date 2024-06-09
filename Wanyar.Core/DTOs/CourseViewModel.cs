using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wanyar.DataLayer.Entities.Course;

namespace Wanyar.Core.DTOs
{
    public  class ShowCourseForAddminViewModel
    {
        public int CourseId { get; set; }
        public string  Title{ get; set; }
        public string ImageName { get; set; }
        public string TeacherId { get; set; }
        public int EpisodeCount { get; set; }
        public DateTime CreateDate { get; set; }
        
    }



    public class ShowItemCourse
    {
        public int CourseId { get; set; }
        public string ImageName { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
        public TimeSpan TotalTime { get; set; }

    }
}
