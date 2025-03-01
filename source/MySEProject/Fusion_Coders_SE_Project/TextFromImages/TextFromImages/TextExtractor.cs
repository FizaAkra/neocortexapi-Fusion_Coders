using System;
using Terrasect.SDK; // Make sure the SDK is correctly referenced

public class TextExtractor
{
    private readonly string terrasectApiKey;

    // Constructor to initialize the API key
    public TextExtractor(string apiKey)
    {
        terrasectApiKey = apiKey; // Replace with your actual API key
    }

    // Method to extract text using Terrasect SDK
    public string ExtractTextFromImage(string imagePath)
    {
        // Initialize the API with the key
        TerrasectAPI terrasect = new TerrasectAPI(terrasectApiKey); // Correct TerrasectAPI class

        // Load image and extract text using the Terrasect SDK
        string extractedText = terrasect.ExtractText(imagePath); // Use Terrasect API method to extract text

        return extractedText;
    }
}
