using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TextFromImages
{
    public static class ImagePreprocessor
    {
        // Method to rotate an image by a specified angle
        public static string RotateImage(string imagePath, float angle)
        {
            using (var bmp = new Bitmap(imagePath))
            {
                using (var rotatedBmp = new Bitmap(bmp.Width, bmp.Height))
                {
                    using (var g = Graphics.FromImage(rotatedBmp))
                    {
                        g.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);
                        g.RotateTransform(angle);
                        g.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);
                        g.DrawImage(bmp, new Point(0, 0));
                    }

                    string rotatedImagePath = Path.GetTempFileName();
                    rotatedBmp.Save(rotatedImagePath, ImageFormat.Jpeg);
                    return rotatedImagePath;
                }
            }
        }

        // Method to shift an image by specified x and y pixels
        public static string ShiftImage(string imagePath, int xShift, int yShift)
        {
            using (var bmp = new Bitmap(imagePath))
            {
                using (var shiftedBmp = new Bitmap(bmp.Width, bmp.Height))
                {
                    using (var g = Graphics.FromImage(shiftedBmp))
                    {
                        g.DrawImage(bmp, new Point(xShift, yShift));
                    }

                    string shiftedImagePath = Path.GetTempFileName();
                    shiftedBmp.Save(shiftedImagePath, ImageFormat.Jpeg);
                    return shiftedImagePath;
                }
            }
        }

        // Method to resize an image by a specified scale factor
        public static string ResizeImage(string imagePath, float scale)
        {
            using (var bmp = new Bitmap(imagePath))
            {
                int newWidth = (int)(bmp.Width * scale);
                int newHeight = (int)(bmp.Height * scale);

                using (var resizedBmp = new Bitmap(newWidth, newHeight))
                {
                    using (var g = Graphics.FromImage(resizedBmp))
                    {
                        g.DrawImage(bmp, 0, 0, newWidth, newHeight);
                    }

                    string resizedImagePath = Path.GetTempFileName();
                    resizedBmp.Save(resizedImagePath, ImageFormat.Jpeg);
                    return resizedImagePath;
                }
            }
        }

        // Method to convert an image to grayscale
        public static string ConvertToGrayscale(string imagePath)
        {
            using (var bmp = new Bitmap(imagePath))
            {
                using (var grayscaleBmp = new Bitmap(bmp.Width, bmp.Height))
                {
                    using (var g = Graphics.FromImage(grayscaleBmp))
                    {
                        var colorMatrix = new System.Drawing.Imaging.ColorMatrix(
                            new float[][]
                            {
                                new float[] { 0.299f, 0.299f, 0.299f, 0, 0 },
                                new float[] { 0.587f, 0.587f, 0.587f, 0, 0 },
                                new float[] { 0.114f, 0.114f, 0.114f, 0, 0 },
                                new float[] { 0, 0, 0, 1, 0 },
                                new float[] { 0, 0, 0, 0, 1 }
                            });

                        var attributes = new ImageAttributes();
                        attributes.SetColorMatrix(colorMatrix);

                        g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height),
                            0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
                    }

                    string grayscaleImagePath = Path.GetTempFileName();
                    grayscaleBmp.Save(grayscaleImagePath, ImageFormat.Jpeg);
                    return grayscaleImagePath;
                }
            }
        }
    }
}