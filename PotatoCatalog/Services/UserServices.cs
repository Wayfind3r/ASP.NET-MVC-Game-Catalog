using System;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PotatoCatalog.Models;

namespace PotatoCatalog.Services
{
    public class UserServices
    {
        //Get current potatoes to be displayed on the layout
        public Potatoes GetCurrentPotatoes()
        {
            var userId = Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.GetUserId());
            decimal potatoes = 0;
            using (var db = new ApplicationDbContext())
            {
                potatoes = db.Users.FirstOrDefault(x => x.Id == userId).Potatoes;
            }
            var result = new Potatoes {Amount = potatoes};
            return result;
        }

        public List<UserTableViewModel> GetUserTableViewModels()
        {
            List<UserTableViewModel> result;
            using (var db = new ApplicationDbContext())
            {
                var iQueryableUsers = from u in db.Users
                    join ord in db.Orders on u.Id equals ord.UserId
                    group new {u, ord} by new {u.Id, u.Email, u.Potatoes}
                    into t
                    select new
                    {
                        Id = t.Key.Id,
                       Email = t.Key.Email,
                       Potatoes = t.Key.Potatoes,
                       NumberOfOrders = t.Count()
                    };
                //In order to avoid System.NotSupportedException: 
                //The entity or complex type 'PotatoCatalog.Models.UserTableViewModel' cannot be constructed in a LINQ to Entities query.
                result = iQueryableUsers.AsEnumerable().Select(x=> new UserTableViewModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    Potatoes = x.Potatoes,
                    NumberOfOrders = x.NumberOfOrders
                }).ToList();
                result = result.OrderByDescending(x => x.Email).ToList();
            }
            return result;
        }

        public List<UserTableViewModel> GetUserTableViewModels(string searchString)
        {
            List<UserTableViewModel> result;
            string seachStringTolower = searchString.ToLowerInvariant();
            using (var db = new ApplicationDbContext())
            {
                var iQueryableUsers = from user in db.Users
                    where user.Email.Contains(seachStringTolower)
                    join order in db.Orders
                        on user.Id equals order.UserId
                    group new {user, order}
                        by new {user.Id, user.Email, user.Potatoes}
                    into g
                    select new
                    {
                        Id = g.Key.Id,
                        Email = g.Key.Email,
                        Potatoes = g.Key.Potatoes,
                        NumberOfOrders = g.Count()
                    };
                //In order to avoid System.NotSupportedException: 
                //The entity or complex type 'PotatoCatalog.Models.UserTableViewModel' cannot be constructed in a LINQ to Entities query.
                result = iQueryableUsers.AsEnumerable().Select(x => new UserTableViewModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    Potatoes = x.Potatoes,
                    NumberOfOrders = x.NumberOfOrders
                }).ToList();
                result = result.OrderByDescending(x => x.Email).ToList();
            }
            return result;
        } 
    }
}