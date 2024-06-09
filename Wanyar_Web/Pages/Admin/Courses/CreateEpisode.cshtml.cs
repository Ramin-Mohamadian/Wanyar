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
    [PermissionChecker(14)]
    public class CreateEpisodeModel : PageModel
    {
        private ICourseSevice _courseSevice;
        public CreateEpisodeModel(ICourseSevice courseSevice)
        {
            _courseSevice = courseSevice;
        }

        [BindProperty]
        public CourseEpisode CourseEpisode { get; set; }
        public void OnGet(int id)
        {
            CourseEpisode=new CourseEpisode();
            CourseEpisode.CourseId = id;
        }

        public IActionResult OnPost(IFormFile episodefile)
        {
            if (!ModelState.IsValid&&episodefile==null)
                return Page();

            if(_courseSevice.CheckExistFile(episodefile.FileName))
            {
                ViewData["ExistFile"]=true;
                return Page();
            }


            _courseSevice.AddEpisode(CourseEpisode, episodefile);

            return Redirect("/Admin/Courses/IndexEpisode/"+CourseEpisode.CourseId);
        }
    }
}
