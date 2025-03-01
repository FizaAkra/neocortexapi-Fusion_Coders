using System;
using System.Drawing;
using System.Drawing.Imaging; // Explicitly using System.Drawing.Imaging
using System.IO;
using Tesseract;

namespace TextFromImages
{
    public class OCRProcessor
    {
        private readonly TesseractEngine _engine;

        public OCRProcessor()
        {
            _engine = new TesseractEngine(@"C:\Program Files\Tesseract-OCR\tessdata", "eng", EngineMode.Default);
        }

        public string ExtractText(Bitmap image)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Explicitly specify System.Drawing.Imaging.ImageFormat.Png
                image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);

                using (var pix = Pix.LoadFromMemory(memoryStream.ToArray()))
                {
                    using (var page = _engine.Process(pix))
                    {
                        return page.GetText();
                    }
                }
            }
        }
    }
}
