using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.Owin.BuilderProperties;
using PotatoCatalog.Models;
using Microsoft.AspNet.Identity;
using PotatoCatalog.Enums;

namespace PotatoCatalog.Services
{
    public class OrderServices
    {
        public bool TryProcessOrder(Cart cart, AddressViewModel address)
        {
            using (var db = new ApplicationDbContext())
            {
                var userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.GetUserId());
                var user = db.Users.FirstOrDefault(x => x.Id == userId);
                if (user.Potatoes < cart.ComputeTotalValue())
                {
                    return false;
                }
                else
                {
                    user.Potatoes -= cart.ComputeTotalValue();
                    var order = new Order
                    {
                        Address = address.Address,
                        ///////////////////////
                        PriceInPotatoes = cart.ComputeTotalValue(),
                        UserId = userId,
                        OrderStatus = OrderStatus.Pending 
                    };
                    db.Orders.Add(order);
                    db.SaveChanges();
                    int orderId = order.Id;
                    foreach (var line in cart.Lines)
                    {
                        var orderItem = new OrderItem
                        {
                            GameEditionId = line.GameEdition.Id,
                            OrderId = orderId,
                            Quantity = line.Quantity,
                            PriceInPotatoes = line.GameEdition.PriceInPotatoes*line.Quantity
                        };
                        db.OrderItems.Add(orderItem);
                    }
                    db.SaveChanges();
                    return true;
                }
            }
        }

        public List<OrderViewModel> GetOrderViewModelList()
        {
            var result = new List<OrderViewModel>();
            using (var db = new ApplicationDbContext())
            {
                result = db.Orders.Select(x => new OrderViewModel
                {
                    Id = x.Id,
                    Address = x.Address,
                    PriceInPotatoes = x.PriceInPotatoes,
                    OrderStatus = x.OrderStatus,
                    UserEmail = x.User.Name
                }).ToList();
            }
            return result;
        }

        public List<OrderViewModel> GetOrderViewModelListForUser(int id)
        {
            var result = new List<OrderViewModel>();
            using (var db = new ApplicationDbContext())
            {
                result = db.Orders.Where(x => x.UserId == id).Select(x => new OrderViewModel
                {
                    Id = x.Id,
                    Address = x.Address,
                    PriceInPotatoes = x.PriceInPotatoes,
                    OrderStatus = x.OrderStatus,
                }).ToList();
            }
            return result;
        }
        public List<OrderViewModel> GetPendingOrderViewModelList()
        {
            var result = new List<OrderViewModel>();
            using (var db = new ApplicationDbContext())
            {
                result = db.Orders.Where(x=>x.OrderStatus== OrderStatus.Pending).Include(t=>t.User).Select(x => new OrderViewModel
                {
                    Id = x.Id,
                    Address = x.Address,
                    PriceInPotatoes = x.PriceInPotatoes,
                    OrderStatus = x.OrderStatus,
                    UserEmail = x.User.Email
                }).ToList();
            }
            return result;
        }

        public List<OrderViewModel> GetNotPendingOrderViewModelList()
        {
            var result = new List<OrderViewModel>();
            using (var db = new ApplicationDbContext())
            {
                result =
                    db.Orders.Where(x => x.OrderStatus != OrderStatus.Pending)
                        .Include(u => u.User)
                        .Select(x => new OrderViewModel
                        {
                            Id = x.Id,
                            Address = x.Address,
                            OrderStatus = x.OrderStatus,
                            PriceInPotatoes = x.PriceInPotatoes,
                            UserEmail = x.User.Email
                        }).ToList();
            }
            return result;
        } 
        public void ChangePendingOrderStatus(int Id, OrderStatus orderStatus)
        {
            if (orderStatus== OrderStatus.Pending)
                return;
            using (var db= new ApplicationDbContext())
            {
                var order = db.Orders.Include(t=>t.User).ToList().FirstOrDefault(x => x.Id == Id);
                if (order.OrderStatus != OrderStatus.Pending)
                {
                    return;
                }
                order.OrderStatus = orderStatus;
                if (orderStatus == OrderStatus.Rejected)
                {
                    var user = db.Users.FirstOrDefault(x => x.Id == order.UserId);
                    user.Potatoes += order.PriceInPotatoes;
                }
                db.SaveChanges();
            }
        }

        
            public void DeleteOrder(int orderID)
            {
                using (var db = new ApplicationDbContext())
                {
                    var gameOrder = db.Orders.FirstOrDefault(x => x.Id == orderID);
                    db.Orders.Remove(gameOrder);
                    db.SaveChanges();
                }
            }

    }
}