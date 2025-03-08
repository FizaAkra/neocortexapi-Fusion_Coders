using System.Threading.Tasks;

namespace TextFromImages
{
    public interface IImageProcessor
    {
        string ProcessImage(string imagePath, string outputFolder);
    }

    public interface ITextExtractor
    {
        Task<string> ExtractTextFromImage(string imagePath);
    }
}