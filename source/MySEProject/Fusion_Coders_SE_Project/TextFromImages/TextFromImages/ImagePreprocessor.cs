using System;
using System.Drawing;

public class ImageProcessor
{
    // Static method for rotation
    public static Bitmap RotateImage(Bitmap img, float angle)
    {
        img.RotateFlip(RotateFlipType.RotateNoneFlipNone); // Apply rotation logic here
        img = new Bitmap(img);
        img.RotateFlip(RotateFlipType.Rotate90FlipNone); // Rotate by 90 degrees (you can change this)
        return img;
    }

    // Static method for shifting the image
    public static Bitmap ShiftImage(Bitmap img, int xShift, int yShift)
    {
        Bitmap shiftedImg = new Bitmap(img.Width, img.Height);
        using (Graphics g = Graphics.FromImage(shiftedImg))
        {
            g.Clear(Color.White);
            g.DrawImage(img, new Point(xShift, yShift));
        }
        return shiftedImg;
    }

    // Static method to apply multiple transformations (rotation + shift)
    public static Bitmap ApplyTransformations(Bitmap img, float rotationAngle, int xShift, int yShift)
    {
        img = RotateImage(img, rotationAngle);
        img = ShiftImage(img, xShift, yShift);
        return img;
    }
}
