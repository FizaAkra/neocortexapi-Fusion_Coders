using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Tesseract;

namespace TextFromImages
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                ProcessImages();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static void ProcessImages()
        {
            string inputFolderPath = @"D:\Git\neocortexapi\neocortexapi-Fusion_Coders\source\MySEProject\Fusion_Coders_SE_Project\TextFromImages\TextFromImages\InputImages";
            string[] imageNames = { "image1.jpg", "image2.jpg" };

            foreach (string imageName in imageNames)
            {
                string imagePath = Path.Combine(inputFolderPath, imageName);
                if (!File.Exists(imagePath))
                {
                    Console.WriteLine($"Image file not found: {imageName}");
                    continue;
                }

                Console.WriteLine($"\nProcessing image: {imageName}");
                Console.WriteLine("==================================");

                ProcessImageWithVariations(imagePath);
            }
        }

        private static void ProcessImageWithVariations(string imagePath)
        {
            string outputDir = Path.Combine(Path.GetDirectoryName(imagePath) ?? string.Empty, "Preprocessed");
            Directory.CreateDirectory(outputDir);

            using (Bitmap originalImage = LoadImage(imagePath))
            {
                if (originalImage == null) return;

                ProcessOriginal(originalImage, outputDir, imagePath);
                ProcessRotated(originalImage, outputDir, imagePath);
                ProcessEnhanced(originalImage, outputDir, imagePath);
                ProcessRotatedAndEnhanced(originalImage, outputDir, imagePath);
            }
        }

        private static void ProcessOriginal(Bitmap originalImage, string outputDir, string imagePath)
        {
            using (Bitmap processedImage = new Bitmap(originalImage))
            {
                ProcessAndSaveVariation(processedImage, "Original", outputDir, imagePath);
            }
        }

        private static void ProcessRotated(Bitmap originalImage, string outputDir, string imagePath)
        {
            using (Bitmap rotated = RotateImage(originalImage))
            {
                if (rotated != null)
                {
                    ProcessAndSaveVariation(rotated, "Rotated", outputDir, imagePath);
                }
            }
        }

        private static void ProcessEnhanced(Bitmap originalImage, string outputDir, string imagePath)
        {
            using (Bitmap enhanced = EnhanceImage(originalImage))
            {
                if (enhanced != null)
                {
                    ProcessAndSaveVariation(enhanced, "Enhanced", outputDir, imagePath);
                }
            }
        }

        private static void ProcessRotatedAndEnhanced(Bitmap originalImage, string outputDir, string imagePath)
        {
            using (Bitmap rotated = RotateImage(originalImage))
            {
                if (rotated != null)
                {
                    using (Bitmap enhanced = EnhanceImage(rotated))
                    {
                        if (enhanced != null)
                        {
                            ProcessAndSaveVariation(enhanced, "RotatedAndEnhanced", outputDir, imagePath);
                        }
                    }
                }
            }
        }

        private static void ProcessAndSaveVariation(Bitmap image, string variationName, string outputDir, string imagePath)
        {
            Console.WriteLine($"\nTrying {variationName} preprocessing:");
            string outputPath = Path.Combine(outputDir, $"{variationName}_{Path.GetFileName(imagePath)}");
            SaveImage(image, outputPath);
            Console.WriteLine($"Saved preprocessed image: {outputPath}");

            string extractedText = ExtractTextUsingTesseract(image);
            Console.WriteLine("Extracted Text:");
            Console.WriteLine(extractedText);
            Console.WriteLine($"Character count: {extractedText.Length}");
        }

        private static Bitmap LoadImage(string imagePath)
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

        private static Bitmap RotateImage(Bitmap sourceImage)
        {
            try
            {
                Bitmap rotated = new Bitmap(sourceImage.Width, sourceImage.Height);
                try
                {
                    using (Graphics g = Graphics.FromImage(rotated))
                    {
                        g.Clear(Color.White);
                        g.DrawImage(sourceImage, 0, 0, sourceImage.Width, sourceImage.Height);
                    }
                    rotated.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    return rotated;
                }
                catch
                {
                    rotated.Dispose();
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in rotation: {ex.Message}");
                return null;
            }
        }

        private static Bitmap EnhanceImage(Bitmap sourceImage)
        {
            try
            {
                Bitmap enhanced = new Bitmap(sourceImage.Width, sourceImage.Height);
                try
                {
                    using (Graphics g = Graphics.FromImage(enhanced))
                    {
                        using (ImageAttributes attributes = new ImageAttributes())
                        {
                            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                            {
                                new float[] {1.5f, 0, 0, 0, 0},
                                new float[] {0, 1.5f, 0, 0, 0},
                                new float[] {0, 0, 1.5f, 0, 0},
                                new float[] {0, 0, 0, 1, 0},
                                new float[] {-0.2f, -0.2f, -0.2f, 0, 1}
                            });

                            attributes.SetColorMatrix(colorMatrix);
                            g.DrawImage(sourceImage,
                                new Rectangle(0, 0, sourceImage.Width, sourceImage.Height),
                                0, 0, sourceImage.Width, sourceImage.Height,
                                GraphicsUnit.Pixel,
                                attributes);
                        }
                    }
                    return enhanced;
                }
                catch
                {
                    enhanced.Dispose();
                    throw;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in enhancement: {ex.Message}");
                return null;
            }
        }

        private static string ExtractTextUsingTesseract(Bitmap image)
        {
            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    engine.SetVariable("tessedit_char_whitelist",
                        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.,!?-_'\"()[]{}:;/ ");

                    using (var page = engine.Process(image))
                    {
                        float confidence = page.GetMeanConfidence();
                        string text = page.GetText()?.Trim() ?? string.Empty;
                        Console.WriteLine($"Confidence: {confidence:P2}");
                        return text;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OCR extraction: {ex.Message}");
                return string.Empty;
            }
        }

        private static void SaveImage(Bitmap image, string outputPath)
        {
            try
            {
                if (image != null)
                {
                    using (var stream = new FileStream(outputPath, FileMode.Create))
                    {
                        image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving image: {ex.Message}");
            }
        }
    }
}