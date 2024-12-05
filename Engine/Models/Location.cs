using Engine.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public  class Location
    {
        public int XCoordinate {  get; }
        public int YCoordinate { get; }
        public string Name { get; }
        public string Description { get; }
        public string ImageName { get; }

        public List<Quest> QuestsAvailableHere { get; } = new List<Quest>();

        public List<MonsterEncounter> MonsterEncounterHere { get; } = new List<MonsterEncounter>();

        public Trader TraderHere { get; set; }

        public Location (int xCoordinate, int yCoordinate, string name, string desc, string imgName)
        {
            XCoordinate = xCoordinate;
            YCoordinate = yCoordinate;
            Name = name;
            Description = desc;
            ImageName = imgName;
        }

        public void AddMonster(int monsterID, int chanceOfEncountering)
        {
            // Checks to see if there's already a preexisting montser with the same ID
            // If there is, overwrite the ChanceOfEncountering wiwth the new number
            if (MonsterEncounterHere.Exists(m => m.MonsterID == monsterID))
            {
                MonsterEncounterHere.First(m => m.MonsterID == monsterID).ChanceOfEncountering = chanceOfEncountering;
            }
            else
            {
                // the monster encounter is not registered in this location, so add it
                MonsterEncounterHere.Add(new MonsterEncounter(monsterID, chanceOfEncountering));
            }
        }

        public Monster GetMonster()
        {
            // if the list is empty return null
            if (!MonsterEncounterHere.Any())
            {
                return null;
            }

            // total the percentatges of all the monsters at this location (including duplicates of the same monster)
            int totalChances = MonsterEncounterHere.Sum(m => m.ChanceOfEncountering);

            // select a random number between 1 and total (total is not 100)
            int randNum = SimpleRandomNumberGenerator.SimpleNumberBetween(1, totalChances);

            int runningTotal = 0;

            // 
            foreach(MonsterEncounter enemy in MonsterEncounterHere)
            {
                runningTotal += enemy.ChanceOfEncountering;

                if(randNum <= runningTotal)
                {
                    return MonsterFactory.GetMonster(enemy.MonsterID);
                }
            }

            //
            return MonsterFactory.GetMonster(MonsterEncounterHere.Last().MonsterID);
        }
    }
}
