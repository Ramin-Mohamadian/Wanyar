using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using Wanyar.Core.Security;
using Wanyar.Core.Services;
using Wanyar.Core.Services.Interfaces;
using Wanyar.DataLayer.Entities.Course;

namespace Wanyar_Web.Pages.Admin.Courses
{

    [PermissionChecker(1)]
    [PermissionChecker(10)]
    [PermissionChecker(12)]
    public class EditeCourseModel : PageModel
    {
        private ICourseSevice _courseSevice;
        public EditeCourseModel(ICourseSevice courseSevice)
        {
            _courseSevice = courseSevice;
        }

        [BindProperty]
        public Course Course { get; set; }

        public void OnGet(int id)
        {
            Course=_courseSevice.GetCourseById(id);

            var group = _courseSevice.GetGroupForAddCourse();
            ViewData["Groups"]=new SelectList(group, "Value", "Text",Course.GroupId);

            var subgroup = _courseSevice.GetSubGroupForAddCourse(Course.GroupId);
            ViewData["SubGroups"]=new SelectList(subgroup, "Value", "Text",Course.SubGroup??0);


            var teachers = _courseSevice.GetTeacherOfCourse();
            ViewData["Teachers"]=new SelectList(teachers, "Value", "Text",Course.TeacherId);

            var level = _courseSevice.GetLevelCouse();
            ViewData["Levels"]=new SelectList(level, "Value", "Text",Course.LevelId);

            var statues = _courseSevice.GetStatuesCourse();
            ViewData["Statues"]=new SelectList(statues, "Value", "Text",Course.StatusId);
        }

        public IActionResult OnPost(IFormFile imgCourseUp, IFormFile demoUp)
        {
            if (!ModelState.IsValid)
                return Page();

            _courseSevice.UpdateCourse(Course, imgCourseUp, demoUp);

            return RedirectToPage("Index");
        }
    }
}
