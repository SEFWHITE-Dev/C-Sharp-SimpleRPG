using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Factories
{
    public static class TraderFactory
    {
        private static readonly List<Trader> _traders = new List<Trader>();

        static TraderFactory() {
            Trader female = new Trader("Woman");
            female.AddItemToInventory(ItemFactory.CreateGameItem(1001));

            Trader male = new Trader("Man");
            male.AddItemToInventory(ItemFactory.CreateGameItem(1001));

            Trader herbalist = new Trader("Herbalist");
            herbalist.AddItemToInventory(ItemFactory.CreateGameItem(1001));

            AddTraderToList(female);
        }

        public static void AddTraderToList(Trader trader)
        {
            if (_traders.Any(t => t.Name == trader.Name))
            {
                // each trader must have a unique name
                throw new ArgumentException($"There is already a trader named '{trader.Name}'");
            }

            _traders.Add(trader);
        }

        public static Trader GetTraderByName(string name)
        {
            return _traders.FirstOrDefault(t => t.Name == name);
        }
    }
}
