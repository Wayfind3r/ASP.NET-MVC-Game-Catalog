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
    }
}