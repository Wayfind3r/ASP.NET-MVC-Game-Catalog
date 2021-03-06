﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using PotatoCatalog.Enums;
using PotatoCatalog.Services;

namespace PotatoCatalog.Models
{
    public class OrderViewModel
    {
         public int Id { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Display(Name = "Price in Potatoes")]
        public decimal PriceInPotatoes { get; set; }
        [Display(Name = "Order Status")]
        public OrderStatus OrderStatus { get; set; }
        [Display(Name = "User E-mail")]
        public string UserEmail { get; set; }
    }

    public class OrderDetailedViedModel
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        [Display(Name = "User E-mail")]
        public string UserEmail { get; private set; }
        [Display(Name = "Address")]
        public string Address { get; private set; }
        public string ReturnUrl { get; private set; }
        private OrderDetailedViedModel() {}

        public OrderDetailedViedModel(int orderId, string returnUrl)
        {
            ReturnUrl = returnUrl;
            using (var db = new ApplicationDbContext())
            {
                var gameEditionServices = new GameEditionServices();
                var result = db.OrderItems.Where(x => x.OrderId == orderId).Select(x=> new
                {
                    x.GameEditionId,
                    x.Quantity
                    
                });
                var EditionIdAndQuantity = result.AsEnumerable() // anything past this is done outside of sql server
                    .Select(item => new KeyValuePair<int, int>(item.GameEditionId, item.Quantity))
                    .ToList();
                foreach (var pair in EditionIdAndQuantity)
                {
                    lineCollection.Add(new CartLine
                    {
                        GameEdition = gameEditionServices.GetGameEditionCartViewModel(pair.Key),
                        Quantity = pair.Value
                    });
                }
                UserEmail = db.Orders.FirstOrDefault(x => x.Id == orderId).User.Email;
                Address = db.Orders.FirstOrDefault(x => x.Id == orderId).Address;
            }
        }

        class GameEditionIdQuantityPair
        {
             
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(g => g.GameEdition.PriceInPotatoes * g.Quantity);
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }
}