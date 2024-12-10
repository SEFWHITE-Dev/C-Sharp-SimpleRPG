using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Actions
{
    // A 'Command' class from the Command design pattern
    // you can only inherit from one class, but can implement as many interfaces as you want
    public class AttackWithWeapon : BaseAction, IAction
    {
        
        private readonly int _maxDamage;
        private readonly int _minDamage;


        // the constructor will pass in any requried parameters and store them.
        // the function to 'execute' an action will use the stored parameters values
        public AttackWithWeapon(GameItem itemInUse, int minDamage, int maxDamage)
            : base(itemInUse)
        {
            if (itemInUse.Category != GameItem.ItemCategory.Weapon)
            {
                throw new ArgumentException($"{itemInUse.Name} is not a Weapon!");
            }

            if (minDamage < 0)
            {
                throw new ArgumentException("minDamage must be <= 0");
            }
            if (minDamage > maxDamage)
            {
                throw new ArgumentException("maxDamage must be >= to minDamage");
            }

            _maxDamage = maxDamage;
            _minDamage = minDamage;
        }

        public void Execute(LivingEntity actor, LivingEntity target)
        {
            int damage = SimpleRandomNumberGenerator.SimpleNumberBetween(_minDamage, _maxDamage);

            string actorName = (actor is Player) ? "You" : $"The {actor.Name}";
            string targetName = (target is Player) ? "you" : $"The {target.Name}";

            if (damage == 0)
            {
                ReportResult($"{actorName} missed the {targetName}.");
            }
            else
            {
                ReportResult($"{actorName} hit the {targetName} for {damage} damage!");
                target.TakeDamage(damage);
            }
        }

    }
}
