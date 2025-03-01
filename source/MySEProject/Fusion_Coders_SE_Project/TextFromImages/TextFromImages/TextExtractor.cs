using System;
using Terrasect.SDK; // Ensure you have the correct SDK referenced

public class TextExtractor
{
    private readonly string terrasectApiKey;

    public TextExtractor(string apiKey)
    {
        terrasectApiKey = apiKey;
    }

    public string ExtractTextFromImage(string imagePath)
    {
        try
        {
            TerrasectAPI terrasect = new TerrasectAPI(terrasectApiKey);
            return terrasect.ExtractText(imagePath);
        }
        catch (Exception ex)
        {
            Console.WriteLine("OCR Extraction Failed: " + ex.Message);
            return null;
        }
    }
}
