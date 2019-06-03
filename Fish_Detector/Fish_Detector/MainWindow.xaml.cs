using Emgu.CV;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Windows;

namespace Fish_Detector
{
	public partial class MainWindow : Window
	{
		private bool isProcessing = false;
        private bool isUpdateRequired = false;
		private bool isUiInitialized = false;

		public MainWindow()
		{
			InitializeComponent();

			isUiInitialized = true;

		}

		private void SaveImage(object sender, RoutedEventArgs e)
		{
			SaveImage();
		}

		private void SaveImage()
		{
			EmguCV.SaveImage();
		}

		private void LoadImage(object sender, RoutedEventArgs e)
		{
			LoadImage();
		}

		private void LoadImage()
		{
			EmguCV.LoadImage();
			originalImage.Source = EmguCV.GetOriginalBitmap();
			resultImage.Source = EmguCV.GetResultBitmap();

			saveImageButton.IsEnabled = true;

            ProcessImage();
		}

		private void ProcessImage()
		{
            if(!EmguCV.IsImageLoaded() || isUpdateRequired)
            {
                return;
            }

			if (isProcessing && !isUpdateRequired)
			{
                isUpdateRequired = true;
                while (!isProcessing);
                isUpdateRequired = false;
			}

			isProcessing = true;
				
			Stopwatch stopwatch = Stopwatch.StartNew();
			Preprocessing();
			preprocessingImage.Source = EmguCV.GetOriginalBitmap();
			stopwatch.Stop();

			preprocessingTimeTextBox.Text = stopwatch.ElapsedMilliseconds.ToString();

			stopwatch.Restart();
			System.Drawing.Point position = DetectFish();
			stopwatch.Stop();

			detectionTimeTextBox.Text = stopwatch.ElapsedMilliseconds.ToString();
			objectPositionXTextBox.Text = position.X.ToString();
			objectPositionYTextBox.Text = position.Y.ToString();

			resultImage.Source = EmguCV.GetResultBitmap();
			
			isProcessing = false;
		}

		private void Preprocessing()
		{
			EmguCV.ConvertToGrayscale();
			EmguCV.GaussianBlur(adaptiveKernelSizeCheckBox.IsChecked, adaptiveSigmaCheckBox.IsChecked, Convert.ToInt32(kernelSizeSlider.Value), sigmaXSlider.Value, sigmaYSlider.Value);
			EmguCV.EqualizeHistogram(adaptiveHistogramEqualizationCheckBox.IsChecked);
			EmguCV.Threshold(adaptiveThresholdCheckBox.IsChecked, Convert.ToInt32(thresholdSlider.Value));
		}

		private System.Drawing.Point DetectFish()
		{
			return EmguCV.FindContour();
		}

		private void KernelSizeSliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if(!isUiInitialized)
			{
				return;
			}

			kernelSizeTextBox.Text = kernelSizeSlider.Value.ToString();

            ProcessImage();

        }

		private void SigmaXSliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (!isUiInitialized)
			{
				return;
			}

			sigmaXTextBox.Text = sigmaXSlider.Value.ToString("0.00").Replace(',', '.');

            ProcessImage();
        }

		private void SigmaYSliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (!isUiInitialized)
			{
				return;
			}

			sigmaYTextBox.Text = sigmaYSlider.Value.ToString("0.00").Replace(',', '.');

            ProcessImage();
        }

		private void ThresholdSliderChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			if (!isUiInitialized)
			{
				return;
			}

			thresholdTextBox.Text = thresholdSlider.Value.ToString();

            ProcessImage();
        }

		private void AdaptiveHistogramEqualizationChecked(object sender, RoutedEventArgs e)
		{
			if (!isUiInitialized)
			{
				return;
			}

			ProcessImage();
		}

		private void AdaptiveHistogramEqualizationUnchecked(object sender, RoutedEventArgs e)
		{
			if (!isUiInitialized)
			{
				return;
			}

			ProcessImage();
		}

		private void AdaptiveKernelSizeChecked(object sender, RoutedEventArgs e)
		{
			if (!isUiInitialized)
			{
				return;
			}

			kernelSizeSlider.IsEnabled = false;
			kernelSizeTextBox.IsEnabled = false;

			ProcessImage();
		}

		private void AdaptiveKernelSizeUnchecked(object sender, RoutedEventArgs e)
		{
			if (!isUiInitialized)
			{
				return;
			}

			kernelSizeSlider.IsEnabled = true;
			kernelSizeTextBox.IsEnabled = true;

			ProcessImage();
		}

		private void AdaptiveSigmaChecked(object sender, RoutedEventArgs e)
		{
			if (!isUiInitialized)
			{
				return;
			}

			sigmaXSlider.IsEnabled = false;
			sigmaXTextBox.IsEnabled = false;
			sigmaYSlider.IsEnabled = false;
			sigmaYTextBox.IsEnabled = false;

			ProcessImage();
		}

		private void AdaptiveSigmanUnchecked(object sender, RoutedEventArgs e)
		{
			if (!isUiInitialized)
			{
				return;
			}

			sigmaXSlider.IsEnabled = true;
			sigmaXTextBox.IsEnabled = true;
			sigmaYSlider.IsEnabled = true;
			sigmaYTextBox.IsEnabled = true;

			ProcessImage();
		}

		private void AdaptiveThresholdChecked(object sender, RoutedEventArgs e)
		{
			if (!isUiInitialized)
			{
				return;
			}

			thresholdSlider.IsEnabled = false;
			thresholdTextBox.IsEnabled = false;

			ProcessImage();
		}

		private void AdaptiveThresholdUnchecked(object sender, RoutedEventArgs e)
		{
			if (!isUiInitialized)
			{
				return;
			}

			thresholdSlider.IsEnabled = true;
			thresholdTextBox.IsEnabled = true;

			ProcessImage();
		}
	}
}
