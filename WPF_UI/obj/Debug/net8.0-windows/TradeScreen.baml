   M S B A M L   `   `   `  ����A  =WPF_UI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:WPF_UIWPF_UI  5   	   A =Engine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE/clr-namespace:Engine.ViewModels;assembly=EngineEngine.ViewModels 5   	       WPF_UI.TradeScreen   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:WPF_UI     5   	   F
viewModels/clr-namespace:Engine.ViewModels;assembly=Engine     5   	     � WindowStartupLocation$  CenterOwner=�5
   	   $
��11pt(�5   	    � Title$ Trade Screen��5   	   $	��400��6   $	��800��6+   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $
��auto �6   �� 5      $��* �6   �� 5      $
��auto �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0��6   $��2��6-   $��Center=�6A   ��6^   �� *CurrentTrader.Name+�� 5   
   $��1��6   $��0��6   $��Center=�6-   $��Your Inventory}��6J   �� 5   
   $��1��6   $��1��6   $��Center=�6-   $��Trader's Inventory��6J   &   System.Windows.Controls.DataGrid  5!   
   $��2��6   $��0��6    $��10q�60      AutoGenerateColumns$ False��6s      HeadersVisibility$ Column=�5"         CanUserAddRows$ False��6.      CanUserDeleteRows$ False��6E   "   VerticalScrollBarVisibility$
 Auto=�6_   ��5!   <   �� * CurrentPlayer.GroupedInventory+	   Columns	 5#      0  *System.Windows.Controls.DataGridTextColumn  5$      ,  &System.Windows.Controls.DataGridColumn
   Header$
 Description��6%      
IsReadOnly$
 True��6:      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6L   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6V   ��  �� Path$ 	Item.Name�  5%      $
 In Bag��6%   $
 True��65   $
 Auto 6G    6T   �� $ Quantity�  5&      $
 Price��6%   $
 True��64   $
 Auto 6F    6S   �� $ 
Item.Price�4  .System.Windows.Controls.DataGridTemplateColumn  5(         MinWidth$ 75Q�6)      CellTemplate 5)      �� 5*      .R�5+      �� -   5        $��55��5+   :   $��Sell 1��6E   5,      5.      50        52   
   $��2��6   $��1��6    $��10q�60   $ False��6s   $ Column=�53      $ False��6.   $ False��6E   $
 Auto=�6_   ��52   <   �� * CurrentTrader.GroupedInventory+	 54        55      $
 Description��6%   $
 True��6:   $ * 6L    6V   �� $ 	Item.Name�  56      $
 In Stock��6%   $
 True��67   $
 Auto 6I    6V   �� $ Quantity�  57      $
 Price��6%   $
 True��64   $
 Auto 6F    6S   �� $ 
Item.Price�  59      $ 75Q�6)    5:      �� 5;      .R�5<      �� -   5        $��55��5<   9   $��Buy 1��6D   5=      5?      5A      �� 5C   
   -   5        $��3��5C      $��1��6   $��Right=�6.   $��Bottom=�6J   $��75��6e   $��Close��6p   6�   5E      5F      