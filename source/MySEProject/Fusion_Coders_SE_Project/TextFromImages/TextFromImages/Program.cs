using System;
using System.IO;
using Tesseract;

namespace TextFromImages
{
    class Program
    {
        static void Main(string[] args)
        {
            // Path to the InputImages folder
            string inputFolder = @"D:\Git\neocortexapi\neocortexapi-Fusion_Coders\source\MySEProject\Fusion_Coders_SE_Project\TextFromImages\TextFromImages\InputImages";

            // Check if the folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.WriteLine("Input folder does not exist. Please check the path.");
                return;
            }

            // Get all image files from the folder
            string[] imageFiles = Directory.GetFiles(inputFolder, "*.jpg");

            if (imageFiles.Length == 0)
            {
                Console.WriteLine("No image files found in the InputImages folder.");
                return;
            }

            // Process each image
            foreach (var imageFile in imageFiles)
            {
                Console.WriteLine($"Processing: {Path.GetFileName(imageFile)}");

                // Original Image
                string originalText = ExtractText(imageFile);
                Console.WriteLine("Original Text:\n" + originalText);

                // Rotate Image (90 degrees)
                string rotatedImagePath = ImagePreprocessor.RotateImage(imageFile, 90);
                string rotatedText = ExtractText(rotatedImagePath);
                Console.WriteLine("Rotated Text:\n" + rotatedText);

                // Shift Image (50 pixels right, 50 pixels down)
                string shiftedImagePath = ImagePreprocessor.ShiftImage(imageFile, 50, 50);
                string shiftedText = ExtractText(shiftedImagePath);
                Console.WriteLine("Shifted Text:\n" + shiftedText);

                // Resize Image (50% smaller)
                string resizedImagePath = ImagePreprocessor.ResizeImage(imageFile, 0.5f);
                string resizedText = ExtractText(resizedImagePath);
                Console.WriteLine("Resized Text:\n" + resizedText);

                // Convert to Grayscale
                string grayscaleImagePath = ImagePreprocessor.ConvertToGrayscale(imageFile);
                string grayscaleText = ExtractText(grayscaleImagePath);
                Console.WriteLine("Grayscale Text:\n" + grayscaleText);

                // Clean up temporary files
                File.Delete(rotatedImagePath);
                File.Delete(shiftedImagePath);
                File.Delete(resizedImagePath);
                File.Delete(grayscaleImagePath);

                Console.WriteLine("----------------------------------------");
            }
        }

        // Method to extract text from an image using Tesseract OCR
        static string ExtractText(string imagePath)
        {
            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    using (var img = Pix.LoadFromFile(imagePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            return page.GetText();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return $"Error extracting text: {ex.Message}";
            }
        }
    }
}