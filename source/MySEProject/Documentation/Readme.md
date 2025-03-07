# TextFromImages - OCR with Image Preprocessing

## Introduction
TextFromImages is a C# console application that extracts text from images using Optical Character Recognition (OCR) technology. The project applies various image preprocessing techniques to improve text extraction accuracy, comparing different transformations to optimize results.

## Key Features
- Processes multiple image formats (JPG, JPEG, PNG, BMP, TIFF, GIF)
- Applies 10+ different image preprocessing techniques
- Extracts text using Tesseract OCR engine
- Batch processes all images in a specified folder
- Saves both processed images and extracted text

## System Requirements
- .NET 6.0 or higher
- Visual Studio 2019 or later

## Dependencies (NuGet Packages)
- `Tesseract` (for OCR text extraction)
- `SixLabors.ImageSharp` (for image processing operations)

## Installation
1. Clone the repository or download the source code.
2. Open the solution in Visual Studio.
3. Restore NuGet packages.
4. Download Tesseract language data files:
   - Create a `tessdata` folder in your project directory.
   - Download the English language data file (`eng.traineddata`) from the [Tesseract GitHub repository](https://github.com/tesseract-ocr/tessdata).
   - Place the downloaded file in the `tessdata` folder.

## Project Structure
The project follows a clean architecture with interfaces for modularity:

- **`Program.cs`**: Entry point, sets up the batch processing pipeline.
- **`IImageProcessor.cs` & `ITextExtractor.cs`**: Core interfaces.
- **`AdvancedImageProcessor.cs`**: Implements various image transformations.
- **`TesseractTextExtractor.cs`**: Handles OCR text extraction.
- **`ImageBatchProcessor.cs`**: Orchestrates the processing of multiple images.
- **`ExperimentalImageProcessor.cs`**: Alternative processor with different techniques.
## Image Preprocessing Techniques
The application applies these transformations to improve OCR accuracy:

1. **Basic Grayscale**: Converts image to grayscale.
2. **Rotation Variations**:
   - 90° rotation
   - -90° rotation
   - 15° clockwise rotation
   - 15° counter-clockwise rotation
   - 5° slight skew
3. **Shifting**:
   - Horizontal shift
   - Vertical shift
4. **Image Enhancement**:
   - Contrast enhancement
   - Brightness adjustment
5. **Experimental Techniques**:
   - Resizing
   - Combined transformations

## OCR Processing
- Uses Tesseract OCR engine.
- Configures Tesseract to preserve interword spaces.
- Reports confidence level for extracted text.
- Post-processes extracted text to remove excessive whitespace.

## Usage
1. Update the input folder path in `Program.cs` to your images location.
2. Build and run the application.
3. Check the output folders for:
   - Processed images (in `OutputImages` subfolder).
   - Extracted text files (in `ExtractedText` subfolder).
