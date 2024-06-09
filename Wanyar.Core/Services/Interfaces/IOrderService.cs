using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wanyar.Core.DTOs;
using Wanyar.DataLayer.Entities.Order;

namespace Wanyar.Core.Services.Interfaces
{
    public interface IOrderService
    {
        int AddOrder(string userName, int courseId);

        Order GetOrderForUserPanel(string userName, int orderId);
        Order GetOrderById(int orderId);

        bool FinalyOrder(string userName,int orderId);

        List<Order> GetUserOrders(string userName);

        void UpdateOrder(Order order);

        DiscountUseType UseDiscount(int orderId, string code);

        void AddDiscount(Discount discount);

        List<Discount> GetAllDiscount();

        Discount GetDiscountById(int discountId);

        void UpdateDiscount(Discount discount);

        bool IsExistCode(string code);
        bool IsUserInCourse(string userName , int courseId);
    }
}
