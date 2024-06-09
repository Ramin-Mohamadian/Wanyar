using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Wanyar.Core.Security;
using Wanyar.Core.Services.Interfaces;
using Wanyar.DataLayer.Entities.Course;

namespace Wanyar_Web.Pages.Admin.Courses
{
    [PermissionChecker(1)]
    [PermissionChecker(10)]
    public class IndexEpisodeModel : PageModel
    {
        private ICourseSevice _courseSevice;
        public IndexEpisodeModel(ICourseSevice courseSevice)
        {
            _courseSevice = courseSevice;
        }

        public List<CourseEpisode>  CourseEpisodes { get; set; }
        public void OnGet(int id)
        {
            ViewData["CourseId"]=id;
            CourseEpisodes=_courseSevice.GetAllEpisodeForCourse(id);
        }
    }
}
