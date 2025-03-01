using System;
using System.Drawing;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string inputFolder = @"D:\Git\neocortexapi\neocortexapi-Fusion_Coders\source\MySEProject\Fusion_Coders_SE_Project\TextFromImages\InputImages";
        string outputFolder = @"D:\Git\neocortexapi\neocortexapi-Fusion_Coders\source\MySEProject\Fusion_Coders_SE_Project\TextFromImages\OutputText";

        // Create output folder if it doesn't exist
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
        }

        string terrasectApiKey = "your-api-key-here"; // Replace with your actual API key
        TextExtractor extractor = new TextExtractor(terrasectApiKey);

        foreach (var imagePath in Directory.GetFiles(inputFolder))
        {
            try
            {
                Bitmap img = new Bitmap(imagePath);

                // Apply transformations (rotation and shifting)
                img = ImageProcessor.ApplyTransformations(img, rotationAngle: 90, xShift: 5, yShift: 5);

                // Save the transformed image temporarily for text extraction
                string tempImagePath = Path.Combine(outputFolder, "temp_" + Path.GetFileName(imagePath));
                img.Save(tempImagePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                // Extract text from transformed image
                string extractedText = extractor.ExtractTextFromImage(tempImagePath);

                // Save extracted text to a text file
                string outputTextPath = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(imagePath) + "_extracted.txt");
                File.WriteAllText(outputTextPath, extractedText);

                Console.WriteLine($"Extracted text saved to: {outputTextPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing image {imagePath}: {ex.Message}");
            }
        }

        Console.WriteLine("Text extraction process completed.");
    }
}
