using System;
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
            _tessdataPath = tessdataPath;
        }

        public async Task<string> ExtractTextFromImage(string imagePath)
        {
            return await Task.Run(() =>
            {
                try
                {
                    using (var engine = new TesseractEngine(_tessdataPath, "eng", EngineMode.Default))
                    {
                        engine.SetVariable("preserve_interword_spaces", "1");

                        using (var img = Pix.LoadFromFile(imagePath))
                        {
                            using (var page = engine.Process(img))
                            {
                                string text = page.GetText();
                                float confidence = page.GetMeanConfidence();

                                Console.WriteLine($"Text extracted with confidence: {confidence:P}");

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

        private string PostProcessText(string text)
        {
            text = Regex.Replace(text, @"\s+", " ");
            text = text.Trim();
            return text;
        }
    }
}