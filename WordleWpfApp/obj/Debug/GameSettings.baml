   M S B A M L   `   `   `  ����G  CWordleWpfApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:WordleWpfAppWordleWpfApp  5   	   %    WordleWpfApp.GameSettingsWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:WordleWpfApp     5   	     � Title$  Game Settings��5   	    � 
ResizeMode$ NoResize=�6   $	��250��65   $	��230��6B   .��5	      � .+�5
   
   �� -   5         ��Name$ textBox1��5
       �� TextAlignment$ Center=�6$   $
��Left=�6;   $��50��6V   $��	15,20,0,0q�6b    �� TextWrapping$
 Wrap=�6u   $�� ��6�   $	��Top=�6�   $��50��6�   $��36(�6�   $
��Bold"�6�   
������� �� 
IsReadOnly . 6�   �� 5   
   -   5        $ textBox2��5      $ Center=�6$   $
��Left=�6;   $��50��6V   $��	70,20,0,0q�6b   $
 Wrap=�6u   $�� ��6�   $	��Top=�6�   $��50��6�   $��36(�6�   $
��Bold"�6�   
������� . 6�   �� 5   
   -   5        $ textBox3��5      $ Center=�6$   $
��Left=�6;   $��50��6V   $��
125,20,0,0q�6b   $
 Wrap=�6v   $�� ��6�   $	��Top=�6�   $��50��6�   $��36(�6�   $
��Bold"�6�   
������� . 6�   �� 5   
   -   5         ��Name$ groupBoxSettings��5      $��Settings��6-   $
��Left=�6?   $��90��6Z   $	��Top=�6f   $	��200��6~   $��	10,80,0,0q�6�   .��5      �� $��5q�6   .+�5      �� -   5         ��Name$ checkBoxDarkMode��5      $��	Dark Mode��65   $��0,0,0,5q�6I   6Z   �� 5      -   5        $ checkBoxHighContrastMode��5      $��High Contrast Mode��6=   $��0,0,0,5q�6Z   6k   �� 5      -   5        $ checkBoxHardMode��5      $��	Hard Mode��65   6I   5      5      �� 5   
   -   5         ��Name$ buttonOK��5      $��OK��6#   $
��Left=�60   $��
25,185,0,0q�6K   $	��Top=�6_   $��75��6w   	 �� 	IsDefault	 . 6�   �� 5   
   -	   5        $ buttonCancel��5      $��Cancel��6'   $
��Left=�68   $��120,185,0,0q�6S   $	��Top=�6h   $��75��6�   
 �� IsCancel
 . 6�   5      5      