using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Models
{
    // with encapsulation you don't allow one class to directly change property values in another class, 
    // you have functions that control changing a properties value.
    // health and gold will be encapsulated

    // 'abstract' class means you can never instantiate that object
    // you can instantiate an object that uses an abstract class as its base class
    public abstract class LivingEntity : BaseNotificationClass
    {
        #region Properties

        private string _name;
        private int _currentHealth;
        private int _maxHealth;
        private int _gold;
        private int _level;
        private GameItem _currentWeapon;
        private GameItem _currentConsumable;

        // making the setters private so their values can only change within other functions inside the LivingEntity class

        public string Name
        {
            get { return _name; }
            private set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public int CurrentHealth
        {
            get
            { return _currentHealth; }
            private set
            {
                _currentHealth = value;
                OnPropertyChanged();
            }
        }

        public int MaxHealth
        {
            get
            { return _maxHealth; }
            protected set // protected setter so the value can be set child class, Player, whenever they gain a new level
            {
                
                _maxHealth = value;
                OnPropertyChanged();
            }
        }

        public int Gold
        {
            get
            { return _gold; }
            private set
            {
                _gold = value;
                OnPropertyChanged();
            }
        }

        public int Level
        {
            get { return _level; }
            protected set // protected setter so the value can be set within the LivingEntity class or any of the child classes
            {
                _level = value;
                OnPropertyChanged();
            }
        }

        public GameItem CurrentWeapon
        {
            get { return _currentWeapon; }
            set
            {
                // whenever the player changes their weapon, unsubscribe the event
                if (_currentWeapon != null)
                {
                    _currentWeapon.Action.OnActionPerformed -= RaiseActionPerformedEvent;
                }

                _currentWeapon = value;

                if (_currentWeapon != null)
                {
                    _currentWeapon.Action.OnActionPerformed += RaiseActionPerformedEvent;
                }

                OnPropertyChanged();
            }
        }

        public GameItem CurrentConsumable
        {
            get { return _currentConsumable; }
            set
            {
                if (_currentConsumable != null)
                {
                    _currentConsumable.Action.OnActionPerformed -= RaiseActionPerformedEvent;
                }

                _currentConsumable = value;

                if (_currentConsumable != null)
                {
                    _currentConsumable.Action.OnActionPerformed += RaiseActionPerformedEvent;
                }

                OnPropertyChanged();
            }
        }

        // ObservableCollection automatically handles all of the notifications (property changes) for the View Model
        public ObservableCollection<GameItem> Inventory { get; }

        public ObservableCollection<GroupedInventoryItem> GroupedInventory { get; }

        // whenever something accesses the Weapons property, return the Inventory items 'Where' the item is a Weapon
        // .ToList(); at the end of the LINQ statement to materialise the value
        // LINQ statements may not 'finalize' and return a result until ToList() or another method is called
        // this is called 'deferred execution', where it waits to execute the LINQ query
        public List<GameItem> Weapons => Inventory.Where(i => i.Category == GameItem.ItemCategory.Weapon).ToList();

        public List<GameItem> Consumables => Inventory.Where(i => i.Category == GameItem.ItemCategory.Consumable).ToList();

        public bool HasConsumable => Consumables.Any();

        // return the result of the computation. if CurrentHealth is <= 0 then returns true
        public bool IsDead => CurrentHealth <= 0;

        #endregion


        // The GameSession class is going to subscribe(listen to) this event.
        // when a LivingEntity (Player, Monster, Trader etc) is killed,
        // LivingEntity will raise this event and notify any subscribers
        public event EventHandler OnKilled;

        // the way the command notifications will work, is the UI will listen for OnActionsPerformed event on the LivingEntity
        // the Player object is going to look for ActionsPerformed by the Weapon, HealingPotion, Food, etc
        // so when the Weapon raises an Action message, the player is going to catch it and use OnActionPerformed
        // and RaiseActionPerformedEvent() to notify the UI, because the UI is subscribed to the Player's OnActionPerformed
        public event EventHandler<string> OnActionPerformed;

        // 'protected' means that this constructor can only be accessed by the child classes
        protected LivingEntity(string name, int maxHp, int currentHp, int gold, int level = 1)
        {
            Name = name;
            MaxHealth = maxHp;
            CurrentHealth = currentHp;
            Gold = gold;
            Level = level;

            Inventory = new ObservableCollection<GameItem>();
            GroupedInventory = new ObservableCollection<GroupedInventoryItem>();
        }


        public void UseCurrentWeaponOn(LivingEntity target)
        {
            CurrentWeapon.PerformAction(this, target);
        }

        public void UseCurrentConsumable()
        {
            CurrentConsumable.PerformAction(this, this);
            RemoveItemFromInventory(CurrentConsumable);
        }

        public void TakeDamage(int hitPointsOfDamage)
        {
            CurrentHealth -= hitPointsOfDamage;

            if (IsDead)
            {
                CurrentHealth = 0;
                RaiseOnKilledEvent();
            }
        }

        public void Heal(int hitPointsToHeal)
        {
            CurrentHealth += hitPointsToHeal;

            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }

        public void ComepletelyHeal()
        {
            CurrentHealth = MaxHealth;
        }

        public void ReceiveGold(int amountOfGold)
        {
            Gold += amountOfGold;
        }

        public void SpendGold(int amountOfGold)
        {
            if(amountOfGold > Gold)
            {
                throw new ArgumentOutOfRangeException($"{Name} only has {Gold}.");
            }
            Gold -= amountOfGold;
        }


        public void AddItemToInventory(GameItem item)
        {
            Inventory.Add(item);

            if (item.IsUnique)
            {
                GroupedInventory.Add(new GroupedInventoryItem(item, 1));
            }
            else
            {
                // if there are any GroupedInventory items inside the GroupedInventory property,
                // where the ItemTypeID matches the ItemTypeID of the item passed into the function
                // if NOT then add in new item, of quantity 0
                if(!GroupedInventory.Any(gi => gi.Item.ItemTypeID == item.ItemTypeID))
                {
                    GroupedInventory.Add(new GroupedInventoryItem(item, 0));
                }

                // find the first item with the matching ItemTypeID and increase its quantity by 1
                GroupedInventory.First(gi => gi.Item.ItemTypeID == item.ItemTypeID).Quantity++;
            }

            // raise the OnPropertyChanged event to update the UI
            OnPropertyChanged(nameof(Weapons));
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumable));
        }

        public void RemoveItemFromInventory(GameItem item)
        {
            Inventory.Remove(item);

            // get the first item from the GroupedInventory property where the item matches the passed in item
            // ternary operator '?' - it evaluates the calculation before the question mark. another way to do a ifelse statement
            // If that calculation is true, it returns the first result (the part before the ':')
            // if false, returns the part after the ':'
            GroupedInventoryItem groupedInventoryItemToRemove = item.IsUnique ?
                GroupedInventory.FirstOrDefault(gi => gi.Item == item) :
                GroupedInventory.FirstOrDefault(gi => gi.Item.ItemTypeID == item.ItemTypeID);

            if (groupedInventoryItemToRemove != null)
            {
                // check if the Quantity of the item in the inventory is 1
                // 'unique' items will always have a quantity of 1
                if (groupedInventoryItemToRemove.Quantity == 1)
                {
                    // completely removes the item from the inventory list
                    GroupedInventory.Remove(groupedInventoryItemToRemove);
                }
                else
                {
                    // reduces the quantity of the item by 1
                    groupedInventoryItemToRemove.Quantity--;
                }
            }

            // in case a weapon was removed from the player's inventory, need to update the UI
            OnPropertyChanged(nameof(Weapons));
            OnPropertyChanged(nameof(Consumables));
            OnPropertyChanged(nameof(HasConsumable));
        }

        public void RemoveItemsFromInventory(List<ItemQuantity> itemQuantities)
        {
            foreach (ItemQuantity itemQuantity in itemQuantities)
            {
                for (int i = 0; i < itemQuantity.Quantity; i++)
                {
                    RemoveItemFromInventory(Inventory.First(item => item.ItemTypeID == itemQuantity.ItemID));
                }
            }
        }

        public bool HasAllTheseItems(List<ItemQuantity> items)
        {
            foreach(ItemQuantity item in items)
            {
                if(Inventory.Count(i => i.ItemTypeID == item.ItemID) < item.Quantity) 
                {
                    return false;
                }
            }
            return true;
        }

        #region Private functions

        private void RaiseOnKilledEvent()
        {
            OnKilled?.Invoke(this, new System.EventArgs());
        }

        private void RaiseActionPerformedEvent(object sender, string result)
        {
            OnActionPerformed?.Invoke(this, result);
        }

        #endregion
    }
}
