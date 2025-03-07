using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

namespace TextFromImages.Tests
{
    [TestClass]
    public class TesseractTextExtractorTests
    {
        private string _tessdataPath = string.Empty;
        private string _testImagePath = string.Empty;

        [TestInitialize]
        public void Setup()
        {
            // Setup test paths - adjust as needed for your environment
            _tessdataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");
            _testImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestImages", "sample.png");

            // Create directories if they don't exist
            string? testImageDir = Path.GetDirectoryName(_testImagePath);
            if (!string.IsNullOrEmpty(testImageDir))
            {
                Directory.CreateDirectory(testImageDir);
            }
        }

        [TestMethod]
        public async Task ExtractTextFromImage_ValidImage_ReturnsText()
        {
            // Skip if test image doesn't exist (for CI environments without test images)
            if (!File.Exists(_testImagePath))
            {
                Assert.Inconclusive("Test image not found. Skipping test.");
                return;
            }

            // Arrange
            var extractor = new TesseractTextExtractor(_tessdataPath);

            // Act
            string result = await extractor.ExtractTextFromImage(_testImagePath);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(result), "Extracted text should not be empty");
        }

        [TestMethod]
        public async Task ExtractTextFromImage_InvalidImage_ReturnsErrorMessage()
        {
            // Arrange
            var extractor = new TesseractTextExtractor(_tessdataPath);
            string invalidImagePath = "nonexistent.png";

            // Act
            string result = await extractor.ExtractTextFromImage(invalidImagePath);

            // Assert
            Assert.AreEqual("Error extracting text.", result);
        }

        [TestMethod]
        public void Constructor_WithValidPath_CreatesInstance()
        {
            // Arrange & Act
            var extractor = new TesseractTextExtractor(_tessdataPath);

            // Assert
            Assert.IsNotNull(extractor);
        }
    }

    [TestClass]
    public class AdvancedImageProcessorTests
    {
        private string _testImagePath = string.Empty;
        private string _outputFolder = string.Empty;

        [TestInitialize]
        public void Setup()
        {
            // Setup test paths
            _testImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestImages", "sample.png");
            _outputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestOutput");

            // Ensure directories exist
            string? testImageDir = Path.GetDirectoryName(_testImagePath);
            if (!string.IsNullOrEmpty(testImageDir))
            {
                Directory.CreateDirectory(testImageDir);
            }
            Directory.CreateDirectory(_outputFolder);

            // Create a test image if it doesn't exist
            if (!File.Exists(_testImagePath))
            {
                // Create a simple test image using ImageSharp
                using (var image = new Image<SixLabors.ImageSharp.PixelFormats.Rgba32>(100, 100))
                {
                    image.Save(_testImagePath, new PngEncoder());
                }
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Clean up created files
            if (Directory.Exists(_outputFolder))
            {
                try
                {
                    Directory.Delete(_outputFolder, true);
                }
                catch (IOException)
                {
                    // Ignore if files are in use
                }
            }
        }

        [TestMethod]
        public void ProcessImage_ValidImage_CreatesProcessedImages()
        {
            // Arrange
            var processor = new AdvancedImageProcessor();

            // Act
            string result = processor.ProcessImage(_testImagePath, _outputFolder);

            // Assert
            Assert.IsTrue(File.Exists(result), "Processed image should exist");

            // Check if multiple processed images were created
            string baseName = Path.GetFileNameWithoutExtension(_testImagePath);
            string extension = Path.GetExtension(_testImagePath);

            // Check for at least one of the expected output files
            bool hasAnyProcessedFile = File.Exists(Path.Combine(_outputFolder, $"{baseName}_original{extension}")) ||
                                      File.Exists(Path.Combine(_outputFolder, $"{baseName}_rotate_90{extension}"));

            Assert.IsTrue(hasAnyProcessedFile, "At least one processed image variant should be created");
        }

        [TestMethod]
        public void ProcessImage_InvalidImage_HandlesException()
        {
            // Arrange
            var processor = new AdvancedImageProcessor();
            string invalidImagePath = "nonexistent.png";

            // Act
            string result = processor.ProcessImage(invalidImagePath, _outputFolder);

            // Assert
            Assert.AreEqual(
                Path.Combine(_outputFolder, $"{Path.GetFileNameWithoutExtension(invalidImagePath)}_processed{Path.GetExtension(invalidImagePath)}"),
                result,
                "Should return the expected output path even when processing fails");
        }
    }

    [TestClass]
    public class ExperimentalImageProcessorTests
    {
        private string _testImagePath = string.Empty;
        private string _outputFolder = string.Empty;

        [TestInitialize]
        public void Setup()
        {
            // Setup test paths
            _testImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestImages", "sample.png");
            _outputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestOutput");

            // Ensure directories exist
            string? testImageDir = Path.GetDirectoryName(_testImagePath);
            if (!string.IsNullOrEmpty(testImageDir))
            {
                Directory.CreateDirectory(testImageDir);
            }
            Directory.CreateDirectory(_outputFolder);

            // Create a test image if it doesn't exist
            if (!File.Exists(_testImagePath))
            {
                // Create a simple test image using ImageSharp
                using (var image = new Image<SixLabors.ImageSharp.PixelFormats.Rgba32>(100, 100))
                {
                    image.Save(_testImagePath, new PngEncoder());
                }
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Clean up created files
            if (Directory.Exists(_outputFolder))
            {
                try
                {
                    Directory.Delete(_outputFolder, true);
                }
                catch (IOException)
                {
                    // Ignore if files are in use
                }
            }
        }

        [TestMethod]
        public void ProcessImage_ValidImage_CreatesBestAndVariants()
        {
            // Arrange
            var processor = new ExperimentalImageProcessor();

            // Act
            string result = processor.ProcessImage(_testImagePath, _outputFolder);

            // Assert
            Assert.IsTrue(File.Exists(result), "Best processed image should exist");
            Assert.IsTrue(result.Contains("_best"), "Result should be the 'best' image path");

            // Check for at least one technique variant
            bool hasAnyVariant =
                Directory.GetFiles(_outputFolder, $"{Path.GetFileNameWithoutExtension(_testImagePath)}_*{Path.GetExtension(_testImagePath)}").Length > 0;

            Assert.IsTrue(hasAnyVariant, "At least one processing technique variant should be created");
        }
    }

    [TestClass]
    public class ImageBatchProcessorTests
    {
        private Mock<IImageProcessor> _mockImageProcessor = null!;
        private Mock<ITextExtractor> _mockTextExtractor = null!;
        private string _inputFolder = string.Empty;
        private string _outputFolder = string.Empty;
        private string _extractedTextFolder = string.Empty;
        private string _testImagePath = string.Empty;

        [TestInitialize]
        public void Setup()
        {
            // Create mocks
            _mockImageProcessor = new Mock<IImageProcessor>();
            _mockTextExtractor = new Mock<ITextExtractor>();

            // Setup test paths
            _inputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestInputImages");
            _outputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestOutputImages");
            _extractedTextFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestExtractedText");
            _testImagePath = Path.Combine(_inputFolder, "test.jpg");

            // Ensure directories exist
            Directory.CreateDirectory(_inputFolder);
            Directory.CreateDirectory(_outputFolder);
            Directory.CreateDirectory(_extractedTextFolder);

            // Create a test image
            if (!File.Exists(_testImagePath))
            {
                // Create a file with .jpg extension (doesn't need to be an actual image for the test)
                File.WriteAllText(_testImagePath, "Test file");
            }

            // Setup mock behavior
            string processedImagePath = Path.Combine(_outputFolder, "test_processed.jpg");
            _mockImageProcessor.Setup(p => p.ProcessImage(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(processedImagePath);

            _mockTextExtractor.Setup(e => e.ExtractTextFromImage(It.IsAny<string>()))
                .ReturnsAsync("Extracted test text");
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Clean up created files and folders
            if (Directory.Exists(_inputFolder))
            {
                try
                {
                    Directory.Delete(_inputFolder, true);
                }
                catch (IOException)
                {
                    // Ignore if files are in use
                }
            }

            if (Directory.Exists(_outputFolder))
            {
                try
                {
                    Directory.Delete(_outputFolder, true);
                }
                catch (IOException)
                {
                    // Ignore if files are in use
                }
            }

            if (Directory.Exists(_extractedTextFolder))
            {
                try
                {
                    Directory.Delete(_extractedTextFolder, true);
                }
                catch (IOException)
                {
                    // Ignore if files are in use
                }
            }
        }

        [TestMethod]
        public async Task ProcessImagesInFolder_WithValidImages_ProcessesAll()
        {
            // Arrange
            var batchProcessor = new ImageBatchProcessor(_mockImageProcessor.Object, _mockTextExtractor.Object);

            // Act
            await batchProcessor.ProcessImagesInFolder(_inputFolder, _outputFolder, _extractedTextFolder);

            // Assert
            _mockImageProcessor.Verify(p => p.ProcessImage(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            _mockTextExtractor.Verify(e => e.ExtractTextFromImage(It.IsAny<string>()), Times.Once());

            // Check if text file was created
            string expectedTextFile = Path.Combine(_extractedTextFolder, Path.GetFileNameWithoutExtension(_testImagePath) + ".txt");
            Assert.IsTrue(File.Exists(expectedTextFile), "Text file should be created");
        }

        [TestMethod]
        public async Task ProcessImagesInFolder_ProcessorThrowsException_HandlesGracefully()
        {
            // Arrange
            _mockImageProcessor.Setup(p => p.ProcessImage(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception("Test exception"));

            var batchProcessor = new ImageBatchProcessor(_mockImageProcessor.Object, _mockTextExtractor.Object);

            // Act - should not throw
            await batchProcessor.ProcessImagesInFolder(_inputFolder, _outputFolder, _extractedTextFolder);

            // Assert - method completed without throwing
            Assert.IsTrue(true, "Method should handle exceptions gracefully");

            // Verify the processor was called
            _mockImageProcessor.Verify(p => p.ProcessImage(It.IsAny<string>(), It.IsAny<string>()), Times.Once());

            // Extractor should not be called if processor throws
            _mockTextExtractor.Verify(e => e.ExtractTextFromImage(It.IsAny<string>()), Times.Never());
        }

        [TestMethod]
        public async Task ProcessImagesInFolder_ExtractorThrowsException_HandlesGracefully()
        {
            // Arrange
            _mockTextExtractor.Setup(e => e.ExtractTextFromImage(It.IsAny<string>()))
                .ThrowsAsync(new Exception("Test exception"));

            var batchProcessor = new ImageBatchProcessor(_mockImageProcessor.Object, _mockTextExtractor.Object);

            // Act - should not throw
            await batchProcessor.ProcessImagesInFolder(_inputFolder, _outputFolder, _extractedTextFolder);

            // Assert - method completed without throwing
            Assert.IsTrue(true, "Method should handle exceptions gracefully");

            // Verify both methods were called
            _mockImageProcessor.Verify(p => p.ProcessImage(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            _mockTextExtractor.Verify(e => e.ExtractTextFromImage(It.IsAny<string>()), Times.Once());

            // No text file should exist
            string expectedTextFile = Path.Combine(_extractedTextFolder, Path.GetFileNameWithoutExtension(_testImagePath) + ".txt");
            Assert.IsFalse(File.Exists(expectedTextFile), "Text file should not be created when extractor throws");
        }

        [TestMethod]
        public async Task ProcessImagesInFolder_NoImages_CompletesWithoutError()
        {
            // Arrange
            // Delete any test images
            if (File.Exists(_testImagePath))
            {
                File.Delete(_testImagePath);
            }

            var batchProcessor = new ImageBatchProcessor(_mockImageProcessor.Object, _mockTextExtractor.Object);

            // Act
            await batchProcessor.ProcessImagesInFolder(_inputFolder, _outputFolder, _extractedTextFolder);

            // Assert
            _mockImageProcessor.Verify(p => p.ProcessImage(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            _mockTextExtractor.Verify(e => e.ExtractTextFromImage(It.IsAny<string>()), Times.Never());
        }
    }

    [TestClass]
    public class IntegrationTests
    {
        private string _inputFolder = string.Empty;
        private string _outputFolder = string.Empty;
        private string _extractedTextFolder = string.Empty;
        private string _tessdataPath = string.Empty;

        [TestInitialize]
        public void Setup()
        {
            // Setup test paths
            _inputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IntegrationTestImages");
            _outputFolder = Path.Combine(_inputFolder, "OutputImages");
            _extractedTextFolder = Path.Combine(_inputFolder, "ExtractedText");
            _tessdataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");

            // Ensure directories exist
            Directory.CreateDirectory(_inputFolder);
            Directory.CreateDirectory(_outputFolder);
            Directory.CreateDirectory(_extractedTextFolder);
            Directory.CreateDirectory(_tessdataPath);
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Clean up created files
            if (Directory.Exists(_outputFolder))
            {
                try
                {
                    Directory.Delete(_outputFolder, true);
                }
                catch (IOException)
                {
                    // Ignore if files are in use
                }
            }

            if (Directory.Exists(_extractedTextFolder))
            {
                try
                {
                    Directory.Delete(_extractedTextFolder, true);
                }
                catch (IOException)
                {
                    // Ignore if files are in use
                }
            }
        }

        [TestMethod]
        [Ignore] // Ignore for automated CI/CD - requires real image files
        public async Task FullIntegration_WithRealComponents_CompletesSuccessfully()
        {
            // This test requires actual image files and tessdata to be present
            // Skip if no image files
            if (Directory.GetFiles(_inputFolder, "*.jpg").Length == 0)
            {
                Assert.Inconclusive("No test images found. Skipping integration test.");
                return;
            }

            // Arrange
            var imageProcessor = new AdvancedImageProcessor();
            var textExtractor = new TesseractTextExtractor(_tessdataPath);
            var batchProcessor = new ImageBatchProcessor(imageProcessor, textExtractor);

            // Act
            await batchProcessor.ProcessImagesInFolder(_inputFolder, _outputFolder, _extractedTextFolder);

            // Assert
            // Check if any processed images were created
            Assert.IsTrue(Directory.GetFiles(_outputFolder).Length > 0, "Processed images should be created");

            // Check if any text files were created
            Assert.IsTrue(Directory.GetFiles(_extractedTextFolder).Length > 0, "Text files should be created");
        }
    }
}