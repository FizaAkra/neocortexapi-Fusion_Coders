using System;
using System.Drawing;  // Required for Image class
using Tesseract;

class Program
{
    static void Main(string[] args)
    {
        string imagePath = @"D:\Git\neocortexapi\neocortexapi-Fusion_Coders\source\MySEProject\Fusion_Coders_SE_Project\TextFromImages\TextFromImages\InputImages\download.jpeg"; // Path to your image file
        string tessdataPath = @"D:\Git\neocortexapi\neocortexapi-Fusion_Coders\source\MySEProject\Fusion_Coders_SE_Project\TextFromImages\TextFromImages\tessdata"; // Correct path to the tessdata folder

        // Initialize Tesseract engine
        using (var engine = new TesseractEngine(tessdataPath, "eng", EngineMode.Default))
        {
            // Load image from file
            using (var img = Pix.LoadFromFile(imagePath))
            {
                // Perform OCR (text extraction)
                using (var page = engine.Process(img))
                {
                    // Output the recognized text
                    string text = page.GetText();
                    Console.WriteLine("Extracted Text: ");
                    Console.WriteLine(text);
                }
            }
        }
    }
}
