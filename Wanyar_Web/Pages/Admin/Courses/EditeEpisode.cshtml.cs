using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wanyar.Core.Security;
using Wanyar.Core.Services.Interfaces;
using Wanyar.DataLayer.Entities.Course;

namespace Wanyar_Web.Pages.Admin.Courses
{
    [PermissionChecker(1)]
    [PermissionChecker(10)]
    [PermissionChecker(15)]
    public class EditeEpisodeModel : PageModel
    {
        private ICourseSevice _courseSevice;
        public EditeEpisodeModel(ICourseSevice courseSevice)
        {
                _courseSevice = courseSevice;
        }


        [BindProperty]
        public CourseEpisode  CourseEpisode { get; set; }

        public void OnGet(int id)
        {
            CourseEpisode=_courseSevice.GetEpisodeById(id);
        }

        public IActionResult OnPost(IFormFile episodefile)
        {
            if (!ModelState.IsValid)
                return Page();

         


            _courseSevice.UpdateEpisode(CourseEpisode, episodefile);

            return Redirect("/Admin/Courses/IndexEpisode/"+CourseEpisode.CourseId);
        }
    }
}
