using System;
using System.Threading.Tasks;
using Tesseract;
using System.Text.RegularExpressions;

namespace TextFromImages
{
    // Tesseract OCR implementation for text extraction
    public class TesseractTextExtractor : ITextExtractor
    {
        private readonly string _tessdataPath;

        public TesseractTextExtractor(string tessdataPath)
        {
            _tessdataPath = tessdataPath;
        }

        public async Task<string> ExtractTextFromImage(string imagePath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    // Initialize Tesseract engine
                    using (var engine = new TesseractEngine(_tessdataPath, "eng", EngineMode.Default))
                    {
                        // Set Tesseract variables for better results
                        engine.SetVariable("preserve_interword_spaces", "1");

                        using (var img = Pix.LoadFromFile(imagePath))
                        {
                            using (var page = engine.Process(img))
                            {
                                string text = page.GetText();
                                float confidence = page.GetMeanConfidence();

                                Console.WriteLine($"Text extracted with confidence: {confidence:P}");

                                // Post-process the extracted text
                                text = PostProcessText(text);

                                return text;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Tesseract error: {ex.Message}");
                    return "Error extracting text.";
                }
            });
        }

        // Post-process the extracted text to improve quality
        private string PostProcessText(string text)
        {
            // Remove excessive whitespace
            text = Regex.Replace(text, @"\s+", " ");

            // Trim the text
            text = text.Trim();

            return text;
        }
    }
}