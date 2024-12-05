using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    public class Quest
    {
        public int ID { get; }
        public string Name { get; }
        public string Description { get; }

        // items required to complete a quest
        public List<ItemQuantity> ItemsToComplete { get; }

        public int RewardExp { get; }
        public int RewardGold { get; }
        public List<ItemQuantity> RewardItems { get; }

        public Quest(int id, string name, string description, List<ItemQuantity> itemsToComplete,
            int rewardExp, int rewardGold, List<ItemQuantity> rewardItems) 
        {
            ID = id;
            Name = name;
            Description = description;
            ItemsToComplete = itemsToComplete;
            RewardExp = rewardExp;
            RewardGold = rewardGold;
            RewardItems = rewardItems;

        }
    }
}
