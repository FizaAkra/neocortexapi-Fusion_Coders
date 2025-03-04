using System;
using System.IO;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace TextFromImages
{
    // Optional: Add an experimental class that tries multiple processing techniques
    public class ExperimentalImageProcessor : IImageProcessor
    {
        public string ProcessImage(string imagePath, string outputFolder)
        {
            // Create a list of processing techniques to try
            var techniques = new List<Tuple<string, Action<Image<Rgba32>>>>();

            // Add basic grayscale
            techniques.Add(new Tuple<string, Action<Image<Rgba32>>>(
                "grayscale",
                img => img.Mutate(x => x.Grayscale())
            ));

            // Add grayscale with contrast
            techniques.Add(new Tuple<string, Action<Image<Rgba32>>>(
                "contrast",
                img => {
                    img.Mutate(x => x.Grayscale());
                    try { img.Mutate(x => x.Contrast(1.3f)); } catch { }
                }
            ));

            // Add 90 degree rotation
            techniques.Add(new Tuple<string, Action<Image<Rgba32>>>(
                "rotate90",
                img => {
                    img.Mutate(x => x.Grayscale());
                    try { img.Mutate(x => x.Rotate(90)); } catch { }
                }
            ));

            // Add 270 degree rotation (or -90)
            techniques.Add(new Tuple<string, Action<Image<Rgba32>>>(
                "rotate270",
                img => {
                    img.Mutate(x => x.Grayscale());
                    try { img.Mutate(x => x.Rotate(270)); } catch { }
                }
            ));

            // Add resize
            techniques.Add(new Tuple<string, Action<Image<Rgba32>>>(
                "resize",
                img => {
                    img.Mutate(x => x.Grayscale());
                    try { img.Mutate(x => x.Resize(img.Width * 2, img.Height * 2)); } catch { }
                }
            ));

            // Process the image with each technique and save it
            string bestOutputPath = Path.Combine(outputFolder,
                Path.GetFileNameWithoutExtension(imagePath) + "_best" + Path.GetExtension(imagePath));

            foreach (var technique in techniques)
            {
                string currentOutputPath = Path.Combine(outputFolder,
                    $"{Path.GetFileNameWithoutExtension(imagePath)}_{technique.Item1}{Path.GetExtension(imagePath)}");

                try
                {
                    using (Image<Rgba32> image = Image.Load<Rgba32>(imagePath))
                    {
                        // Apply current technique
                        technique.Item2(image);

                        // Save with this technique
                        image.Save(currentOutputPath);
                    }

                    // In a real implementation, we would test which technique produces the best OCR result
                    // For now, we'll just use the last technique as our "best" output
                    File.Copy(currentOutputPath, bestOutputPath, true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error applying {technique.Item1}: {ex.Message}");
                }
            }

            return bestOutputPath;
        }
    }
}