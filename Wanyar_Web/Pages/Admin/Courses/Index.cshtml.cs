using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Wanyar.Core.DTOs;
using Wanyar.Core.Security;
using Wanyar.Core.Services.Interfaces;

namespace Wanyar_Web.Pages.Admin.Courses
{
    [PermissionChecker(1)]
    [PermissionChecker(10)]

    public class IndexModel : PageModel
    {
        private ICourseSevice _courseSevice;
        public IndexModel(ICourseSevice courseSevice)
        {
            _courseSevice = courseSevice;
        }



        [BindProperty]
        public List<ShowCourseForAddminViewModel> ShowCourses { get; set; }
        public void OnGet()
        {
            ShowCourses=_courseSevice.ShowCourseForAddminViewModels();   
        }

    }
}
