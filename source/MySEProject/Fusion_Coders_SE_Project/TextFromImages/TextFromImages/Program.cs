using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

class Program
{
    static async Task Main()
    {
        string inputFolder = @"D:\Git\neocortexapi\neocortexapi-Fusion_Coders\source\MySEProject\Fusion_Coders_SE_Project\TextFromImages\TextFromImages\InputImages";
        string outputFolder = Path.Combine(inputFolder, "OutputImages");
        string extractedTextFolder = Path.Combine(inputFolder, "ExtractedText");

        // Ensure output directories exist
        Directory.CreateDirectory(outputFolder);
        Directory.CreateDirectory(extractedTextFolder);

        // Process each image
        foreach (string imagePath in Directory.GetFiles(inputFolder, "*.jpg"))
        {
            Console.WriteLine($"Processing: {Path.GetFileName(imagePath)}");

            // Apply preprocessing
            string processedImagePath = ProcessImage(imagePath, outputFolder);

            // Extract text
            string extractedText = await ExtractTextFromImage(processedImagePath);

            // Save extracted text
            string textFilePath = Path.Combine(extractedTextFolder, Path.GetFileNameWithoutExtension(imagePath) + ".txt");
            File.WriteAllText(textFilePath, extractedText);

            Console.WriteLine($"✅ Text extracted and saved: {textFilePath}\n");
        }

        Console.WriteLine("🎉 All images processed successfully!");
    }

    // 📌 Apply preprocessing: Convert to grayscale and rotate image
    static string ProcessImage(string imagePath, string outputFolder)
    {
        string outputImagePath = Path.Combine(outputFolder, Path.GetFileName(imagePath));

        using (Image<Rgba32> image = Image.Load<Rgba32>(imagePath))
        {
            // Convert to grayscale
            image.Mutate(x => x.Grayscale());

            // Rotate the image by 90 degrees
            image.Mutate(x => x.Rotate(90));

            // Save processed image
            image.Save(outputImagePath);
        }

        return outputImagePath;
    }

    // 📌 Function to send image to Terrasect API and extract text
    static async Task<string> ExtractTextFromImage(string imagePath)
    {
        try
        {
            using (var client = new HttpClient())
            using (var form = new MultipartFormDataContent())
            {
                form.Add(new ByteArrayContent(File.ReadAllBytes(imagePath)), "file", "image.jpg");

                HttpResponseMessage response = await client.PostAsync("VBvXX77MpPGeeD3smbnBwEZHAadRW4O4", form);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error extracting text: {ex.Message}");
            return "Error extracting text.";
        }
    }
}
