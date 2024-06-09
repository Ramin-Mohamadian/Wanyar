using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using Wanyar.Core.Security;
using Wanyar.Core.Services.Interfaces;
using Wanyar.DataLayer.Entities.Course;

namespace Wanyar_Web.Pages.Admin.Coursegroup
{

    [PermissionChecker(1)]
    [PermissionChecker(20)]
    
    public class IndexModel : PageModel
    {
        private ICourseSevice _courseSevice;
        public IndexModel(ICourseSevice courseSevice)
        {
            _courseSevice = courseSevice;
        }


        [BindProperty]
        public List<CourseGroup> CourseGroup { get; set; }
        public void OnGet()
        {
            CourseGroup=_courseSevice.GetAllGroup();
        }
    }
}
