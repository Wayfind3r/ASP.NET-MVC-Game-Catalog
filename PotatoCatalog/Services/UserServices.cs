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
        //Returns TableViewModels for all Users
        public List<UserTableViewModel> GetUserTableViewModels()
        {
            List<UserTableViewModel> result;
            using (var db = new ApplicationDbContext())
            {
                var iQueryableUsers = from user in db.Users
                                      select new
                                      {
                                          Id = user.Id,
                                          Email = user.Email,
                                          Potatoes = user.Potatoes,
                                          NumberOfOrders = user.Orders.Count
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
        //Returns TableViewModels for all Users
        //Search by user E-mail (Contains)
        public List<UserTableViewModel> GetUserTableViewModels(string searchString)
        {
            List<UserTableViewModel> result;
            string seachStringTolower = searchString.ToLowerInvariant();
            using (var db = new ApplicationDbContext())
            {
                
                var iQueryableUsers = from user in db.Users
                    where user.Email.Contains(seachStringTolower)
                    select new
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Potatoes = user.Potatoes,
                        NumberOfOrders = user.Orders.Count
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

        //Modifies a Users potatoes by adding the decimal value
        public bool ModifyPotatoes(int Id, decimal value, out decimal currentPotatoes)
        {
            bool result = false;
            currentPotatoes = 0;
            using (var db = new ApplicationDbContext())
            {
                var userExists = db.Users.Any(x => x.Id == Id);
                if (userExists)
                {
                    result = true;
                    var user = db.Users.FirstOrDefault(u => u.Id == Id);
                    user.Potatoes += value;
                    currentPotatoes = user.Potatoes;
                    db.SaveChanges();
                }
            }
            return result;
        }
    }
}