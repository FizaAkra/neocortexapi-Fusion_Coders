using System;
using System.IO;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace TextFromImages
{
    public class AdvancedImageProcessor : IImageProcessor
    {
        public string ProcessImage(string imagePath, string outputFolder)
        {
            // Create output directory if it doesn't exist
            Directory.CreateDirectory(outputFolder);

            // Generate a unique output filename
            string outputImagePath = Path.Combine(outputFolder,
                $"{Path.GetFileNameWithoutExtension(imagePath)}_processed{Path.GetExtension(imagePath)}");

            try
            {
                // Load the original image
                using (Image<Rgba32> originalImage = Image.Load<Rgba32>(imagePath))
                {
                    // Define transformation techniques
                    var transformations = new List<Action<Image<Rgba32>>>
                    {
                        // 1. No transformation (original image)
                        (img) => {
                            img.Mutate(x => x.Grayscale());
                            SaveProcessedImage(img, outputFolder, imagePath, "_original");
                        },

                        // 2. Rotate 90 degrees
                        (img) => {
                            img.Mutate(x => x
                                .Grayscale()
                                .Rotate(90f)
                            );
                            SaveProcessedImage(img, outputFolder, imagePath, "_rotate_90");
                        },

                        // 3. Rotate -90 degrees (270)
                        (img) => {
                            img.Mutate(x => x
                                .Grayscale()
                                .Rotate(-90f)
                            );
                            SaveProcessedImage(img, outputFolder, imagePath, "_rotate_-90");
                        },

                        // 4. Slight clockwise rotation
                        (img) => {
                            img.Mutate(x => x
                                .Grayscale()
                                .Rotate(15f)
                            );
                            SaveProcessedImage(img, outputFolder, imagePath, "_rotate_15");
                        },

                        // 5. Slight counter-clockwise rotation
                        (img) => {
                            img.Mutate(x => x
                                .Grayscale()
                                .Rotate(-15f)
                            );
                            SaveProcessedImage(img, outputFolder, imagePath, "_rotate_-15");
                        },

                        // 6. Horizontal shift
                        (img) => {
                            img.Mutate(x => x
                                .Grayscale()
                                .Crop(new Rectangle(
                                    img.Width / 10,  // Shift from left
                                    0,
                                    img.Width * 9 / 10,  // Crop right side
                                    img.Height
                                ))
                            );
                            SaveProcessedImage(img, outputFolder, imagePath, "_horizontal_shift");
                        },

                        // 7. Vertical shift
                        (img) => {
                            img.Mutate(x => x
                                .Grayscale()
                                .Crop(new Rectangle(
                                    0,
                                    img.Height / 10,  // Shift from top
                                    img.Width,
                                    img.Height * 9 / 10  // Crop bottom
                                ))
                            );
                            SaveProcessedImage(img, outputFolder, imagePath, "_vertical_shift");
                        },

                        // 8. Enhanced contrast 
                        (img) => {
                            img.Mutate(x => x
                                .Grayscale()
                                .Contrast(1.5f)  // Corrected contrast method
                            );
                            SaveProcessedImage(img, outputFolder, imagePath, "_enhanced_contrast");
                        },

                        // 9. Brightness adjustment
                        (img) => {
                            img.Mutate(x => x
                                .Grayscale()
                                .Brightness(1.2f)  // Corrected brightness method
                            );
                            SaveProcessedImage(img, outputFolder, imagePath, "_brightness_adjusted");
                        },

                        // 10. Slight skew
                        (img) => {
                            img.Mutate(x => x
                                .Grayscale()
                                .Rotate(5f)  // Slight rotation as skew alternative
                            );
                            SaveProcessedImage(img, outputFolder, imagePath, "_slight_skew");
                        }
                    };

                    // Apply all transformations
                    foreach (var transform in transformations)
                    {
                        using (Image<Rgba32> transformedImage = Image.Load<Rgba32>(imagePath))
                        {
                            transform(transformedImage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing image {imagePath}: {ex.Message}");
            }

            return outputImagePath;
        }

        // Helper method to save processed images with unique names
        private string SaveProcessedImage(Image<Rgba32> image, string outputFolder, string originalImagePath, string suffix)
        {
            string outputImagePath = Path.Combine(outputFolder,
                $"{Path.GetFileNameWithoutExtension(originalImagePath)}{suffix}{Path.GetExtension(originalImagePath)}");

            try
            {
                image.Save(outputImagePath);
                Console.WriteLine($"Saved processed image: {outputImagePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving image {outputImagePath}: {ex.Message}");
            }

            return outputImagePath;
        }
    }
}