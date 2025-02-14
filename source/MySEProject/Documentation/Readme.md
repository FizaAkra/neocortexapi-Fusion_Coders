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
