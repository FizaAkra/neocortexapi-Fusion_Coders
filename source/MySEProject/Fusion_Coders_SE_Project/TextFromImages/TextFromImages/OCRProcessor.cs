using Tesseract;
using System.Drawing;

class OCRProcessor
{
    public static string ExtractText(Bitmap image)
    {
        using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
        {
            using (var img = PixConverter.ToPix(image))
            {
                var result = engine.Process(img);
                return result.GetText();
            }
        }
    }
}
