using System;
using System.Drawing; // For Bitmap
using System.IO;
using Tesseract; // Add this using statement

namespace TextFromImages
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define input folder and image file names
            string inputFolderPath = @"D:\Git\neocortexapi\neocortexapi-Fusion_Coders\source\MySEProject\Fusion_Coders_SE_Project\TextFromImages\TextFromImages\InputImages";
            string image1Name = "image1.jpg";
            string image2Name = "image2.png";

            // Create full paths to images
            string image1Path = Path.Combine(inputFolderPath, image1Name);
            string image2Path = Path.Combine(inputFolderPath, image2Name);

            // Ensure images exist
            if (!File.Exists(image1Path) || !File.Exists(image2Path))
            {
                Console.WriteLine("One or both image files were not found. Please check the paths.");
                return;
            }

            // Process images
            ProcessImage(image1Path);
            ProcessImage(image2Path);
        }

        static void ProcessImage(string imagePath)
        {
            Console.WriteLine($"Processing image: {imagePath}");

            // Step 1: Load the image
            Bitmap image = LoadImage(imagePath);
            if (image == null)
            {
                Console.WriteLine("Failed to load image.");
                return;
            }

            // Step 2: Preprocess the image (e.g., rotation, contrast adjustment)
            Bitmap preprocessedImage = PreprocessImage(image);

            // Step 3: Extract text using Tesseract OCR
            string extractedText = ExtractTextUsingTesseract(preprocessedImage);

            // Output the extracted text
            Console.WriteLine("Extracted Text:");
            Console.WriteLine(extractedText);

            // Save the preprocessed image for comparison if needed
            string preprocessedImagePath = Path.Combine(Path.GetDirectoryName(imagePath), $"Preprocessed_{Path.GetFileName(imagePath)}");
            SaveImage(preprocessedImage, preprocessedImagePath);
            Console.WriteLine($"Preprocessed image saved at: {preprocessedImagePath}");
        }

        static Bitmap LoadImage(string imagePath)
        {
            try
            {
                return new Bitmap(imagePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image: {ex.Message}");
                return null;
            }
        }

        static Bitmap PreprocessImage(Bitmap image)
        {
            try
            {
                // Example: Apply basic preprocessing steps (adjust as needed)
                using (Graphics g = Graphics.FromImage(image))
                {
                    // Rotate 90 degrees
                    image.RotateFlip(RotateFlipType.Rotate90FlipNone);

                    // Additional transformations can be added here
                }
                return image;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during preprocessing: {ex.Message}");
                return null;
            }
        }

        static string ExtractTextUsingTesseract(Bitmap image)
        {
            try
            {
                // Use Tesseract OCR to extract text
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    using (var page = engine.Process(image))
                    {
                        return page.GetText();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during OCR extraction: {ex.Message}");
                return string.Empty;
            }
        }

        static void SaveImage(Bitmap image, string outputPath)
        {
            try
            {
                image.Save(outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving image: {ex.Message}");
            }
        }
    }
}