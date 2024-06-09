using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Wanyar.Core.Security;
using Wanyar.Core.Services.Interfaces;
using Wanyar.DataLayer.Entities.Course;

namespace Wanyar_Web.Pages.Admin.Coursegroup
{
    [PermissionChecker(1)]
    [PermissionChecker(20)]
    [PermissionChecker(21)]
    
    public class CreateGroupModel : PageModel
    {
        private ICourseSevice _courseSevice;
        public CreateGroupModel(ICourseSevice courseSevice)
        {
            _courseSevice = courseSevice;
        }


        [BindProperty]
        public CourseGroup  CourseGroup { get; set; }
        public void OnGet(int? id)
        {
            CourseGroup=new CourseGroup()
            {
                ParentId = id,
            };
        }

        public IActionResult OnPost()
        {
            if(!ModelState.IsValid)
                return Page();

            _courseSevice.AddGroup(CourseGroup);
            return RedirectToPage("Index");
        }
    }
}
