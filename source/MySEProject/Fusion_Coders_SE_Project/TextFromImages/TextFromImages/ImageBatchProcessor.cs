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
            _imageProcessor = imageProcessor ?? throw new ArgumentNullException(nameof(imageProcessor));
            _textExtractor = textExtractor ?? throw new ArgumentNullException(nameof(textExtractor));
        }

        public async Task ProcessImagesInFolder(string inputFolder, string outputFolder, string extractedTextFolder)
        {
            if (!Directory.Exists(inputFolder))
            {
                throw new DirectoryNotFoundException($"Input folder not found: {inputFolder}");
            }

            // Create output directories if they don't exist
            Directory.CreateDirectory(outputFolder);
            Directory.CreateDirectory(extractedTextFolder);

            string[] imageExtensions = { "*.jpg", "*.jpeg", "*.png", "*.bmp", "*.tiff", "*.gif" };
            List<string> imageFiles = new List<string>();

            foreach (string extension in imageExtensions)
            {
                imageFiles.AddRange(Directory.GetFiles(inputFolder, extension));
            }

            Console.WriteLine($"Found {imageFiles.Count} images to process");

            if (imageFiles.Count == 0)
            {
                Console.WriteLine($"No image files found in {inputFolder}");
                return;
            }

            int successCount = 0;
            foreach (string imagePath in imageFiles)
            {
                Console.WriteLine();
                Console.WriteLine($"==================================================");
                Console.WriteLine($"Processing image {imageFiles.IndexOf(imagePath) + 1}/{imageFiles.Count}: {Path.GetFileName(imagePath)}");

                try
                {
                    // Process the image and get the path to the best processed version
                    Console.WriteLine("Step 1: Processing image...");
                    string processedImagePath = _imageProcessor.ProcessImage(imagePath, outputFolder);
                    Console.WriteLine($"Image processed: {processedImagePath}");

                    // Extract text from the processed image
                    Console.WriteLine("Step 2: Extracting text...");
                    string extractedText = await _textExtractor.ExtractTextFromImage(processedImagePath);

                    if (string.IsNullOrWhiteSpace(extractedText) || extractedText.StartsWith("Error extracting text"))
                    {
                        Console.WriteLine("Text extraction failed or returned empty result, trying with original image...");
                        extractedText = await _textExtractor.ExtractTextFromImage(imagePath);
                    }

                    Console.WriteLine($"Extracted text length: {extractedText?.Length ?? 0} characters");

                    // Save extracted text to file
                    string textFilePath = Path.Combine(extractedTextFolder, Path.GetFileNameWithoutExtension(imagePath) + ".txt");
                    File.WriteAllText(textFilePath, extractedText);

                    Console.WriteLine($"✅ Text extracted and saved to: {textFilePath}");
                    successCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error processing {Path.GetFileName(imagePath)}: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                }

                Console.WriteLine($"==================================================");
            }

            Console.WriteLine($"Successfully processed {successCount} out of {imageFiles.Count} images");
        }
    }
}