using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TextFromImages
{
    public class ImageBatchProcessor
    {
        private readonly IImageProcessor _imageProcessor;
        private readonly ITextExtractor _textExtractor;

        public ImageBatchProcessor(IImageProcessor imageProcessor, ITextExtractor textExtractor)
        {
            _imageProcessor = imageProcessor;
            _textExtractor = textExtractor;
        }

        public async Task ProcessImagesInFolder(string inputFolder, string outputFolder, string extractedTextFolder)
        {
            string[] imageExtensions = { "*.jpg", "*.jpeg", "*.png", "*.bmp", "*.tiff", "*.gif" };
            List<string> imageFiles = new List<string>();

            foreach (string extension in imageExtensions)
            {
                imageFiles.AddRange(Directory.GetFiles(inputFolder, extension));
            }

            Console.WriteLine($"Found {imageFiles.Count} images to process");

            int successCount = 0;
            foreach (string imagePath in imageFiles)
            {
                Console.WriteLine($"Processing image {imageFiles.IndexOf(imagePath) + 1}/{imageFiles.Count}: {Path.GetFileName(imagePath)}");

                try
                {
                    string processedImagePath = _imageProcessor.ProcessImage(imagePath, outputFolder);
                    string extractedText = await _textExtractor.ExtractTextFromImage(processedImagePath);

                    string textFilePath = Path.Combine(extractedTextFolder, Path.GetFileNameWithoutExtension(imagePath) + ".txt");
                    File.WriteAllText(textFilePath, extractedText);

                    Console.WriteLine($"✅ Text extracted and saved to: {textFilePath}");
                    successCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error processing {Path.GetFileName(imagePath)}: {ex.Message}");
                }
            }

            Console.WriteLine($"Successfully processed {successCount} out of {imageFiles.Count} images");
        }
    }
}