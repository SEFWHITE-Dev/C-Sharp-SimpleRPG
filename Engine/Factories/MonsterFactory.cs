using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Factories
{
    public static class MonsterFactory
    {
        public static Monster GetMonster(int monsterID)
        {
            switch (monsterID)
            {
                case 1:
                    //string name, string imageName, int maxHealth, int health, int rewardExp, int rewardGold
                    Monster chicken = new Monster("Chicken", "Chicken.png", 4, 4, 5, 1);

                    chicken.CurrentWeapon = ItemFactory.CreateGameItem(1501);

                    AddLootItem(chicken, 9001, 25);
                    AddLootItem(chicken, 9002, 25);

                    return chicken;

                case 2:
                    Monster skeleton = new Monster("Skeleton", "Skele.png", 4, 4, 5, 1);

                    skeleton.CurrentWeapon = ItemFactory.CreateGameItem(1502);

                    AddLootItem(skeleton, 9003, 25);
                    AddLootItem(skeleton, 9004, 25);

                    return skeleton;

                case 3:
                    Monster slime = new Monster("Slime", "Slime.png", 4, 4, 5, 1);

                    slime.CurrentWeapon = ItemFactory.CreateGameItem(1503);

                    AddLootItem(slime, 9005, 25);
                    AddLootItem(slime, 9006, 25);

                    return slime;

                default:
                    throw new ArgumentException(string.Format("MonsterType '{0}' does not exist", monsterID));
            }
        }

        private static void AddLootItem(Monster monster, int itemID, int percentage)
        {
            if (SimpleRandomNumberGenerator.SimpleNumberBetween(1, 100) <= percentage) 
            { 
                monster.AddItemToInventory(ItemFactory.CreateGameItem(itemID));
            }
        }
    }
}
