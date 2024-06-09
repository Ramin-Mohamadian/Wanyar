using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using Wanyar.Core.Services;
using Wanyar.Core.Services.Interfaces;
using Wanyar.DataLayer.Entities.Course;
using Wanyar.DataLayer.Entities.Order;

namespace Wanyar_Web.Controllers
{
    public class CourseController : Controller
    {
        private ICourseSevice _courseSevice;
        private IOrderService _orderService;
        private IUserService _userService;
        public CourseController(ICourseSevice courseSevice, IOrderService orderService, IUserService userService)
        {
            _courseSevice = courseSevice;
            _orderService=orderService;
            _userService=userService;

        }


        public IActionResult Index(int pageId = 1, string search = "", string GetType = "all", string orderByType = "date", int startPrice = 0, int endPrice = 0, List<int> selectedGroups = null)
        {
            ViewBag.gettype=GetType;
            ViewBag.Groups=_courseSevice.GetAllGroup();
            ViewBag.pageId=pageId;
            ViewBag.selectedGroup=selectedGroups;
            return View(_courseSevice.GetlistItemCourse(pageId, search, GetType, orderByType, startPrice, endPrice, selectedGroups));

        }


        [Route("ShowDetailCourse/{id}")]
        public IActionResult ShowDetailCourse(int id)
        {
            var course = _courseSevice.GetDetailsCourse(id);
            if (course==null)
            {
                return NotFound();
            }
            return View(course);
        }



        [Authorize]
        [Route("BuyCourse/{id}")]
        public IActionResult BuyCourse(int id)
        {
           
           
              int orderid= _orderService.AddOrder(User.Identity.Name, id);

          

            return Redirect("/UserPanel/MyOrder/ShowOrder/"+ orderid);
        }



        [Route("DownloadFile/{episodeId}")]
        public IActionResult DownloadFile(int episodeId)
        {
            var episode = _courseSevice.GetEpisodeById(episodeId);
            string filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/CourseEpisode",
                episode.EpisodeFileName);
            string fileName = episode.EpisodeFileName;
            if (episode.IsFree&&User.Identity.IsAuthenticated)
            {
                byte[] file = System.IO.File.ReadAllBytes(filepath);
                return File(file, "application/force-download", fileName);
            }

            if (User.Identity.IsAuthenticated)
            {
                if (_orderService.IsUserInCourse(User.Identity.Name, episode.CourseId))
                {
                    byte[] file = System.IO.File.ReadAllBytes(filepath);
                    return File(file, "application/force-download", fileName);
                }
            }

            return Forbid();

        }


        [HttpPost]
        public IActionResult CreateComment(CourseComment courseComment)
        {
            courseComment.IsDelete=false;
            courseComment.CreateDate=DateTime.Now;
            courseComment.UserId=_userService.GetUserIdByUserName(User.Identity.Name);
            _courseSevice.AddComment(courseComment);
            return View("ShowComment",_courseSevice.GetAllComments(courseComment.CourseId));
        }


        public IActionResult ShowComment(int id,int pageId=1)
        {
            return View(_courseSevice.GetAllComments(id,pageId));
        }


    }
}
