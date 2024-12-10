using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Engine.EventArgs;
using Engine.Models;
using Engine.ViewModels;
using System.Collections.Generic;
using System;


namespace WPF_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly GameSession _gameSession = new GameSession();

        // Dictionary is a special type of collection. Each item within a Dictionary consists of a Key and a Value
        // Key is what key was pressed on the keybaord, the Value is the function to run, e.g. MoveNorth()
        // You cannot have two values with the same keys - each key must be unique
        private readonly Dictionary<Key, Action> _userInputActions = new Dictionary<Key, Action>();

        public MainWindow()
        {
            InitializeComponent();

            InitializeUserInputActions();

            // subscribe to the OnMessageRaised event
            _gameSession.OnMessageRaised += OnGameMessageRaised;

            // DataContext is a built in property used by xaml
            DataContext = _gameSession;
        }



        private void OnClick_MoveNorth(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveNorth();
        }
        private void OnClick_MoveWest(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveWest();
        }
        private void OnClick_MoveEast(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveEast();
        }
        private void OnClick_MoveSouth(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveSouth();
        }

        private void OnClick_AttackMonster(object? sender, RoutedEventArgs e)
        {
            _gameSession.AttackCurrentMonster();
        }

        private void OnClick_UseCurrentConsumable(object? sender, RoutedEventArgs e)
        {
            _gameSession.UseCurrentConsumable();
        }

        private void OnGameMessageRaised(object? sender, GameMessageEventArgs e)
        {
            // pass in the message text into the GameMessage RichTextBox xaml component
            GameMessage.Document.Blocks.Add(new Paragraph(new Run(e.Message)));
            GameMessage.ScrollToEnd();
        }

        private void OnClick_DisplayTradeScreen(object? sender, RoutedEventArgs e)
        {
            // guard clause to check if the player is at a location where there is a Trader
            if (_gameSession.CurrentTrader != null)
            {
                TradeScreen tradeScreen = new TradeScreen();
                tradeScreen.Owner = this;
                tradeScreen.DataContext = _gameSession;
                tradeScreen.ShowDialog(); // ShowDialog locks the user into the current screen
            }
        }

        private void OnClick_Craft(object? sender, RoutedEventArgs e)
        {
            Recipe recipe = ((FrameworkElement)sender).DataContext as Recipe;
            _gameSession.CraftItemUsing(recipe);
        }

        private void InitializeUserInputActions()
        {
            // add actions to dictionaries using DELEGATES
            // () is the list of parametes passed into the function that we are going to run e.g. (x, y) => SomeFunction(x, y);
            // => the lambda expression
            // in the dictionary we are storing the pointer to this function
            _userInputActions.Add(Key.W, () => _gameSession.MoveNorth());
            _userInputActions.Add(Key.A, () => _gameSession.MoveWest());
            _userInputActions.Add(Key.S, () => _gameSession.MoveSouth());
            _userInputActions.Add(Key.D, () => _gameSession.MoveEast());
            _userInputActions.Add(Key.Z, () => _gameSession.AttackCurrentMonster());
            _userInputActions.Add(Key.C, () => _gameSession.UseCurrentConsumable());
            _userInputActions.Add(Key.I, () => SetTabFocusTo("InventoryTabItem"));
            _userInputActions.Add(Key.Q, () => SetTabFocusTo("QuestsTabItem"));
            _userInputActions.Add(Key.R, () => SetTabFocusTo("RecipesTabItem"));
            _userInputActions.Add(Key.T, () => OnClick_DisplayTradeScreen(this, new RoutedEventArgs()));
        }

        private void MainWindow_OnKeyDown(object? sender, KeyEventArgs e) 
        {
            if (_userInputActions.ContainsKey(e.Key))
            {
                // get the Value/ the DELEGATE and run it
                _userInputActions[e.Key].Invoke();
            }
        }

        private void SetTabFocusTo(string tabName)
        {
            // PlayerDataTabControl.Items is the name of each TabItem inside the conrtol, in XAML
            foreach (object item in PlayerDataTabControl.Items)
            {
                // check if PlayerDataTabControl.Items is a TabItem object, since there could be some other Tag types within the PlayerDataTabControl
                // if it is, assign it to the tabItem variable
                /* The same as writing it as the following
                 * 
                TabItem tabItem = item as TabItem;
                if (tabItem != null)
                {
                    tabItem.IsSelected = true;
                    return;
                } */

                if (item is TabItem tabItem)
                {
                    // check if the name e.g. x:Name="RecipesTabItem" matches the name of the parameter passed in
                    if (tabItem.Name == tabName)
                    {
                        tabItem.IsSelected = true;
                        return;
                    }
                }
            }
        }
    }
}