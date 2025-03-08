using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Moq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace TextFromImages.Tests
{
    public class AdvancedImageProcessorTests
    {
        private readonly string _imagePath = @"D:\Git\neocortexapi\neocortexapi-Fusion_Coders\source\MySEProject\Fusion_Coders_SE_Project\TextFromImages\TextFromImages\InputImages\test_image.jpg";

        [Fact]
        public void ProcessImage_ShouldSaveProcessedImages()
        {
            // Arrange
            var processor = new AdvancedImageProcessor();
            string outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "output");

            // Act
            var result = processor.ProcessImage(_imagePath, outputFolder);

            // Assert
            Assert.True(Directory.Exists(outputFolder)); // Ensure the output folder exists

            // Check that at least one processed image exists in the output folder
            var processedImages = Directory.GetFiles(outputFolder, "*_processed.jpg");
            Assert.True(processedImages.Length > 0, "No processed images were found in the output folder.");
        }

        [Fact]
        public void ProcessImage_ShouldHandleInvalidImagePath()
        {
            // Arrange
            var processor = new AdvancedImageProcessor();
            string invalidImagePath = "invalid_path.jpg"; // This file does not exist
            string outputFolder = "output";

            // Act & Assert
            var exception = Assert.Throws<FileNotFoundException>(() => processor.ProcessImage(invalidImagePath, outputFolder));
            Assert.Contains("Image file not found", exception.Message);
        }
    }

    public class ExperimentalImageProcessorTests
    {
        private readonly string _imagePath = @"D:\Git\neocortexapi\neocortexapi-Fusion_Coders\source\MySEProject\Fusion_Coders_SE_Project\TextFromImages\TextFromImages\InputImages\test_image.jpg";

        [Fact]
        public void ProcessImage_ShouldApplyAllTechniques()
        {
            // Arrange
            var processor = new ExperimentalImageProcessor();
            string outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "output");

            // Act
            var result = processor.ProcessImage(_imagePath, outputFolder);

            // Assert
            Assert.True(File.Exists(result)); // Ensure the processed image file exists
            Assert.True(Directory.GetFiles(outputFolder).Length > 0); // Ensure the output folder is not empty
        }
    }

    public class ImageBatchProcessorTests
    {
        [Fact]
        public async Task ProcessImagesInFolder_ShouldProcessAllImages()
        {
            // Arrange
            var mockImageProcessor = new Mock<IImageProcessor>();
            var mockTextExtractor = new Mock<ITextExtractor>();

            mockImageProcessor.Setup(x => x.ProcessImage(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("processed_image.jpg");

            mockTextExtractor.Setup(x => x.ExtractTextFromImage(It.IsAny<string>()))
                .ReturnsAsync("extracted text");

            var batchProcessor = new ImageBatchProcessor(mockImageProcessor.Object, mockTextExtractor.Object);
            string inputFolder = @"D:\Git\neocortexapi\neocortexapi-Fusion_Coders\source\MySEProject\Fusion_Coders_SE_Project\TextFromImages\TextFromImages\InputImages";
            string outputFolder = Path.Combine(Directory.GetCurrentDirectory(), "output");
            string extractedTextFolder = Path.Combine(Directory.GetCurrentDirectory(), "extracted_text");

            // Create directories if they don't exist
            Directory.CreateDirectory(outputFolder);
            Directory.CreateDirectory(extractedTextFolder);

            // Act
            await batchProcessor.ProcessImagesInFolder(inputFolder, outputFolder, extractedTextFolder);

            // Assert
            mockImageProcessor.Verify(x => x.ProcessImage(It.IsAny<string>(), It.IsAny<string>()), Times.AtLeastOnce);
            mockTextExtractor.Verify(x => x.ExtractTextFromImage(It.IsAny<string>()), Times.AtLeastOnce);
        }
    }
}