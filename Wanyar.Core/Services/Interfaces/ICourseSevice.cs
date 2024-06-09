using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wanyar.Core.DTOs;
using Wanyar.DataLayer.Entities.Course;

namespace Wanyar.Core.Services.Interfaces
{
    public interface ICourseSevice
    {
        #region Group
        List<CourseGroup> GetAllGroup();
        List<SelectListItem> GetGroupForAddCourse();
        List<SelectListItem> GetSubGroupForAddCourse(int id);

        List<SelectListItem> GetTeacherOfCourse();
        List<SelectListItem> GetLevelCouse();
        List<SelectListItem> GetStatuesCourse();

        void AddGroup(CourseGroup group);
        void UpdateGroup(CourseGroup group);
        CourseGroup GetGroupById(int id);

        #endregion

        #region Course
        int AddCourse(Course course, IFormFile imgCourseUp, IFormFile demoUp);
        List<ShowCourseForAddminViewModel> ShowCourseForAddminViewModels();

        Course GetCourseById(int id);

        void UpdateCourse(Course course, IFormFile imgCourse, IFormFile courseDemo);

        Tuple<List<ShowItemCourse>,int> GetlistItemCourse(int pageId=1,string search="",string GetType="All", string orderByType = "date",
            int startPrice = 0, int endPrice = 0, List<int> selectedGroups = null, int take = 0);

        Course GetDetailsCourse(int id);

        List<ShowItemCourse> GetPopularCourse();

        #endregion

        #region Episode
        int AddEpisode(CourseEpisode episode, IFormFile episodefile);
        bool CheckExistFile(string filename);
        List<CourseEpisode> GetAllEpisodeForCourse(int courseId);
        CourseEpisode GetEpisodeById(int id);

        void UpdateEpisode(CourseEpisode episode, IFormFile episodefile);

        #endregion

        #region Comment
        void AddComment(CourseComment courseComment);
        Tuple<List<CourseComment>,int> GetAllComments(int courseId,int pageId=1);
     
        #endregion
    }
}
