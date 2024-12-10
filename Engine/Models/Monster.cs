using Engine.Factories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Monster : LivingEntity
    {
        private readonly List<ItemPercentage> _lootTable = new List<ItemPercentage>();

        public int ID { get; }
        public string ImageName { get; }
        public int RewardExpereience { get; }


        public Monster(int id, string name, string imageName, int maxHealth, GameItem currentWeapon, int rewardExp, int rewardGold):
            base(name, maxHealth, maxHealth, rewardGold)
        {
            ID = id;
            ImageName = imageName;
            //ImageName = $"pack://application:,,,/Engine;component/Images/Monsters/{imageName}";
            CurrentWeapon = currentWeapon;
            RewardExpereience = rewardExp;
        }

        public void AddItemToLootTable(int id, int percentage)
        {
            // when we read the xml file for the first time and need to populate the loot table,
            // with all the possible loot items the monster could have, this function will be called

            // remove the entry from the loot table if it already contains an entry with this ID
            _lootTable.RemoveAll(ip =>  ip.ID == id);

            _lootTable.Add(new ItemPercentage(id, percentage));
        }

        public Monster GetNewInstance()
        {
            // clone the monster, rather than defeating the same previously instanced monster
            Monster newMonster = new Monster(ID, Name, ImageName, MaxHealth, CurrentWeapon, RewardExpereience, Gold);

            foreach (ItemPercentage itemPercentage in _lootTable)
            {
                // clone the loot table
                newMonster.AddItemToLootTable(itemPercentage.ID, itemPercentage.Percentage);

                // populate the new monster's inventory using the loot table
                if (SimpleRandomNumberGenerator.SimpleNumberBetween(1, 100) <= itemPercentage.Percentage)
                {
                    newMonster.AddItemToInventory(ItemFactory.CreateGameItem(itemPercentage.ID));
                }
            }

            return newMonster;
        }
    }
}
