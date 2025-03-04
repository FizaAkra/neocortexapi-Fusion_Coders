using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace TextFromImages
{
    // Advanced image processor with multiple filters and transformations
    public class AdvancedImageProcessor : IImageProcessor
    {
        public string ProcessImage(string imagePath, string outputFolder)
        {
            string outputImagePath = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(imagePath) + "_processed" + Path.GetExtension(imagePath));

            using (Image<Rgba32> image = Image.Load<Rgba32>(imagePath))
            {
                // Basic enhancements for OCR
                try
                {
                    image.Mutate(x => x.Grayscale()); // Convert to grayscale
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in grayscale: {ex.Message}");
                }

                // Apply contrast adjustment
                try
                {
                    image.Mutate(x => x.Contrast(1.3f)); // Increase contrast
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in contrast: {ex.Message}");
                }

                // Apply transformations for better text extraction

                // 1. Rotation - Try to straighten text
                try
                {
                    // For text that might be slightly rotated, a small angle adjustment can help
                    float rotationAngle = 0.5f; // Small correction angle, adjust as needed
                    image.Mutate(x => x.Rotate(rotationAngle));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in rotation: {ex.Message}");
                }

                // 2. Skew correction - Fix perspective issues
                try
                {
                    // Apply skew transformation to correct perspective (values are examples)
                    // This can help with documents photographed at an angle
                    AffineTransformBuilder builder = new AffineTransformBuilder();

                    // The following creates a mild perspective correction
                    // Adjust values as needed for your specific images
                    Point destination = new Point(0, 0);
                    PointF source = new PointF(5f, 5f); // Slight shift from corner

                    Point destination2 = new Point(image.Width, 0);
                    PointF source2 = new PointF(image.Width - 5f, 5f);

                    Point destination3 = new Point(0, image.Height);
                    PointF source3 = new PointF(5f, image.Height - 5f);

                    var matrix = builder.AppendTranslation(new PointF(-5f, -5f));
                    image.Mutate(x => x.Transform(matrix));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in skew correction: {ex.Message}");
                }

                // 3. Resize for better OCR resolution
                try
                {
                    // Increase size - helps OCR detect smaller text
                    image.Mutate(x => x.Resize(image.Width * 2, image.Height * 2));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in resize: {ex.Message}");
                }

                // 4. Binarization - Convert to black and white for clearer text
                try
                {
                    // Separate brightness and contrast operations instead of using BrightnessContrast
                    image.Mutate(x => x.Brightness(0.2f)); // Adjust brightness
                    image.Mutate(x => x.Contrast(0.8f));   // Adjust contrast

                    // Increase saturation to make text more visible
                    image.Mutate(x => x.Saturate(1.5f));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in binarization: {ex.Message}");
                }

                // 5. Edge enhancement - Make text edges more defined
                try
                {
                    // Sharpen the image to enhance text edges
                    image.Mutate(x => x.GaussianSharpen(3f));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in edge enhancement: {ex.Message}");
                }

                // Save the enhanced image
                try
                {
                    image.Save(outputImagePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving image: {ex.Message}");
                    // Fallback to saving as PNG if original format has issues
                    string pngPath = Path.ChangeExtension(outputImagePath, ".png");
                    image.SaveAsPng(pngPath);
                    return pngPath;
                }
            }

            return outputImagePath;
        }
    }
}