   M S B A M L   `   `   `  ����N  JWorldTimeConversion, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:WorldTimeConversionWorldTimeConversion  5   	   $    WorldTimeConversion.MainWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:WorldTimeConversion     5   	   -   5          �Name$  MainWindow1��5   	    � Title$ 
MainWindow��5   	   $	��350��6   $	��525��6)   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $	��22* �6   �� 5      $
��385* �6   �� 5      $
��117* �6   .+�5   
   �� $��Your Time zone :��6   $��1��6+   $
��Left=�6;   $��	10,10,0,0q�6V   $	��Top=�6i   �� 5   
   -   5         ��Name$ comboBoxTimeZones��5      $��1��6.   $
��Left=�6>   $��
113,14,0,0q�6Y   $	��Top=�6m   $	��120��6�   �� 5   
   -   5         ��Name$ Enter_Your_Local_TIme_��5      $��Enter Your Local Time:��60   $��22,56,260,0q�6Q   $��26��6f   $	��Top=�6r   $��2��6�   �� 5   
   -   5         ��Name$ txtLocalTime��5       �� TextWrapping$
 Wrap=�6(   $��TextBox��6<   $��1��6K   $��296,58,112,252q�6[   $��2��6s   �� 5   
   -   5         ��Name$ btnConvertToUTC��5      $��Convert To UTC��6*   $��2��6C   $��10,58,10,252q�6S   6i   �� 5   
   -   5        $ lblUtcTimeResult��5      $��Label��6*   $��1��6:   $��148,108,31,199q�6J    J� RenderTransformOrigin$ 0.496,0.535&�6b   �� 5   
   -   5        $ Enter_Your_UTC_Time��5      $��Enter Your UTC Time:��6-   $��22,167,260,0q�6L   $��26��6b   $	��Top=�6n   $��2��6�   �� 5   
   -   5        $ 
txtUtcTime��5      $
 Wrap=�6&   $��TextBox��6:   $��1��6I   $��296,167,112,143q�6Y   $��2��6r   �� 5   
   -	   5        $ btnConvertToLocal��5      $��Convert To Local��6,   $��2��6G   $��10,167,10,143q�6W   6n   �� 5   
   -
   5        $ lblLocalTimeResult��5      $��Label��6,   $��1��6<   $��148,231,31,76q�6L   $ 0.496,0.535&�6c   (  "System.Windows.Controls.DatePicker  5   
   -   5        	  Name$	 datePickerLocal��5      $��133,59,95,252q�6.   $��1��6E     5   
   -   5        $	 datePickerUTC��5      $��133,167,95,144q�6,   $��1��6D   5      5      