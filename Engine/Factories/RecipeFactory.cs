using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Factories
{
    public static class RecipeFactory
    {
        private static readonly List<Recipe> _recipes = new List<Recipe>();

        static RecipeFactory() 
        {
            Recipe healthPotion = new Recipe(1, "Health Potion");
            
            healthPotion.AddIngredient(3001, 1);
            healthPotion.AddIngredient(3002, 1);
            healthPotion.AddIngredient(3003, 1);

            healthPotion.AddOutputItems(2001, 1);

            _recipes.Add(healthPotion);
        }

        public static Recipe RecipeByID(int id)
        {
            return _recipes.FirstOrDefault(x => x.ID == id);
        }
    }
}
