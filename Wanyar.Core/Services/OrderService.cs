using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wanyar.Core.DTOs;
using Wanyar.Core.Services.Interfaces;
using Wanyar.DataLayer.Context;
using Wanyar.DataLayer.Entities.Course;
using Wanyar.DataLayer.Entities.Order;
using Wanyar.DataLayer.Entities.Users;
using Wanyar.DataLayer.Entities.Wallet;

namespace Wanyar.Core.Services
{
    public class OrderService : IOrderService
    {
        private WanyarContext _context;
        private IUserService _userService;
        public OrderService(WanyarContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public void AddDiscount(Discount discount)
        {
            _context.Discounts.Add(discount);
            _context.SaveChanges();
        }

        public int AddOrder(string userName, int courseId)
        {
            int userId = _userService.GetUserIdByUserName(userName);

            Order order = _context.Orders.FirstOrDefault(o => o.UserId==userId && !o.IsFinaly);

            var course = _context.Courses.Find(courseId);

            if (order==null)
            {
                order = new Order()
                {
                    UserId= userId,
                    IsFinaly= false,
                    CreateDate= DateTime.Now,
                    OrderSum=course.CoursePrice,
                    OrderDetails=new List<OrderDetail>()
                    {
                        new OrderDetail()
                        {
                            CourseId=courseId,
                            Count=1,
                            Price=course.CoursePrice,
                        }
                    }
                };
                _context.Orders.Add(order);
                _context.SaveChanges();
            }
            else
            {
                OrderDetail detail = _context.OrderDetails.FirstOrDefault(d => d.OrderId==order.OrderId&&d.CourseId==courseId);

                if (detail!=null)
                {
                    detail.Count+=1;
                    _context.OrderDetails.Update(detail);
                }
                else
                {
                    detail = new OrderDetail()
                    {
                        OrderId=order.OrderId,
                        CourseId=courseId,
                        Count=1,
                        Price=course.CoursePrice,
                    };
                    _context.OrderDetails.Add(detail);


                }
                _context.SaveChanges();
                UpdatePriceOrder(order.OrderId);
            }


            return order.OrderId;


        }


        public bool FinalyOrder(string userName, int orderId)
        {
            int userId = _userService.GetUserIdByUserName(userName);
            var order = _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Course)
                .FirstOrDefault(o => o.OrderId == orderId && o.UserId == userId);

            if (order == null || order.IsFinaly)
            {
                return false;
            }

            if (_userService.TotalWallet(userName) >= order.OrderSum)
            {
                order.IsFinaly = true;
                _userService.AddWallet(new Wallet()
                {
                    Amount = order.OrderSum,
                    CreateDate = DateTime.Now,
                    IsPay = true,
                    Description = "فاکتور شما #" + order.OrderId,
                    UserId = userId,
                    TypeId = 2
                });
                _context.Orders.Update(order);

                foreach (var detail in order.OrderDetails)
                {
                    _context.UserCourses.Add(new UserCourse()
                    {
                        CourseId = detail.CourseId,
                        UserId = userId
                    });
                }

                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public List<Discount> GetAllDiscount()
        {
            return _context.Discounts.ToList();
        }

        public Discount GetDiscountById(int discountId)
        {
           return _context.Discounts.Find(discountId);
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders.Find(orderId);
        }

        public Order GetOrderForUserPanel(string userName, int orderId)
        {
            int userid = _userService.GetUserIdByUserName(userName);

            return _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Course)
                .FirstOrDefault(o => o.UserId==userid&&o.OrderId==orderId);
        }

        public List<Order> GetUserOrders(string userName)
        {
            int userId = _userService.GetUserIdByUserName(userName);
            return _context.Orders.Where(u => u.UserId==userId).ToList();
        }

        public bool IsExistCode(string code)
        {
            return _context.Discounts.Any(d=>d.DisCountCode==code);
        }

        public bool IsUserInCourse(string userName, int courseId)
        {
            var userid=_userService.GetUserIdByUserName(userName);

            return _context.UserCourses.Any(uc=>uc.UserId==userid&&uc.CourseId==courseId);
        }

        public void UpdateDiscount(Discount discount)
        {
            _context.Discounts.Update(discount);
            _context.SaveChanges();
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public void UpdatePriceOrder(int orderId)
        {
            var order = _context.Orders.Find(orderId);
            order.OrderSum = _context.OrderDetails.Where(d => d.OrderId == orderId).Sum(d => d.Price);
            _context.Orders.Update(order);
            _context.SaveChanges();


        }

        public DiscountUseType UseDiscount(int orderId, string code)
        {
            var discount = _context.Discounts.SingleOrDefault(c => c.DisCountCode==code);

            if (discount == null)
            {
                return DiscountUseType.NotFound;
            }

            if (discount != null&&discount.StartDate>DateTime.Now)
            {
                return DiscountUseType.ExpireDate;
            }

            if (discount != null && discount.EndDate<=DateTime.Now)
            {
                return DiscountUseType.ExpireDate;
            }

            if (discount!= null && discount.UseableCount<1)
            {
                return DiscountUseType.Finished;
            }

            


            var order = GetOrderById(orderId);

            if(_context.userDiscountCodes.Any(u=>u.UserId==order.UserId&&u.DiscountId==discount.DiscountId))
            {
                return DiscountUseType.UserUsed;
            }

            int percent = (order.OrderSum * discount.DiscountPercent) / 100;
            order.OrderSum = order.OrderSum - percent;

            UpdateOrder(order);
            if(discount != null)
            {
                discount.UseableCount -=1;
            }
            _context.Discounts.Update(discount);
            _context.userDiscountCodes.Add(new UserDiscountCode()
            {
                UserId= order.UserId,
                DiscountId=discount.DiscountId,
            });
            _context.SaveChanges();
            return DiscountUseType.Success;
        }
    }
}
