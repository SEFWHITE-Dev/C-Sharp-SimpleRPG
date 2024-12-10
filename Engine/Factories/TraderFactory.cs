using Engine.Models;
using Engine.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Engine.Factories
{
    public static class TraderFactory
    {
        private const string GAME_DATA_FILENAME = ".\\GameData\\Traders.xml";

        private static readonly List<Trader> _traders = new List<Trader>();

        static TraderFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));
                LoadTradersFromNodes(data.SelectNodes("/Traders/Trader"));
            }
            else
            {
                throw new FileNotFoundException($"Missing data file: {GAME_DATA_FILENAME}");
            }
        }

        private static void LoadTradersFromNodes(XmlNodeList nodes)
        {
            foreach (XmlNode node in nodes)
            {
                Trader trader =
                    new Trader(node.AttributeAsInt("ID"),
                               node.SelectSingleNode("./Name")?.InnerText ?? "");
                foreach (XmlNode childNode in node.SelectNodes("./InventoryItems/Item"))
                {
                    int quantity = childNode.AttributeAsInt("Quantity");
                    // Create a new GameItem object for each item we add.
                    // This is to allow for unique items, like swords with enchantments.
                    for (int i = 0; i < quantity; i++)
                    {
                        trader.AddItemToInventory(ItemFactory.CreateGameItem(childNode.AttributeAsInt("ID")));
                    }
                }
                _traders.Add(trader);
            }
        }

        /*
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
        */

        public static Trader GetTraderByID(int id)
        {
            return _traders.FirstOrDefault(t => t.ID == id);
        }
    }
}
