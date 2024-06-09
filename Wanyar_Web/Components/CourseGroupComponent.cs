using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Wanyar.Core.Services;
using Wanyar.Core.Services.Interfaces;

namespace Wanyar_Web.Components
{
    public class CourseGroupComponent:ViewComponent
    {
        private ICourseSevice _courseSevice;
        public CourseGroupComponent(ICourseSevice courseSevice)
        {
            _courseSevice = courseSevice;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult)View("CourseGroup", _courseSevice.GetAllGroup()));
        }

        
    }
}
