using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace TextFromImages
{
    public class AdvancedImageProcessor : IImageProcessor
    {
        public string ProcessImage(string imagePath, string outputFolder)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("Image file not found.", imagePath);
            }

            // Ensure the output folder exists
            Directory.CreateDirectory(outputFolder);

            // Generate a unique output filename
            string outputImagePath = Path.Combine(outputFolder,
                $"{Path.GetFileNameWithoutExtension(imagePath)}_processed{Path.GetExtension(imagePath)}");

            try
            {
                // Load the original image
                using (Image<Rgba32> originalImage = Image.Load<Rgba32>(imagePath))
                {
                    // Apply transformations (e.g., grayscale)
                    originalImage.Mutate(x => x.Grayscale());

                    // Save the processed image
                    originalImage.Save(outputImagePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing image {imagePath}: {ex.Message}");
                throw; // Re-throw the exception to fail the test
            }

            return outputImagePath;
        }
    }
}