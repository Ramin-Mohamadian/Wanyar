using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wanyar.Core.Convertors;
using Wanyar.Core.DTOs;
using Wanyar.Core.Generator;
using Wanyar.Core.Security;
using Wanyar.Core.Services.Interfaces;
using Wanyar.DataLayer.Context;
using Wanyar.DataLayer.Entities.Course;
using static System.Net.WebRequestMethods;

namespace Wanyar.Core.Services
{
    public class CourseSevice : ICourseSevice
    {
        private WanyarContext _context;
        private IUserService _userService;

      
        public CourseSevice(WanyarContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }






        #region Group

        public List<CourseGroup> GetAllGroup()
        {
            return _context.CourseGroups.Include(c=>c.CourseGroups).ToList();
        }

        public List<SelectListItem> GetGroupForAddCourse()
        {
            return _context.CourseGroups.Where(g => g.ParentId==null).Select(g => new SelectListItem()
            {
                Text=g.GroupTitle,
                Value=g.GroupId.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetLevelCouse()
        {
            return _context.CourseLevels.Select(g => new SelectListItem()
            {
                Text=g.LevelTitle,
                Value=g.LevelId.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetStatuesCourse()
        {
            return _context.CourseStatuses.Select(g => new SelectListItem()
            {
                Text=g.StatusTitle,
                Value=g.StatusId.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetSubGroupForAddCourse(int id)
        {
            return _context.CourseGroups.Where(g => g.ParentId==id).Select(g => new SelectListItem()
            {
                Text=g.GroupTitle,
                Value=g.GroupId.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetTeacherOfCourse()
        {
            return _context.UserRoles.Where(r => r.RoleId==4).Include(r => r.User).Select(u => new SelectListItem()
            {
                Text=u.User.UserName,
                Value=u.UserId.ToString()
            }).ToList();



        }


        public void AddGroup(CourseGroup group)
        {
            _context.CourseGroups.Add(group);
            _context.SaveChanges();
        }

        public void UpdateGroup(CourseGroup group)
        {
            _context.CourseGroups.Update(group);
            _context.SaveChanges();
        }

        public CourseGroup GetGroupById(int id)
        {
           return _context.CourseGroups.Find(id);
        }

        #endregion


        #region Course
        public int AddCourse(Course course, IFormFile imgCourseUp, IFormFile demoUp)
        {
            course.CreateDate = DateTime.Now;
            course.CourseImageName = "no-photo.jpg";
            //TODo:Cheacked image
            if (imgCourseUp != null&&imgCourseUp.IsImage())
            {
                course.CourseImageName =NameGenerator.GenerateUniqeCode() + Path.GetExtension(imgCourseUp.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/image", course.CourseImageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgCourseUp.CopyTo(stream);
                }


                //TODO:ImageResize

                ImageConvertor imgresizer = new ImageConvertor();
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/thumb", course.CourseImageName);

                imgresizer.Image_resize(imagePath, thumbPath, 180);



            }

            //TO Do Demo

            if (demoUp != null)
            {
                course.DemoFileName =NameGenerator.GenerateUniqeCode() + Path.GetExtension(demoUp.FileName);
                string demoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/demos", course.DemoFileName);
                using (var stream = new FileStream(demoPath, FileMode.Create))
                {
                    demoUp.CopyTo(stream);
                }
            }

            _context.Courses.Add(course);
            _context.SaveChanges();
            return course.CourseId;
        }

        public List<ShowCourseForAddminViewModel> ShowCourseForAddminViewModels()
        {
            return _context.Courses.Select(c => new ShowCourseForAddminViewModel()
            {
                CourseId = c.CourseId,
                CreateDate=c.CreateDate,
                EpisodeCount=c.CourseEpisodes.Count,
                ImageName=c.CourseImageName,
                Title=c.CourseTitle,
                TeacherId=_userService.GetUserNameByUserId(c.TeacherId)              

        }).ToList();
        }

        public Course GetCourseById(int id)
        {
            return _context.Courses.Find(id);
        }

        public void UpdateCourse(Course course, IFormFile imgCourse, IFormFile courseDemo)
        {
            course.UpdateDate=DateTime.Now;

            if (imgCourse != null && imgCourse.IsImage())
            {
                if (course.CourseImageName != "no-photo.jpg")
                {
                    string deleteimagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/image", course.CourseImageName);
                    if (System.IO.File.Exists(deleteimagePath))
                    {
                        System.IO.File.Delete(deleteimagePath);
                    }

                    string deletethumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/thumb", course.CourseImageName);
                    if (System.IO.File.Exists(deletethumbPath))
                    {
                        System.IO.File.Delete(deletethumbPath);
                    }
                }
                course.CourseImageName = NameGenerator.GenerateUniqeCode() + Path.GetExtension(imgCourse.FileName);
                string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/image", course.CourseImageName);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    imgCourse.CopyTo(stream);
                }

                ImageConvertor imgResizer = new ImageConvertor();
                string thumbPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/thumb", course.CourseImageName);

                imgResizer.Image_resize(imagePath, thumbPath, 180);
            }

            if (courseDemo != null)
            {
                if (course.DemoFileName != null)
                {
                    string deleteDemoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/demos", course.DemoFileName);
                    if (System.IO.File.Exists(deleteDemoPath))
                    {
                        System.IO.File.Delete(deleteDemoPath);
                    }
                }
                course.DemoFileName = NameGenerator.GenerateUniqeCode() + Path.GetExtension(courseDemo.FileName);
                string demoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Course/demos", course.DemoFileName);
                using (var stream = new FileStream(demoPath, FileMode.Create))
                {
                    courseDemo.CopyTo(stream);
                }
            }

            _context.Courses.Update(course);
            _context.SaveChanges();
        }

        public Tuple<List<ShowItemCourse>, int> GetlistItemCourse(int pageId = 1, string search = "", string GetType = "all", string orderByType = "date", int startPrice = 0, int endPrice = 0, List<int> selectedGroups = null, int take = 0)
        {
            if (take == 0)
                take = 8;

            IQueryable<Course> result = _context.Courses;

            if (!string.IsNullOrEmpty(search))
            {
                result = result.Where(c => c.CourseTitle.Contains(search));
            }

            switch (GetType)
            {
                case "all":
                    break;
                case "buy":
                    {
                        result = result.Where(c => c.CoursePrice != 0);
                        break;
                    }
                case "free":
                    {
                        result = result.Where(c => c.CoursePrice == 0);
                        break;
                    }

            }

            switch (orderByType)
            {
                case "date":
                    {
                        result = result.OrderByDescending(c => c.CreateDate);
                        break;
                    }
                case "updatedate":
                    {
                        result = result.OrderByDescending(c => c.UpdateDate);
                        break;
                    }
            }

            if (startPrice > 0)
            {
                result = result.Where(c => c.CoursePrice > startPrice);
            }

            if (endPrice > 0)
            {
                result = result.Where(c => c.CoursePrice < startPrice);
            }


            if (selectedGroups != null && selectedGroups.Any())
            {
                foreach (int groupId in selectedGroups)
                {
                    result = result.Where(c => c.GroupId == groupId || c.SubGroup == groupId);
                }

            }

            int skip = (pageId - 1) * take;

            int pageCount = result.Include(c => c.CourseEpisodes).Select(c => new ShowItemCourse()
            {
                CourseId = c.CourseId,
                ImageName = c.CourseImageName,
                Price = c.CoursePrice,
                Title = c.CourseTitle,
                //TotalTime = new TimeSpan(c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks))

            }).Count() / take;

            var query = result.Include(c => c.CourseEpisodes).Select(c => new ShowItemCourse()
            {
                CourseId = c.CourseId,
                ImageName = c.CourseImageName,
                Price = c.CoursePrice,
                Title = c.CourseTitle,
                //TotalTime = new TimeSpan(c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks))

            }).Skip(skip).Take(take).ToList();

            return Tuple.Create(query, pageCount);
        }



        public Course GetDetailsCourse(int id)
        {
            return _context.Courses.Include(c=>c.CourseEpisodes).Include(c=>c.CourseStatus).Include(c=>c.UserCourse)
                .Include(c=>c.CourseLevel).Include(c => c.User).FirstOrDefault(c=>c.CourseId == id);
        }


        public List<ShowItemCourse> GetPopularCourse()
        {
            return _context.Courses.Include(c => c.orderDetails)
                .Where(c => c.orderDetails.Any())
                .OrderByDescending(d => d.orderDetails.Count)
                .Take(8)
                .Select(c => new ShowItemCourse()
                {
                    CourseId = c.CourseId,
                    ImageName = c.CourseImageName,
                    Price = c.CoursePrice,
                    Title = c.CourseTitle,                    
                   // TotalTime = new TimeSpan(c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks))
                })
                .ToList();
        }



        #endregion


        #region Episode
        public int AddEpisode(CourseEpisode episode, IFormFile episodefile)
        {

            episode.EpisodeFileName=episodefile.FileName;
            string episodePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseEpisode", episode.EpisodeFileName);

            using (var stream = new FileStream(episodePath, FileMode.Create))
            {
                episodefile.CopyTo(stream);
            }

            _context.CourseEpisodes.Add(episode);
            _context.SaveChanges();
            return episode.EpisodeId;
        }

        public bool CheckExistFile(string filename)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseEpisode", filename);
            return System.IO.File.Exists(filePath);
        }

        public List<CourseEpisode> GetAllEpisodeForCourse(int courseId)
        {
            return _context.CourseEpisodes.Where(e => e.CourseId==courseId).ToList();
        }

        public CourseEpisode GetEpisodeById(int id)
        {
            return _context.CourseEpisodes.Find(id);
        }

        public void UpdateEpisode(CourseEpisode episode, IFormFile episodefile)
        {

            if (episodefile!=null)
            {
                string deleteFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseEpisode", episode.EpisodeFileName);
                System.IO.File.Delete(deleteFilePath);
                episode.EpisodeFileName = episodefile.FileName;
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/courseFiles", episode.EpisodeFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    episodefile.CopyTo(stream);
                }
            }

            _context.CourseEpisodes.Update(episode);
            _context.SaveChanges();
        }








        #endregion

        #region Comment
        public void AddComment(CourseComment courseComment)
        {
           _context.CourseComments.Add(courseComment);
            _context.SaveChanges();
        }

        public Tuple<List<CourseComment>, int> GetAllComments(int courseId, int pageId = 1)
        {
            int take = 5;
            int skip = (pageId - 1) * take;
            int pageCount = _context.CourseComments.Where(c => !c.IsDelete && c.CourseId == courseId).Count() / take;

            if ((pageCount % 2) != 0)
            {
                pageCount+=1;
            }

            return Tuple.Create(
                _context.CourseComments.Include(c => c.User).Where(c => !c.IsDelete && c.CourseId == courseId).Skip(skip).Take(take)
                    .OrderByDescending(c => c.CreateDate).ToList(), pageCount);
        }

 


        #endregion

    }
}
