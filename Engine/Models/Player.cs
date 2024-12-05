using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    
    public class Player : LivingEntity
    {
        // Property Changed event is raised within the setters, when a new value is set
        // when properties are set to public as 'auto-properties' and their values are automatically changed, 
        // there is no function call to visually update those valuse. 
        // creating a private a 'backing variable' allows for the event handler to be called
        // often refferred to the 'Sub-Pub' method (subscribe, publish)
        #region Properties

        private string _characterClass;
        private int _experience;


        public string CharacterClass {
            get { return _characterClass; }
            set
            {
                _characterClass = value;
                OnPropertyChanged();
            }
        }


        public int Experience { 
            get { return _experience; } 
            private set { // private set for encapsulation
                _experience = value;
                OnPropertyChanged();
                SetLevelAndMaxHealth();
            }
        }

        // observables collections automatically get the UI update notifications
        public ObservableCollection<QuestStatus> Quests { get; }

        public ObservableCollection<Recipe> Recipes { get; }


        #endregion 

        // UI can listen for this event
        public event EventHandler OnLeveledUp;

        public Player(string name, string characterClass, int exp, int maxHp, int currentHp, int gold) :
            base(name, maxHp, currentHp, gold) 
        { 
            CharacterClass = characterClass;
            Experience = exp;
            Quests = new ObservableCollection<QuestStatus>();
            Recipes = new ObservableCollection<Recipe>();
        }

        public void AddExpereince(int exp)
        {
            Experience += exp;
        }

        public void LearnRecipe(Recipe recipe)
        {
            if(!Recipes.Any(r => r.ID == recipe.ID))
            {
                Recipes.Add(recipe);
            }
        }

        public void SetLevelAndMaxHealth() // function is called whenever the players exp changes
        {
            int originalLevel = Level;

            Level = (Experience / 100) + 1;

            if (Level != originalLevel)
            {
                MaxHealth = Level * 10;

                // raise the Onleveledup event
                OnLeveledUp?.Invoke(this, System.EventArgs.Empty);
            }
        }
    }
}
