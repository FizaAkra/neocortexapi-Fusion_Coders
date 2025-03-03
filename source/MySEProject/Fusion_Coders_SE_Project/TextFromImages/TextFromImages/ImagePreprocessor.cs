using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

public class ImageProcessor
{
    // Rotate image by specified angle
    public static Bitmap RotateImage(Bitmap img, float angle)
    {
        Bitmap rotated = new Bitmap(img.Width, img.Height);
        using (Graphics g = Graphics.FromImage(rotated))
        {
            g.TranslateTransform(img.Width / 2, img.Height / 2);
            g.RotateTransform(angle);
            g.TranslateTransform(-img.Width / 2, -img.Height / 2);
            g.DrawImage(img, new Point(0, 0));
        }
        return rotated;
    }

    // Apply other transformations (e.g., shifting, grayscale)
    public static Bitmap ApplyTransformations(Bitmap img)
    {
        Bitmap transformed = new Bitmap(img);
        using (Graphics g = Graphics.FromImage(transformed))
        {
            g.DrawImage(img, 0, 0);
        }
        return transformed;
    }

    // Save preprocessed image
    public static string SaveImage(Bitmap img, string originalPath, string suffix)
    {
        string newPath = Path.Combine(Path.GetDirectoryName(originalPath), Path.GetFileNameWithoutExtension(originalPath) + suffix + ".jpg");
        img.Save(newPath, ImageFormat.Jpeg);
        return newPath;
    }
}
