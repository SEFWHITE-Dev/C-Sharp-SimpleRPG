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
    public static class MonsterFactory
    {
        // '\\' is the escape sequence used in C#
        private const string GAME_DATA_FILENAME = ".\\GameData\\Monsters.xml";

        // static collection variable to hold all the monsters
        // 'readonly' attributed variable can only be set equal to something where it is declared, like below, or inside a constructor
        private static readonly List<Monster> _baseMonsters = new List<Monster>();


        static MonsterFactory()
        {
            if (File.Exists(GAME_DATA_FILENAME))
            {
                XmlDocument data = new XmlDocument();
                data.LoadXml(File.ReadAllText(GAME_DATA_FILENAME));

                string rootImagePath = data.SelectSingleNode("/Monsters").AttributeAsString("RootImagePath");

                LoadMonstersFromNodes(data.SelectNodes("/Monsters/Monster"), rootImagePath);
            }
            else
            {
                throw new FileNotFoundException($"Missing data file {GAME_DATA_FILENAME}");
            }
        }

        private static void LoadMonstersFromNodes(XmlNodeList nodes, string rootImagePath)
        {
            if(nodes == null)
            {
                return;
            }

            foreach (XmlNode node in nodes) 
            {
                Monster monster = new Monster(
                    node.AttributeAsInt("ID"),
                    node.AttributeAsString("Name"),
                    $".{rootImagePath}{node.AttributeAsString("ImageName")}",
                    node.AttributeAsInt("MaxHealth"),
                    ItemFactory.CreateGameItem(node.AttributeAsInt("WeaponID")),
                    node.AttributeAsInt("RewardEXP"),
                    node.AttributeAsInt("Gold"));

                XmlNodeList lootItemNodes = node.SelectNodes("./LootItems/LootItem");
                if(lootItemNodes != null)
                {
                    foreach(XmlNode lootItemNode in lootItemNodes)
                    {
                        monster.AddItemToLootTable(lootItemNode.AttributeAsInt("ID"), lootItemNode.AttributeAsInt("Percentage"));
                    }
                }

                _baseMonsters.Add(monster);
            }
        }

        public static Monster GetMonster(int monsterID)
        {
            return _baseMonsters.FirstOrDefault(m => m.ID == monsterID)?.GetNewInstance();
        }

        /* OLD 
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
        */
    }
}
