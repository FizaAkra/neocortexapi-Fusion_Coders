using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

class ImageProcessor
{
    public static Dictionary<string, Bitmap> ApplyTransformations(Bitmap original)
    {
        Dictionary<string, Bitmap> processedImages = new Dictionary<string, Bitmap>();

        processedImages.Add("Original", new Bitmap(original));
        processedImages.Add("Grayscale", ConvertToGrayscale(original));
        processedImages.Add("Rotated_90", RotateImage(original, 90));
        processedImages.Add("Rotated_180", RotateImage(original, 180));

        return processedImages;
    }

    private static Bitmap ConvertToGrayscale(Bitmap image)
    {
        Bitmap grayImage = new Bitmap(image.Width, image.Height);
        using (Graphics g = Graphics.FromImage(grayImage))
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][] {
                new float[] { 0.3f, 0.3f, 0.3f, 0, 0 },
                new float[] { 0.59f, 0.59f, 0.59f, 0, 0 },
                new float[] { 0.11f, 0.11f, 0.11f, 0, 0 },
                new float[] { 0, 0, 0, 1, 0 },
                new float[] { 0, 0, 0, 0, 1 }
            });

            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(colorMatrix);
            g.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
        }
        return grayImage;
    }

    private static Bitmap RotateImage(Bitmap image, float angle)
    {
        Bitmap rotatedImage = new Bitmap(image.Width, image.Height);
        using (Graphics g = Graphics.FromImage(rotatedImage))
        {
            g.TranslateTransform((float)image.Width / 2, (float)image.Height / 2);
            g.RotateTransform(angle);
            g.TranslateTransform(-(float)image.Width / 2, -(float)image.Height / 2);
            g.DrawImage(image, new Point(0, 0));
        }
        return rotatedImage;
    }
}
