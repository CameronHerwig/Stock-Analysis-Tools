   M S B A M L   `   `   `  ����  StockUI    StockUI.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      $	��610��5   H   $	��530��6U   $��
10,0,10,10q�6a     � Title  6   �� *windowTitle+ �� UpdateSourceTrigger$ PropertyChanged=�.��5      �� $��Stretch=�6   $��Stretch=�6/   $
��Auto��6K   $��0,10,2,0q�6Y   $
��Auto��6l   .@�5   
   �� $��Fundamentals��6   .��5      � ���	#FFE5E5E5$��	0,0,0,-11q�6*   $
��Auto��6=    � ColumnDefinitions 5      �� 5	      $
��130* �6'   �� 5
      $	��85* �6'   �� 5      $
��271* �6'   .+�5      �� $��Stretch=�6   $
��Auto��6;   $��
25,25,0,10q�6I   $��Stretch=�6]   $
��Auto��6y   $��3��6�   .+�5      &   System.Windows.Controls.DataGrid  -   5          Name$
 Data��5      $	��244��6.   $��
0,0,136,20q�6;    J� RenderTransformOrigin$ 0.5,0.5&�6O   �� 5      -   5        $��Right=�5      $��Extract From HTML��69   $��0,0,20,0q�6U   $��20��6g   $	��150��6s   6   �� 5      -   5        $��Right=�5      $��Add Fundmentals��69   $��	0,10,20,0q�6S   $��20��6f   $	��150��6r   6~   �� 5      -   5        $��Right=�5      $��	Get Dates��69   $��	0,10,20,0q�6M   $��20��6`   $	��150��6l   6x   �� 5      -   5        $��Right=�5      $��Gather Fundmentals��69   $��	0,10,20,0q�6V   $��20��6i   $	��150��6u   6�   �� 5      -   5        $��Right=�5      $��Compare��69   $��	0,10,20,0q�6K   $��20��6^   $	��150��6j   6v   �� 5      $��Select Month:��6   $��	0,0,370,0q�64   �� 5      -   5         ��Name$ SelectMonth��5   2   $��	0,0,321,0q�6   ��5      �� 5      .R�5   "   ��  �� Orientation$ 
Horizontal=�6-   .+�5   &   �� ��60   �� 5   #   5      5      5      �� 5      -   5        $��Show Errors��5      $��2��61   $
��Left=�6A   $��160,440,20,20q�6\   $	��Top=�6s   $ 0.386,-5.067&�6�   �� 5       -	   5        $��Settings��5       $��2��6,   $��Right=�6<   $��0,0 ,0,0q�6i   $	��Top=�6{   $��55��6�   �� 5!      -
   5        $
��Help��5!      $��2��6(   $��Right=�68   $��0,0,57,0q�6e   $	��Top=�6w   $��55��6�   5"      5#      �� 5$   
   $��Comparisons��6   .��5%      � ���	#FFE5E5E5$��	0,0,0,-11q�6*    5&      �� 5'      $	��28* �6'   �� 5(      $
��194* �6'   �� 5)      $
��263* �6'   .+�5+      �� $��Stretch=�6   $
��Auto��6;   $��
25,25,-8,3q�6I   $��Stretch=�6]   $
��Auto��6y   $��3��6�   .+�5,        -   5        $ Data1��5,         AutoGenerateColumns .  $	��244��6k   $��	0,0,30,20q�6x   $ 0.5,0.5&�6�   ��6K   �� *	Results+6�   �� 5-      -   5        $��Right=�5-      $��Get All Comparisons��69   $��	0,10,20,0q�6W   $��20��6j   $	��150��6v   6�   5.      �� 5/      -   5        $��Exclude Partial Results��5/      $��2��6=   $
��Left=�6M    d� 	IsChecked$
 TrueY�6h   $��102,337,0,0q�6y   $	��Top=�6�   6�   �� 50      -   5        $��Settings��50      $��2��6,   $��Right=�6<   $��0,0,0,0q�6i   $	��Top=�6z   $��55��6�   �� 51      -   5        $
��Help��51      $��2��6(   $��Right=�68   $��	0, 0,57,0q�6d   $	��Top=�6w   $��55��6�   52      53      �� 54   
   $��Individual Tests��6   .��55      � ���	#FFE5E5E5$��	0,0,0,-11q�6*    56      �� 57      $
��129* �6'   �� 58      $
��355* �6'   .+�5:      �� $��Stretch=�6   $
��Auto��6;   $��
25,25,0,10q�6I   $��Stretch=�6]   $
��Auto��6y   $��2��6�   .+�5;        -   5        $ PredictionData��5;      $	��244��68   $��
0,0,136,20q�6E   $ 0.5,0.5&�6Y   �� 5<      $��Select Month:��6   $��	0,0,368,0q�64   �� 5=      -   5        $ SelectPredictionMonth��5=   2   $��	0,0,321,0q�6   ��5>      �� 5?      .R�5@   "   �� $ 
Horizontal=�6-   .+�5A   &   �� ��60   �� 5B   #   5C      5E      �� 5F      -   5        $��Right=�5F      $��Get HTML��69   $��	0,80,20,0q�6L   $��20��6_   $	��150��6k   6w   �� 5G      -   5        $��Right=�5G      $��	Get Dates��69   $��	0,10,20,0q�6M   $��20��6`   $	��150��6l   6x   �� 5H      -   5        $��Right=�5H      $��Predict��69   $��	0,10,20,0q�6K   $��20��6^   $	��150��6j   6v   5I      �� 5J      -   5        $	��ADX��5J   y   $��1��6   $
��Left=�6*   $��23��6E   $��
97,312,0,0q�6Q   	 �� TextWrapping$
	 Wrap=�6e   $	��Top=�6�   $	��100��6�   �� 5K      -   5        $��BBANDS��5K   y   $��1��6   $
��Left=�6*   $��23��6E   $��
97,340,0,0q�6Q   $
	 Wrap=�6e   $	��Top=�6�   $	��100��6�   �� 5L      -   5        $	��BOP��5L   y   $��1��6   $
��Left=�6*   $��23��6E   $��
97,368,0,0q�6Q   $
	 Wrap=�6e   $	��Top=�6�   $	��100��6�   �� 5M      -   5        $
��MACD��5M   z   $��1��6   $
��Left=�6*   $��23��6E   $��253,312,0,0q�6Q   $
	 Wrap=�6f   $	��Top=�6�   $	��100��6�   �� 5N      -   5        $	��MOM��5N   z   $��1��6   $
��Left=�6*   $��23��6E   $��253,340,0,0q�6Q   $
	 Wrap=�6f   $	��Top=�6�   $	��100��6�   �� 5O      -   5        $	��RSI��5O   z   $��1��6   $
��Left=�6*   $��23��6E   $��253,368,0,0q�6Q   $
	 Wrap=�6f   $	��Top=�6�   $	��100��6�   �� 5P      $	��ADX��6   $��1��6&   $
��Left=�66   $��
59,312,0,0q�6Q   $	��Top=�6e   $��23��6}   �� 5Q      $��BBANDS��6   $��1��6)   $
��Left=�69   $��
40,340,0,0q�6T   $	��Top=�6h   $��23��6�   $��57��6�   �� 5R      $	��BOP��6   $��1��6&   $
��Left=�66   $��
59,368,0,0q�6Q   $	��Top=�6e   $��23��6}   $ 0.421,2.696&�6�   �� 5S      $
��MACD��6   $��1��6'   $
��Left=�67   $��205,312,0,0q�6R   $	��Top=�6g   $��23��6   $��44��6�   �� 5T      $	��MOM��6   $��1��6&   $
��Left=�66   $��205,340,0,0q�6Q   $	��Top=�6f   $��23��6~   $ 2.947,1.391&�6�   �� 5U      $	��RSI��6   $��1��6&   $
��Left=�66   $��205,368,0,0q�6Q   $	��Top=�6f   $��23��6~   $��41��6�   �� 5V      -   5        $��Show Errors��5V      $��1��61   $
��Left=�6A   $��253,508,0,0q�6\   $	��Top=�6q   $ 0.386,-5.067&�6�   �� 5W      -   5        $��Settings��5W      $��2��6,   $��Right=�6<   $��0,0,0,0q�6i   $	��Top=�6z   $��55��6�   �� 5X      -   5        $
��Help��5X      $��2��6(   $��Right=�68   $��0,0,57,0q�6d   $	��Top=�6v   $��55��6�   5Y      5Z      5[      5\      