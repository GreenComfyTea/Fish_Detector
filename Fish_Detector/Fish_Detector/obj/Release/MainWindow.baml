   M S B A M L   `   `   `  ���� H  DFish_Detector, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Fish_DetectorFish_Detector      Fish_Detector.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:Fish_Detector       � Title$:  4Fish Detector (Artjoms Lužnovs, 161RDB140, 3RDBF01)��$
��1280��$	��720��$
��1280��$	��720��
������� � WindowStartupLocation$ CenterScreen=�.���  � ColumnDefinitions �� $��10 ��� �� $��10 ��� �� $��10 ��� $	��210 ��� $��10 � � RowDefinitions �� $��10 ��� �� $��10 ��� �� $��10 �.+� � $��1��$��1��
������� � $��1��$��3��$��3��
������� � $��1��$��5��$��3��
������� � $��3��$��1��
��������� -    ��Name$ originalImage��$��1��$��1���� -   $ resultImage��$��1��$��3��$��3���� -   $ preprocessingImage��$��3��$��1���� $��1��$��5�� �� Orientation$ Vertical=�$��5q�$��3��.+��� -    ��Name$ loadImageButton��$��
Load Image��$��20���� -   $ saveImageButton��$��
Save Image��$��20��$��
0, 5, 0, 0q�{�.  � $��
0, 5, 0, 0q� �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� -    ��Name$+ %adaptiveHistogramEqualizationCheckBox��$%��Adaptive Histogram Equalization�� d� 	IsChecked$
 TrueY��� -   $  adaptiveKernelSizeCheckBox��$#��Adaptive Gaussian Kernel Size��$��1��$
 TrueY�$��
0, 5, 0, 0q��� -   $ adaptiveSigmaCheckBox��$��Adaptive Gaussian Sigma��$��2��$��
0, 5, 0, 0q�$
 TrueY��� -	   $ adaptiveThresholdCheckBox��$��Adaptive Threshold��$��3��$��
0, 5, 0, 0q�$
 TrueY�� $��
0, 5, 0, 0q� �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto � �� $��* ��� $��50 �.+��� $ ��Gaussian Blur: Kernel Size��$��0���� $��Gaussian Blur: Sigma X��$��2���� $��Gaussian Blur: Sigma Y��$��4���� $��Binarization: Threshold��$��6���� -
   	 ��Name$	 kernelSizeTextBox��$��9��$��0��$��2��$��Stretch=�$��Center=�|�.  {�.  �� -   $	 sigmaXTextBox��$
��0.00��$��2��$��2��$��Stretch=�$��Center=�|�.  {�.  �� -   $	 sigmaYTextBox��$
��0.00��$��4��$��2��$��Stretch=�$��Center=�|�.  {�.  �� -   $	 thresholdTextBox��$	��200��$��6��$��2��$��Stretch=�$��Center=�|�.  {�.  �� -   
 ��Name$
 kernelSizeSlider��$��1��$��3�� � Minimum$ 3Q� � Maximum$	 255Q� � Value$ 9Q� �� TickFrequency$ 2Q� �� IsSnapToTickEnabled . {�.  �� -   $
 sigmaXSlider��$��3��$��3��$ 0Q�$	 255Q�$ 0Q�{�.  �� -   $
 sigmaYSlider��$��5��$��3��$ 0Q�$	 255Q�$ 0Q�{�.  �� -   $
 thresholdSlider��$��7��$��3��$ 0Q�$	 255Q�$	 200Q� . {�.  � $��
0, 5, 0, 0q� �� $
��Auto ��� $
��Auto ��� $��21 ��� $
��Auto � �� $
��Auto ��� $��* ��� $��* �.+��� $��Preprocessing Time (ms):���� $��Detection Time (ms):��$��1��$��
0, 5, 0, 0q��� $��Object Position (px):��$��3��$��
0, 5, 0, 0q��� -    ��Name$ preprocessingTimeTextBox��$��-1��$��1��$��Center=��� -   $ detectionTimeTextBox��$��-1��$��1��$��1��$��
0, 5, 0, 0q�$��Center=��� -   $ objectPositionXTextBox��$��-1��$��3��$��1��$��
0, 5, 0, 0q�$��Center=��� -   $ objectPositionYTextBox��$��-1��$��3��$��3��$��
0, 5, 0, 0q�$��Center=�