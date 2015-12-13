using System.Collections.Generic;
using System.Linq;

namespace PotatoCatalog.Models
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public void AddItem(GameEditionCartViewModel gameEdition, int quantity)
        {
            CartLine line = lineCollection.FirstOrDefault(g => g.GameEdition.Id == gameEdition.Id);

            if (line == null)
            {
                lineCollection.Add(new CartLine {GameEdition = gameEdition, Quantity = quantity});
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(GameEditionCartViewModel gameEdition)
        {
            lineCollection.RemoveAll(l => l.GameEdition.Id == gameEdition.Id);
        }

        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(g=>g.GameEdition.PriceInPotatoes*g.Quantity);
        }

        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        public GameEditionCartViewModel GameEdition { get; set; }
        public int Quantity { get; set; }
    }
}