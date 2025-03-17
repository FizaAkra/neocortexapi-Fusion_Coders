using System;
using System.IO;
using System.Threading.Tasks;
using Tesseract;
using System.Text.RegularExpressions;

namespace TextFromImages
{
    public class TesseractTextExtractor : ITextExtractor
    {
        private readonly string _tessdataPath;

        public TesseractTextExtractor(string tessdataPath)
        {
            // Make sure the tessdata path is absolute
            _tessdataPath = Path.GetFullPath(tessdataPath);

            // Verify the tessdata directory exists
            if (!Directory.Exists(_tessdataPath))
            {
                throw new DirectoryNotFoundException($"Tessdata directory not found at: {_tessdataPath}");
            }

            Console.WriteLine($"Using tessdata directory: {_tessdataPath}");

            // Verify that essential language files exist
            string engDataFile = Path.Combine(_tessdataPath, "eng.traineddata");
            if (!File.Exists(engDataFile))
            {
                throw new FileNotFoundException($"English training data not found at: {engDataFile}");
            }
        }

        public async Task<string> ExtractTextFromImage(string imagePath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    Console.WriteLine($"Attempting to extract text from: {imagePath}");

                    if (!File.Exists(imagePath))
                    {
                        throw new FileNotFoundException("Image file not found.", imagePath);
                    }

                    // Create engine with more detailed parameters
                    using (var engine = new TesseractEngine(_tessdataPath, "eng", EngineMode.Default))
                    {
                        // Set additional variables to improve text recognition
                        engine.SetVariable("preserve_interword_spaces", "1");
                        engine.SetVariable("tessedit_char_whitelist", "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.,;:!?()-_\"'`/\\&@#$%^*+=<>{}[]|~ ");
                        engine.SetVariable("user_defined_dpi", "300");

                        // Try to improve recognition quality
                        engine.SetVariable("tessdit_ocr_engine_mode", "3"); // Legacy + LSTM mode

                        // Load the image
                        Console.WriteLine("Loading image file...");
                        using (var img = Pix.LoadFromFile(imagePath))
                        {
                            if (img == null)
                            {
                                throw new Exception("Failed to load image with Tesseract Pix loader");
                            }

                            Console.WriteLine($"Processing image: {img.Width}x{img.Height} pixels");

                            using (var page = engine.Process(img))
                            {
                                string text = page.GetText();
                                float confidence = page.GetMeanConfidence();

                                Console.WriteLine($"Text extracted with confidence: {confidence:P}");

                                if (string.IsNullOrWhiteSpace(text))
                                {
                                    Console.WriteLine("Warning: Extracted text is empty");
                                    return "No text found in image.";
                                }

                                text = PostProcessText(text);
                                Console.WriteLine($"Extracted text sample: {text.Substring(0, Math.Min(50, text.Length))}...");

                                return text;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Tesseract error: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");

                    // Return more informative error message
                    return $"Error extracting text: {ex.Message}";
                }
            });
        }

        private string PostProcessText(string text)
        {
            // Remove excessive whitespace
            text = Regex.Replace(text, @"\s+", " ");

            // Replace common OCR errors
            text = text.Replace("l", "l").Replace("0", "O");

            // Remove non-printable characters
            text = Regex.Replace(text, @"[^\x20-\x7E]", "");

            return text.Trim();
        }
    }
}