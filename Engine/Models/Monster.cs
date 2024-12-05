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

        public string ImageName { get; }

        public int RewardExpereience { get; }


        public Monster(string name, string imageName, int maxHealth, int health, int rewardExp, int rewardGold):
            base(name, maxHealth, health, rewardGold)
        {
            ImageName = $"pack://application:,,,/Engine;component/Images/Monsters/{imageName}";
            RewardExpereience = rewardExp;
        }
    }
}
