using System.Linq;
using PotatoCatalog.Models;

namespace PotatoCatalog.Services
{
    public class OrderItemServices
    {
        public void CreateOrderItem(int gameEditionId, int orderId, int quantity, decimal price)
        {
            using (var db = new ApplicationDbContext())
            {
                OrderItem newOrderItem = new OrderItem{ GameEditionId = gameEditionId, OrderId = orderId, Quantity = quantity, PriceInPotatoes = price};
                db.OrderItems.Add(newOrderItem);
                db.SaveChanges();
            }
        }

        public void DeleteGameEdition(int orderItemID)
        {
            using (var db = new ApplicationDbContext())
            {
                var gameOrderItem = db.OrderItems.FirstOrDefault(x => x.Id == orderItemID);
                db.OrderItems.Remove(gameOrderItem);
                db.SaveChanges();
            }
        }
    }
}