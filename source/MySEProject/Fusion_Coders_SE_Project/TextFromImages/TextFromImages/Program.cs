using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Tesseract;

namespace TextFromImages
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputFolderPath = @"D:\Fusion_Coders\neocortexapi-Fusion_Coders\source\MySEProject\Fusion_Coders_SE_Project\TextFromImages\TextFromImages\InputImages";
                string image1Name = "image1.jfif";
                string image2Name = "image2.png";

                string image1Path = Path.Combine(inputFolderPath, image1Name);
                string image2Path = Path.Combine(inputFolderPath, image2Name);

                if (!File.Exists(image1Path) || !File.Exists(image2Path))
                {
                    Console.WriteLine("One or both image files not found. Please check the paths.");
                    return;
                }

                ProcessImageWithMultipleMethods(image1Path, "Image 1");
                ProcessImageWithMultipleMethods(image2Path, "Image 2");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("\nPress any key to exit...");
                Console.ReadKey();
            }
        }

        static void ProcessImageWithMultipleMethods(string imagePath, string imageLabel)
        {
            Console.WriteLine($"\nProcessing {imageLabel}: {imagePath}");

            using (Bitmap originalImage = LoadImage(imagePath))
            {
                if (originalImage == null) return;

                // Try different rotations and preprocessing
                int[] rotationAngles = { 0, 90, 180, 270 };

                foreach (int angle in rotationAngles)
                {
                    Console.WriteLine($"\nTrying rotation angle: {angle} degrees");

                    using (Bitmap rotatedImage = RotateImage(originalImage, angle))
                    {
                        // Apply preprocessing and extract text
                        using (Bitmap preprocessedImage = AdvancedPreprocessing(rotatedImage))
                        {
                            string extractedText = ExtractTextUsingTesseract(preprocessedImage);

                            if (!string.IsNullOrWhiteSpace(extractedText))
                            {
                                string outputPath = Path.Combine(
                                    Path.GetDirectoryName(imagePath),
                                    $"Processed_Rotation{angle}_{Path.GetFileName(imagePath)}");

                                SaveImage(preprocessedImage, outputPath);

                                Console.WriteLine($"Extracted Text (Rotation {angle}°):");
                                Console.WriteLine(extractedText);
                                Console.WriteLine($"Processed image saved at: {outputPath}");
                            }
                        }
                    }
                }
            }
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

        static Bitmap RotateImage(Bitmap image, float angle)
        {
            try
            {
                // Create new bitmap with the same dimensions
                Bitmap rotatedBmp = new Bitmap(image.Width, image.Height);
                rotatedBmp.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                // Create graphics object for drawing
                using (Graphics g = Graphics.FromImage(rotatedBmp))
                {
                    // Set the rotation point to the center of the image
                    g.TranslateTransform(image.Width / 2.0f, image.Height / 2.0f);
                    g.RotateTransform(angle);
                    g.TranslateTransform(-image.Width / 2.0f, -image.Height / 2.0f);

                    // Set high quality rendering modes
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    // Draw the rotated image
                    g.DrawImage(image, new Point(0, 0));
                }

                return rotatedBmp;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error rotating image: {ex.Message}");
                return (Bitmap)image.Clone();
            }
        }

        static Bitmap AdvancedPreprocessing(Bitmap image)
        {
            try
            {
                using (Bitmap processedImage = new Bitmap(image.Width, image.Height))
                {
                    processedImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                    using (Graphics g = Graphics.FromImage(processedImage))
                    {
                        // Enhanced quality settings
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        // Apply contrast enhancement
                        using (ImageAttributes imageAttributes = new ImageAttributes())
                        {
                            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                            {
                                new float[] {1.5f, 0, 0, 0, 0},
                                new float[] {0, 1.5f, 0, 0, 0},
                                new float[] {0, 0, 1.5f, 0, 0},
                                new float[] {0, 0, 0, 1.0f, 0},
                                new float[] {-0.2f, -0.2f, -0.2f, 0, 1}
                            });

                            imageAttributes.SetColorMatrix(colorMatrix);

                            g.DrawImage(image,
                                new Rectangle(0, 0, image.Width, image.Height),
                                0, 0, image.Width, image.Height,
                                GraphicsUnit.Pixel,
                                imageAttributes);
                        }
                    }
                    return (Bitmap)processedImage.Clone();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in advanced preprocessing: {ex.Message}");
                return (Bitmap)image.Clone();
            }
        }

        static string ExtractTextUsingTesseract(Bitmap image)
        {
            try
            {
                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    engine.SetVariable("tessedit_char_whitelist", "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789.,!?-_'\"()[]{}:;/ ");

                    using (var page = engine.Process(image))
                    {
                        string text = page.GetText().Trim();
                        float confidence = page.GetMeanConfidence();

                        Console.WriteLine($"OCR Confidence: {confidence:P2}");
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

        static void SaveImage(Bitmap image, string outputPath)
        {
            try
            {
                using (EncoderParameters encoderParameters = new EncoderParameters(1))
                {
                    encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L);
                    ImageCodecInfo jpegEncoder = GetEncoder(System.Drawing.Imaging.ImageFormat.Jpeg);

                    if (jpegEncoder != null)
                    {
                        image.Save(outputPath, jpegEncoder, encoderParameters);
                    }
                    else
                    {
                        image.Save(outputPath, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving image: {ex.Message}");
            }
        }

        static ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}