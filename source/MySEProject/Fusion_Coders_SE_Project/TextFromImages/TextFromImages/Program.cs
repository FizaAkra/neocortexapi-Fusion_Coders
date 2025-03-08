using System;
using System.IO;
using System.Threading.Tasks;

namespace TextFromImages
{
    class Program
    {
        static async Task Main()
        {
            string inputFolder = @"D:\Git\neocortexapi\neocortexapi-Fusion_Coders\source\MySEProject\Fusion_Coders_SE_Project\TextFromImages\TextFromImages\InputImages";
            string outputFolder = Path.Combine(inputFolder, "OutputImages");
            string extractedTextFolder = Path.Combine(inputFolder, "ExtractedText");

            Directory.CreateDirectory(outputFolder);
            Directory.CreateDirectory(extractedTextFolder);

            ImageBatchProcessor batchProcessor = new ImageBatchProcessor(
                new AdvancedImageProcessor(),
                new TesseractTextExtractor("tessdata")
            );

            await batchProcessor.ProcessImagesInFolder(inputFolder, outputFolder, extractedTextFolder);

            Console.WriteLine("🎉 All images processed successfully!");
            Console.ReadKey();
        }
    }
}