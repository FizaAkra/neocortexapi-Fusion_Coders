using System.Threading.Tasks;

namespace TextFromImages
{
    // Interface for image processing
    public interface IImageProcessor
    {
        string ProcessImage(string imagePath, string outputFolder);
    }

    // Interface for text extraction
    public interface ITextExtractor
    {
        Task<string> ExtractTextFromImage(string imagePath);
    }
}