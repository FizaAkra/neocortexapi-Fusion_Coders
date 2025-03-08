using System;
using System.IO;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace TextFromImages
{
    public class ExperimentalImageProcessor : IImageProcessor
    {
        public string ProcessImage(string imagePath, string outputFolder)
        {
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException("Image file not found.", imagePath);
            }

            Directory.CreateDirectory(outputFolder);

            var techniques = new List<Tuple<string, Action<Image<Rgba32>>>>
            {
                new Tuple<string, Action<Image<Rgba32>>>("grayscale", img => img.Mutate(x => x.Grayscale())),
                new Tuple<string, Action<Image<Rgba32>>>("contrast", img => img.Mutate(x => x.Grayscale().Contrast(1.3f))),
                new Tuple<string, Action<Image<Rgba32>>>("rotate90", img => img.Mutate(x => x.Grayscale().Rotate(90))),
                new Tuple<string, Action<Image<Rgba32>>>("rotate270", img => img.Mutate(x => x.Grayscale().Rotate(270))),
                new Tuple<string, Action<Image<Rgba32>>>("resize", img => img.Mutate(x => x.Grayscale().Resize(img.Width * 2, img.Height * 2)))
            };

            string bestOutputPath = Path.Combine(outputFolder, $"{Path.GetFileNameWithoutExtension(imagePath)}_best{Path.GetExtension(imagePath)}");

            foreach (var technique in techniques)
            {
                string currentOutputPath = Path.Combine(outputFolder, $"{Path.GetFileNameWithoutExtension(imagePath)}_{technique.Item1}{Path.GetExtension(imagePath)}");

                using (Image<Rgba32> image = Image.Load<Rgba32>(imagePath))
                {
                    technique.Item2(image);
                    image.Save(currentOutputPath);
                }

                File.Copy(currentOutputPath, bestOutputPath, true);
            }

            return bestOutputPath;
        }
    }
}