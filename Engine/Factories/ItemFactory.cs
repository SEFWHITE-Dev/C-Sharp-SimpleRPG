using Engine.Models;
using Engine.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Factories
{
    public static class ItemFactory
    {
        
        // static collection variable to hold all the items in the world
        // 'readonly' attributed variable can only be set equal to something where it is declared, like below, or inside a constructor
        private static readonly List<GameItem> _standardGameItems = new List<GameItem>();

        // static classes aren't instantiated, hence don't have a constructor
        // instead, when they are first called, it is possible to run the following function
        static ItemFactory()
        {
            // Player weapoons
            BuildWeaponItem(1001, "Stick", 1, 1, 2);
            BuildWeaponItem(1002, "Broken Sword", 5, 1, 4);

            // Monster weapons
            BuildWeaponItem(1501, "Chicekn Talons", 0, 0, 2);
            BuildWeaponItem(1502, "Skele  Bones", 0, 0, 2);
            BuildWeaponItem(1503, "Slime Goop", 0, 0, 3);

            // Healing Item
            BuildHealingItem(2001, "Health Potion", 5, 2);

            // Ingredient Items
            BuildMiscItem(3001, "Tuna Oil", 2);
            BuildMiscItem(3002, "Honey", 2);
            BuildMiscItem(3003, "Herb", 2);

            // Monster drops
            BuildMiscItem(9001, "Chicekn Talons", 1);
            BuildMiscItem(9002, "Chicekn Feathers", 2);
            BuildMiscItem(9003, "Skele  Bones", 1);
            BuildMiscItem(9004, "Skele  Soul", 2);
            BuildMiscItem(9005, "Slime Goop", 1);
            BuildMiscItem(9006, "Slime Acid", 2);

        }

        // function to get objects from the list
        public static GameItem CreateGameItem(int itemTypeID)
        {
            // on the '_standardGameItems' variable, use LINQ (query language unique to List)
            // get the first item that has the type ItemTypeID property value that matches the itemTypeID passed into the function
            // FirstOrDefault - if it can't find an item that matches the condition, it returns the default value, null
            return _standardGameItems.FirstOrDefault(item => item.ItemTypeID == itemTypeID)?.Clone();
        }

        private static void BuildHealingItem(int id, string name, int price, int amount)
        {
            GameItem item = new GameItem(GameItem.ItemCategory.Consumable, id, name, price);
            item.Action = new Heal(item, amount);
            _standardGameItems.Add(item);
        }

        private static void BuildMiscItem(int id, string name, int price)
        {
            _standardGameItems.Add(new GameItem(GameItem.ItemCategory.Miscellaneous, id, name, price));
        }

        private static void BuildWeaponItem(int id, string name, int price, int minDmg, int maxDmg)
        {
            GameItem weapon = new GameItem(GameItem.ItemCategory.Weapon, id, name, price);

            weapon.Action = new AttackWithWeapon(weapon, minDmg, maxDmg);

            _standardGameItems.Add(weapon);
        }

        public static string ItemName(int itemTypeID)
        {
            // '?' to handle null situations or non-game items, if everything left of the '?' results in null, then don't try to get the Name
            // '??' if it does reutrn a null, then return "" (an empty string)
            return _standardGameItems.FirstOrDefault(i => i.ItemTypeID == itemTypeID)?.Name ?? "";
        }
    }
}
