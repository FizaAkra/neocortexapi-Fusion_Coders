using System;
using System.IO;
using System.Threading.Tasks;

namespace TextFromImages
{
    class Program
    {
        static async Task Main()
        {
            string inputFolder = @"D:\fusionProject\neocortexapi-Fusion_Coders\source\MySEProject\Fusion_Coders_SE_Project\TextFromImages\TextFromImages\InputImages";
            string outputFolder = Path.Combine(inputFolder, "OutputImages");
            string extractedTextFolder = Path.Combine(inputFolder, "ExtractedText");

            // Ensure output directories exist
            Directory.CreateDirectory(outputFolder);
            Directory.CreateDirectory(extractedTextFolder);

            // Create batch processor instance
            ImageBatchProcessor batchProcessor = new ImageBatchProcessor(
                new AdvancedImageProcessor(),
                new TesseractTextExtractor("tessdata") // Path to tessdata folder
            );

            // Process all images in the folder
            await batchProcessor.ProcessImagesInFolder(inputFolder, outputFolder, extractedTextFolder);

            Console.WriteLine("🎉 All images processed successfully!");
            Console.ReadKey(); // Keep console window open
        }
    }
}