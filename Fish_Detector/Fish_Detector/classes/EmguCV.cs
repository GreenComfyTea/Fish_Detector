using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Fish_Detector
{
	public class EmguCV
	{
		private static Mat originalMatImage;
		private static Mat resultMatImage;

		private static int thresholdAddition = 60;

		public static BitmapSource GetOriginalBitmap()
		{
			if (originalMatImage == null)
			{
				return null;
			}

			return ToBitmapSource(originalMatImage);
		}

		public static BitmapSource GetResultBitmap()
		{
			if (resultMatImage == null)
			{
				return null;
			}

			return ToBitmapSource(resultMatImage);
		}

		public static void LoadImage()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

			if (openFileDialog.ShowDialog() == true)
			{
				if (openFileDialog.FileName.Length > 0)
				{
					LoadImage(openFileDialog.FileName);
				}
			}
		}

		public static void LoadImage(string fileName)
		{
			originalMatImage = CvInvoke.Imread(fileName, ImreadModes.Color);
			resultMatImage = originalMatImage.Clone();
		}

		public static void SaveImage()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			saveFileDialog.Filter = "Image File|.png;";

			if (saveFileDialog.ShowDialog() == true)
			{
				if (saveFileDialog.FileName.Length > 0)
				{
					SaveImage(saveFileDialog.FileName);
				}
			}
		}

		public static void SaveImage(string fileName)
		{
			CvInvoke.Imwrite(fileName, resultMatImage);
		}

		[DllImport("gdi32")]
		private static extern int DeleteObject(IntPtr intPtr);

		private static BitmapSource ToBitmapSource(IImage image)
		{
			IntPtr intPtr = image.Bitmap.GetHbitmap();

			BitmapSource bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
				intPtr,
				IntPtr.Zero,
				Int32Rect.Empty,
				BitmapSizeOptions.FromEmptyOptions()
			);

			DeleteObject(intPtr);
			return bitmapSource;
		}

		public static void ReloadResultImage()
		{
			resultMatImage = originalMatImage.Clone();
		}

		public static bool IsImageLoaded()
		{
			return resultMatImage != null;
		}

		public static void ConvertToGrayscale()
		{
			CvInvoke.CvtColor(resultMatImage, resultMatImage, ColorConversion.Bgr2Gray);
		}

		public static void EqualizeHistogram(bool? adaptiveEqualization)
		{
			if(adaptiveEqualization == true)
			{
				CvInvoke.CLAHE(resultMatImage, 0d, new System.Drawing.Size(16, 16), resultMatImage);
				thresholdAddition = 40;
			}
			else
			{
				CvInvoke.EqualizeHist(resultMatImage, resultMatImage);
				thresholdAddition = 60;
			}
		}

		public static void GaussianBlur(bool? adaptiveKernelSize, bool? adaptiveSigma, int kernelSize, double sigmaX = 0, double sigmaY = 0)
		{
			if(adaptiveKernelSize == true)
			{
				kernelSize = Convert.ToInt32(Math.Ceiling(resultMatImage.Cols * 0.0135d));
				if(kernelSize % 2 == 0)
				{
					kernelSize--;
				}

				if (kernelSize < 3)
				{
					kernelSize = 3;
				}
			}

			if(adaptiveSigma == true)
			{
				sigmaX = 0;
				sigmaY = 0;
			}

			CvInvoke.GaussianBlur(resultMatImage, resultMatImage, new System.Drawing.Size(kernelSize, kernelSize), sigmaX, sigmaY);
		}

		public static void Threshold(bool? adaptiveThreshold, int threshold)
		{
			if (adaptiveThreshold == true)
			{
				Mat temp = new Mat();
				double otsuThresholdDouble = CvInvoke.Threshold(resultMatImage, temp, 0, 255, ThresholdType.Otsu | ThresholdType.Binary);
				threshold = thresholdAddition + Convert.ToInt32(otsuThresholdDouble);
			}

			CvInvoke.Threshold(resultMatImage, resultMatImage, threshold, 255, ThresholdType.Binary);
		}

		public static System.Drawing.Point FindContour()
		{
			using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
			{
				CvInvoke.FindContours(resultMatImage, contours, null, RetrType.List, ChainApproxMethod.LinkRuns);

				if (contours.Size > 0)
				{
					double maxArea = 0;
					int chosen = 0;
					for (int i = 0; i < contours.Size; i++)
					{
						VectorOfPoint contour = contours[i];

						double area = CvInvoke.ContourArea(contour);
						if (area > maxArea)
						{
							maxArea = area;
							chosen = i;
						}
					}

					resultMatImage = originalMatImage.Clone();
					return MarkDetectedObject(resultMatImage, contours[chosen]);
				}
			}

			return new System.Drawing.Point(-1, -1);
		}

		private static System.Drawing.Point MarkDetectedObject(Mat frame, VectorOfPoint contour)
		{
			System.Drawing.Point[] points = contour.ToArray();
			PointF[] pointsF = new PointF[points.Length];

			int[] nums = Enumerable.Range(0, points.Length).ToArray();
			Parallel.ForEach(nums, i =>
			{
				pointsF[i] = new PointF(points[i].X, points[i].Y);
			});

			RotatedRect rotatedRectangle = CvInvoke.MinAreaRect(pointsF);

			Rectangle box = CvInvoke.BoundingRectangle(contour);

			System.Drawing.Point center = new System.Drawing.Point(box.X + box.Width / 2, box.Y + box.Height / 2);
			System.Drawing.Point rotatedCenter = new System.Drawing.Point(Convert.ToInt32(rotatedRectangle.Center.X), Convert.ToInt32(rotatedRectangle.Center.Y));
			System.Drawing.Point shift = new System.Drawing.Point(center.X - rotatedCenter.X, center.Y - rotatedCenter.Y);

			PointF[] verticesF = rotatedRectangle.GetVertices();

			System.Drawing.Point[] vertices = new System.Drawing.Point[4];
			vertices[0] = new System.Drawing.Point(Convert.ToInt32(verticesF[0].X) + shift.X, Convert.ToInt32(verticesF[0].Y) + shift.Y);
			vertices[1] = new System.Drawing.Point(Convert.ToInt32(verticesF[1].X) + shift.X, Convert.ToInt32(verticesF[1].Y) + shift.Y);
			vertices[2] = new System.Drawing.Point(Convert.ToInt32(verticesF[2].X) + shift.X, Convert.ToInt32(verticesF[2].Y) + shift.Y);
			vertices[3] = new System.Drawing.Point(Convert.ToInt32(verticesF[3].X) + shift.X, Convert.ToInt32(verticesF[3].Y) + shift.Y);
		

			double fontCoeficient = 0.00109d;
			double fontScale = (resultMatImage.Cols + resultMatImage.Rows) * 0.5d * fontCoeficient;
			int lineThickness = Convert.ToInt32(fontScale * 3d);

			if(lineThickness <= 0)
			{
				lineThickness = 1;
			}

			MCvScalar redColor = new Bgr(Color.Red).MCvScalar;

			CvInvoke.Rectangle(frame, box, new Bgr(Color.Green).MCvScalar, lineThickness);
			CvInvoke.Line(frame, vertices[0], vertices[1], redColor, lineThickness);
			CvInvoke.Line(frame, vertices[1], vertices[2], redColor, lineThickness);
			CvInvoke.Line(frame, vertices[2], vertices[3], redColor, lineThickness);
			CvInvoke.Line(frame, vertices[3], vertices[0], redColor, lineThickness);

			CvInvoke.Circle(frame, center, lineThickness * 3, redColor, -1);

			string positionString = center.X.ToString().Replace(',', '.') + ", " + center.Y.ToString().Replace(',', '.');

			WriteMultilineText(frame, positionString, new System.Drawing.Point(box.Right + 5, center.Y));

			return center;
		}

		private static void WriteMultilineText(Mat frame, string text, System.Drawing.Point origin)
		{
			double fontCoeficient = 0.00109d;
			double fontScale = (resultMatImage.Cols + resultMatImage.Rows) * 0.5d * fontCoeficient;
			int fontThickness = Convert.ToInt32(fontScale * 2d);

			if (fontThickness <= 0)
			{
				fontThickness = 1;
			}

			CvInvoke.PutText(frame, text, new System.Drawing.Point(origin.X, origin.Y), FontFace.HersheyComplex, fontScale, new Bgr(Color.White).MCvScalar, fontThickness * 5);
			CvInvoke.PutText(frame, text, new System.Drawing.Point(origin.X, origin.Y), FontFace.HersheyComplex, fontScale, new Bgr(Color.Black).MCvScalar, fontThickness);
		}
	}
}
