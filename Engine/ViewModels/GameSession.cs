using Engine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Factories;
using System.ComponentModel;
using Engine.EventArgs;

namespace Engine.ViewModels
{
    public class GameSession : BaseNotificationClass
    {
        // OnMessageRaised will hold a reference to a function in the view that should be run whenever this message is raised
        // our View is going to have a function to handle messages, in the View Model, when you raise this event you should run this function from the view object
        public event EventHandler<GameMessageEventArgs> OnMessageRaised;

        #region Properties

        private Player _currentPlayer;
        private Location _currentLocation;
        private Monster _currentmonster;
        private Trader _currentTrader;

        public World CurrentWorld {  get; }

        public Player CurrentPlayer 
        { 
            get { return _currentPlayer; } 
            set 
            {
                // when an object subscriber to anotehr objects event, you also need to handle unsubscribing to it, as it helps the program get rid of old objects in memory
                // a common pattern used for garbage collection
                if(_currentPlayer != null)
                {
                    _currentPlayer.OnActionPerformed -= OnCurrentPlayerPerformedAction;
                    // 'object.event -= method' means to unsubscribe to the event
                    _currentPlayer.OnLeveledUp -= OnCurrentPlayerLeveledUp;
                    // once the player is killed, we don't want them to be able to call the event again
                    _currentPlayer.OnKilled -= OnCurrentPlayerKilled;
                }

                _currentPlayer = value;

                if (_currentPlayer != null)
                {
                    _currentPlayer.OnActionPerformed += OnCurrentPlayerPerformedAction;
                    _currentPlayer.OnLeveledUp += OnCurrentPlayerLeveledUp;
                    _currentPlayer.OnKilled += OnCurrentPlayerKilled;
                }
            } 
        }


        // when the current location changes the new location is set
        // CurrntLocation is raised to update the visual information
        // HasLocationFoo is raised to check whether the new location has null locations
        // if so then hide the visual direction buttons
        public Location CurrentLocation 
        { 
            get { return _currentLocation; }
            set { 
                _currentLocation = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasLocationToNorth));
                OnPropertyChanged(nameof(HasLocationToWest));
                OnPropertyChanged(nameof(HasLocationToEast));
                OnPropertyChanged(nameof(HasLocationToSouth));

                CompleteQuestsAtLocation();
                GivePlayerQuestsAtLocation();
                GetMonsterAtLocation();

                CurrentTrader = CurrentLocation.TraderHere;
            } 
        }

        public Monster CurrentMonster
        {
            get { return _currentmonster; }
            set { 

                if(_currentmonster != null)
                {
                    _currentmonster.OnActionPerformed -= OnCurrentMonsterPerformedAction;
                    _currentmonster.OnKilled -= OnCurrentMonsterKilled;
                }

                _currentmonster = value;
                
                if (_currentmonster != null)
                {
                    _currentmonster.OnActionPerformed += OnCurrentMonsterPerformedAction;
                    _currentmonster.OnKilled += OnCurrentMonsterKilled;
                    RaiseMessage("");
                    RaiseMessage($"You encounter a {CurrentMonster.Name}!");
                }

                OnPropertyChanged();
                OnPropertyChanged(nameof(HasMonster));
            }
        }

        public Trader CurrentTrader
        {
            get { return _currentTrader; }
            set
            {
                _currentTrader = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasTrader));
            }
        }


        // boolean to check if a coordinate exists within the world
        // returns true if there is a location, returns false if null
        //public bool HasLocationToNorth { get { return CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null; } }

        // '=>' an expression body, returns the results of the conditional. The equivalent of using the 'get' method as above
        // lambda function to return a computed/calculated value
        public bool HasLocationToNorth => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1) != null;
        public bool HasLocationToWest => CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate) != null;
        public bool HasLocationToEast => CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate) != null;
        public bool HasLocationToSouth => CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1) != null;

        public bool HasMonster => CurrentMonster != null;

        public bool HasTrader => CurrentTrader != null;

        #endregion


        // init the GameSession 
        public GameSession() {

            // an 'instance' of the Player class
            CurrentPlayer = new Player("Player", "Mage", 0, 10, 10, 10);


            if (!CurrentPlayer.Weapons.Any())
            {
                CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(1001)); // give player a stick
            }

            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(2001)); // give the player a Health Potion

            CurrentPlayer.LearnRecipe(RecipeFactory.RecipeByID(1)); // give the player access to Health Potion recipe

            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(3001));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(3002));
            CurrentPlayer.AddItemToInventory(ItemFactory.CreateGameItem(3003));

            // creates an instance of the WorldFactory, however, since only one instance is necessary for the entire game, 
            // a static class is implemented instead

            //WorldFactory factory = new WorldFactory();
            //CurrentWorld = factory.CreateWorld();

            // you can use a static class without instantiating it.
            // using a static class or function to create objects is known as the Factory Design Pattern
            // Goes into the WorldFactory class, calls the CreateWorld() function
            // initialises the World and sets that value to the CurrentWorld property
            CurrentWorld = WorldFactory.CreateWorld();


            CurrentLocation = CurrentWorld.LocationAt(0,0);

            //CurrentPlayer.Inventory.Add(ItemFactory.CreateGameItem(1001));
            //CurrentPlayer.Inventory.Add(ItemFactory.CreateGameItem(1002));
        }

        public void MoveNorth()
        {
            if (HasLocationToNorth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate + 1);
            }
        }

        public void MoveWest()
        {
            if (HasLocationToWest)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate - 1, CurrentLocation.YCoordinate);
            }
        }

        public void MoveEast()
        {
            if (HasLocationToEast)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate + 1, CurrentLocation.YCoordinate);
            }
        }

        public void MoveSouth()
        {
            if ( HasLocationToSouth)
            {
                CurrentLocation = CurrentWorld.LocationAt(CurrentLocation.XCoordinate, CurrentLocation.YCoordinate - 1);
            }
        }

        private void CompleteQuestsAtLocation()
        {
            foreach (Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                QuestStatus questToComplete = CurrentPlayer.Quests.FirstOrDefault(q => q.PlayerQuest.ID == quest.ID && !q.IsCompleted);

                if (questToComplete != null)
                {
                    if (CurrentPlayer.HasAllTheseItems(quest.ItemsToComplete))
                    {
                        CurrentPlayer.RemoveItemsFromInventory(quest.ItemsToComplete);

                        RaiseMessage("");
                        RaiseMessage($"You completed the '{quest.Name}' quest.");

                        // give the player the quest rewards
                        RaiseMessage($"You received {quest.RewardExp} EXP");
                        CurrentPlayer.AddExpereince(quest.RewardExp);

                        RaiseMessage($"You received {quest.RewardGold} Gold");
                        CurrentPlayer.ReceiveGold( quest.RewardGold);
                        
                        foreach (ItemQuantity items in quest.RewardItems)
                        {
                            GameItem rewardItem = ItemFactory.CreateGameItem(items.ItemID);

                            RaiseMessage($"You received {rewardItem.Name}");
                            CurrentPlayer.AddItemToInventory(rewardItem);
                        }

                        // mark the quest as completed
                        questToComplete.IsCompleted = true;
                    }
                }
            }
        }

        private void GivePlayerQuestsAtLocation()
        {
            foreach(Quest quest in CurrentLocation.QuestsAvailableHere)
            {
                // check if player already has the quest by comparing questIDs
                // 'Any' function lets us compare against all the objects in the quest list property
                if(!CurrentPlayer.Quests.Any(q => q.PlayerQuest.ID == quest.ID))
                {
                    CurrentPlayer.Quests.Add(new QuestStatus(quest));

                    RaiseMessage("");
                    RaiseMessage($"You received the '{quest.Name}' quest.");
                    RaiseMessage(quest.Description);

                    RaiseMessage("Return with: ");
                    foreach (ItemQuantity itemQuantitiy in quest.ItemsToComplete)
                    {
                        RaiseMessage($"  {itemQuantitiy.Quantity} {ItemFactory.CreateGameItem(itemQuantitiy.ItemID).Name}");
                    }

                    RaiseMessage("Reward: ");
                    RaiseMessage($"  {quest.RewardExp} EXP");
                    RaiseMessage($"  {quest.RewardGold} Gold");
                    foreach (ItemQuantity itemQuantity in quest.RewardItems)
                    {
                        RaiseMessage($"  {itemQuantity.Quantity} {ItemFactory.CreateGameItem(itemQuantity.ItemID).Name}");
                    }
                }
            }
        }

        private void GetMonsterAtLocation()
        {
            CurrentMonster = CurrentLocation.GetMonster();
        }

        public void AttackCurrentMonster()
        {
            // a guard clause/ early exit for if the player doesn't have a weapon
            if (CurrentPlayer.CurrentWeapon == null)
            {
                RaiseMessage("Equip a Weapon to Attack.");
                return;
            }

            CurrentPlayer.UseCurrentWeaponOn(CurrentMonster);

            // if mosnter is killed, collect rewards and loot
            if (CurrentMonster.IsDead)
            {
                // get anotehr monster to fight
                GetMonsterAtLocation();
            }
            else
            {
                CurrentMonster.UseCurrentWeaponOn(CurrentPlayer);
            }
        }

        public void UseCurrentConsumable() // helper function so when the funciton is called by the MainWindow.xaml.cs, it doesn't have to call a deeply nested function
        {
            CurrentPlayer.UseCurrentConsumable();
        }

        public void CraftItemUsing(Recipe recipe)
        {
            if (CurrentPlayer.HasAllTheseItems(recipe.Ingredients))
            {
                CurrentPlayer.RemoveItemsFromInventory(recipe.Ingredients);

                foreach (ItemQuantity itemQuantity in recipe.OutputItems)
                {
                    for (int i = 0; i < itemQuantity.Quantity; i++)
                    {
                        GameItem outputItem = ItemFactory.CreateGameItem(itemQuantity.ItemID);
                        CurrentPlayer.AddItemToInventory(outputItem);
                        RaiseMessage($"You craft 1 {outputItem.Name}");
                    }
                }
            }
            else
            {
                RaiseMessage("You do not have the required ingredients: ");
                foreach(ItemQuantity itemQuantity in recipe.Ingredients)
                {
                    RaiseMessage($"{itemQuantity.Quantity} {ItemFactory.ItemName(itemQuantity.ItemID)}");
                }
            }
        }



        private void OnCurrentPlayerPerformedAction(object? sender, string result)
        {
            RaiseMessage(result);
        }

        private void OnCurrentMonsterPerformedAction(object? sender, string result)
        {
            RaiseMessage(result);
        }

        private void OnCurrentPlayerKilled(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage("");
            RaiseMessage("You have been defeated...");

            CurrentLocation = CurrentWorld.LocationAt(0, -1); // player home
            CurrentPlayer.ComepletelyHeal(); // heal the player
        }

        private void OnCurrentMonsterKilled(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage("");
            RaiseMessage($"You defeated the {CurrentMonster.Name}!");

            RaiseMessage($"You received {CurrentMonster.RewardExpereience} EXP");
            CurrentPlayer.AddExpereince(CurrentMonster.RewardExpereience);

            RaiseMessage($"You received {CurrentMonster.Gold} Gold");
            CurrentPlayer.ReceiveGold( CurrentMonster.Gold);

            foreach (GameItem gameItem in CurrentMonster.Inventory)
            {
                RaiseMessage($"You received one {gameItem.Name}");
                CurrentPlayer.AddItemToInventory(gameItem);
            }

        }

        private void OnCurrentPlayerLeveledUp(object sender, System.EventArgs eventArgs)
        {
            RaiseMessage($"You are now Level {CurrentPlayer.Level}!");
        }

        // if there is anything subscribed to the OnMessageRaised, the ViewModel will call this function by 'Invoking' it
        private void RaiseMessage(string message)
        {
            OnMessageRaised?.Invoke(this, new GameMessageEventArgs(message));
        }
    }
}
