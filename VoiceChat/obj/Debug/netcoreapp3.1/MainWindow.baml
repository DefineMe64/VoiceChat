   M S B A M L   `   `   `  ����D  @VoiceChat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:VoiceChat	VoiceChat  5   	       VoiceChat.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   V RMaterialDesignThemes.Wpf, Version=3.1.3.1513, Culture=neutral, PublicKeyToken=nullZ VMaterialDesignThemes.MahApps, Version=0.1.4.1513, Culture=neutral, PublicKeyToken=nullHmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes   5   	   %localclr-namespace:VoiceChat   5   	   -   5          � 
ResizeMode$  NoResize=�5	   	    � AllowsTransparency$ False��5
   	    � WindowStyle$
 None=�5   	    � Icon$ images/logo.ico��5   	    � WindowStartupLocation$ CenterScreen=�5   	    � Title$ 登录��5   	   $	��320��6   $	��420��6!   .��5      �  � RowDefinitions 5   
   �� 5      $	��180 �6   6+   �� 5      $��* �6   6)   �� 5      $��50 �6   6*    � ColumnDefinitions 5   
   �� 5      $	��120 �6   6-   �� 5      $��* �6   6+   �� 5      $	��120 �6   6-   .+�5   
   �� $��images/LoginBG.png��6   $��UniformToFill=�6,   $��0��6D   $��3��6T   6j   �� 5   
   -   5         ��Name$ localUserName��5      $��40��5      $	��180��6   $��14(�6*   	 �� 	MaxLength$	 20��68   $��Center=�5      $��Center=�64   
 �� 
LineHeight$
 22��5       �� LineStackingStrategy$ BlockLineHeight=�6,   $��1��5       $��1��6   ���#ccc
���   �$��	0,30,0,20q�5"      6'   �� 5#   
   -   5        $��登  录��5#      $��30��6    $	��180��6,   $��12(�68   ���#00A3FF � Cursor$
 Hand��5$   &   
����������#00A3FF$��0q�5&      $��Center=�5'      $��Center=�63   $��1��5(      $��2��6!   $��Center=�6.   $��Center=�6I   5)   '   �� 5*   
   -   5        $��2��5*      $	��Top=�6!   $
��Left=�69      MaterialDesignFlatButton#��[  5+      $��60��6C   .��5,      '  !MaterialDesignThemes.Wpf.PackIcon     Kind$ WindowMinimize=�6&   $��25��6<   $��25��6G   6U   5-      �� 5.   
   -   5        $��2��5.      $	��Top=�6!   $��Right=�69   #��[  5/      $��60��6C   .��50        $ WindowClose=�6&   $��25��69   $��25��6D   6R   51      52      53      