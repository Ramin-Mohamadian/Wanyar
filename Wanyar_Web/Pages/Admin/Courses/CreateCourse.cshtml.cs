using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Wanyar.Core.Security;
using Wanyar.Core.Services.Interfaces;
using Wanyar.DataLayer.Entities.Course;

namespace Wanyar_Web.Pages.Admin.Courses
{

    [PermissionChecker(1)]
    [PermissionChecker(10)]
    [PermissionChecker(11)]
    public class CreateCourseModel : PageModel
    {


        private ICourseSevice _courseSevice;
        public CreateCourseModel(ICourseSevice courseSevice)
        {
            _courseSevice = courseSevice;
        }

        [BindProperty]
        public Course  Course  { get; set; }

        public void OnGet()
        {
            var group = _courseSevice.GetGroupForAddCourse();
            ViewData["Groups"]=new SelectList(group, "Value", "Text");

            var subgroup = _courseSevice.GetSubGroupForAddCourse(int.Parse(group.First().Value));
            ViewData["SubGroups"]=new SelectList(subgroup, "Value", "Text");


            var teachers = _courseSevice.GetTeacherOfCourse();
            ViewData["Teachers"]=new SelectList(teachers, "Value", "Text");

            var level = _courseSevice.GetLevelCouse();
            ViewData["Levels"]=new SelectList(level, "Value", "Text");

            var statues = _courseSevice.GetStatuesCourse();
            ViewData["Statues"]=new SelectList(statues, "Value", "Text");
        }

        public IActionResult OnPost(IFormFile imgCourseUp,IFormFile demoUp)
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            _courseSevice.AddCourse(Course, imgCourseUp, demoUp);

            return RedirectToPage("Index");
        }
    }
}
