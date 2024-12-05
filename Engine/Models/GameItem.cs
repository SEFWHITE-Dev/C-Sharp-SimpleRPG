using Engine.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class GameItem
    {
        // Composition Design pattern used - GameItem now includes parameters for Weapon type objects

        public enum ItemCategory
        {
            Miscellaneous,
            Weapon,
            Consumable
        }

        // there should be no need to set another value to these parameters outside the constructor, hence no setter
        public ItemCategory Category { get; }
        public int ItemTypeID { get; }
        public string Name { get; }
        public int Price { get; } 
        public bool IsUnique { get; }
        public IAction Action { get; set; }

        public GameItem(ItemCategory category, int  itemTypeID, string name, int price, bool isUnique = false, IAction action = null)
        {
            Category = category;
            ItemTypeID = itemTypeID;
            Name = name;
            Price = price;
            IsUnique = isUnique;
            Action = action;
        }

        public void PerformAction(LivingEntity actor, LivingEntity target)
        {
            Action?.Execute(actor, target);
        }

        // 'public new' is used when the parent Class has a function of the same name
        // in this case, when Clone() is called on the Weapon object, we want to override the 
        // Clone() function on the GameItem() class
        public GameItem Clone()
        {
            return new GameItem(Category, ItemTypeID, Name, Price, IsUnique, Action);
        }
    }
}
