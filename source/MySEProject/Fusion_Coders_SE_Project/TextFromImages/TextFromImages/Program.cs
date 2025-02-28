using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Tesseract;

class Program
{
    static string inputFolder = @"D:\Git\neocortexapi\neocortexapi-Fusion_Coders\source\MySEProject\Fusion_Coders_SE_Project\TextFromImages\TextFromImages\InputImages";
    static string outputFolder = @"D:\Git\neocortexapi\neocortexapi-Fusion_Coders\source\MySEProject\Fusion_Coders_SE_Project\TextFromImages\TextFromImages\OutputText";

    static void Main()
    {
        if (!Directory.Exists(outputFolder))
            Directory.CreateDirectory(outputFolder);

        string[] imageFiles = Directory.GetFiles(inputFolder, "*.jpg");
        foreach (string file in imageFiles)
        {
            ProcessImage(file);
        }
    }

    static void ProcessImage(string imagePath)
    {
        try
        {
            Bitmap originalImage = new Bitmap(imagePath);
            Bitmap rotatedImage = RotateImage(originalImage, 90);
            Bitmap shiftedImage = ShiftImage(rotatedImage, 10, 10);

            string extractedText = ExtractText(shiftedImage);
            string outputTextPath = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(imagePath) + ".txt");

            File.WriteAllText(outputTextPath, extractedText);
            Console.WriteLine($"Processed: {imagePath}, Output: {outputTextPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing {imagePath}: {ex.Message}");
        }
    }

    static string ExtractText(Bitmap image)
    {
        using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
        {
            using (var img = PixConverter.ToPix(image))
            {
                var result = engine.Process(img);
                return result.GetText();
            }
        }
    }

    static Bitmap RotateImage(Bitmap image, float angle)
    {
        Bitmap rotated = new Bitmap(image.Width, image.Height);
        using (Graphics g = Graphics.FromImage(rotated))
        {
            g.TranslateTransform(image.Width / 2, image.Height / 2);
            g.RotateTransform(angle);
            g.TranslateTransform(-image.Width / 2, -image.Height / 2);
            g.DrawImage(image, new Point(0, 0));
        }
        return rotated;
    }

    static Bitmap ShiftImage(Bitmap image, int shiftX, int shiftY)
    {
        Bitmap shifted = new Bitmap(image.Width, image.Height);
        using (Graphics g = Graphics.FromImage(shifted))
        {
            g.DrawImage(image, new Rectangle(shiftX, shiftY, image.Width, image.Height));
        }
        return shifted;
    }
}
