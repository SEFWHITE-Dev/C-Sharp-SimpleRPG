using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Actions
{
    public class Heal : BaseAction, IAction
    {

        private readonly int _healAmount;

        public Heal(GameItem itemInUse, int healAmount)
            :base(itemInUse)
        {
            if(itemInUse.Category != GameItem.ItemCategory.Consumable)
            {
                throw new ArgumentException($"{itemInUse.Name} is not a consumable");
            }

            _healAmount = healAmount;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            string actorName = (actor is Player) ? "You" : $"The {actor.Name}";
            string targetName = (target is Player) ? "yourself" : $"the {target.Name}";

            ReportResult($"{actorName} heal {targetName} for {_healAmount}");
            target.Heal(_healAmount);
        }

    }
}
