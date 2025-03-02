## Introduction
This C# console application is designed to extract text from images using Optical Character Recognition (OCR) technology. The project utilizes Tesseract OCR engine along with image preprocessing capabilities to enhance text extraction accuracy. Our implementation focuses on improving text recognition by applying various image transformations such as rotation, shifting, and quality adjustments before performing the OCR process.
## System Requirements & Prerequisites
1. Software Requirements
   - Visual Studio 2019 or later
   - .NET Framework 6.0 or higher
   - Tesseract OCR engine (v5.0.0 or later)
   - System.Drawing.Common NuGet package
   - Tesseract NuGet package (v4.1.1 or later)
2. Installation Steps
   - Install Visual Studio 2019/2022
   - Clone the project repository
   - Install required NuGet packages:
     * Tesseract (via NuGet Package Manager)
     * System.Drawing.Common
     * Tesseract.Drawing
   - Download and install Tesseract language data files (eng.traineddata)
## Usage
1. Application Setup
   - Place input images in the designated input folder
   - Configure desired preprocessing parameters
   - Run the console application with appropriate command-line arguments
2. Features
   - Multiple image preprocessing options
   - Text extraction using Tesseract OCR
   - Quality comparison of different preprocessing methods
   - Output in various formats (.txt)

## Project Structure & Implementation

1. **Core Components**
   * ImagePreprocessor: Handles various image transformations
   * OCREngine: Manages Tesseract integration and text extraction
   * ResultEvaluator: Compares and analyzes extraction quality
   * ConsoleInterface: Processes command-line arguments

2. **Image Preprocessing Pipeline**
   * Grayscale conversion
   * Noise reduction
   * Contrast enhancement
   * Binarization/thresholding
   * Rotation correction 
   * Perspective transformation
   * Scaling and resolution adjustment
3. **OCR Processing Flow**
   * Load image from input directory
   * Apply selected preprocessing techniques
   * Initialize Tesseract engine with appropriate parameters
   * Process image through OCR
   * Store extracted text and quality metrics
   * Compare results across different preprocessing methods
   * Output results to specified format
