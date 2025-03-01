// AppConfig.cs
using System;
using System.IO;

namespace TextFromImages
{
    /// <summary>
    /// Holds application configuration settings
    /// </summary>
    public class AppConfig
    {
        public string InputFolder { get; set; }
        public string OutputFolder { get; set; }
        public string TessDataPath { get; set; }
        public string Language { get; set; }
        public bool ApplyAllPreprocessing { get; set; }

        public AppConfig()
        {
            // Default values
            InputFolder = @"D:\Git\neocortexapi\neocortexapi-Fusion_Coders\source\MySEProject\Fusion_Coders_SE_Project\TextFromImages\TextFromImages\InputImages";
            OutputFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OutputResults");
            TessDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");
            Language = "eng";
            ApplyAllPreprocessing = true;
        }
    }

    /// <summary>
    /// Handles command line argument parsing
    /// </summary>
    public static class CommandLineParser
    {
        public static AppConfig ParseArguments(string[] args)
        {
            AppConfig config = new AppConfig();

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "-input":
                    case "-i":
                        if (i + 1 < args.Length) config.InputFolder = args[++i];
                        break;
                    case "-output":
                    case "-o":
                        if (i + 1 < args.Length) config.OutputFolder = args[++i];
                        break;
                    case "-tessdata":
                    case "-t":
                        if (i + 1 < args.Length) config.TessDataPath = args[++i];
                        break;
                    case "-lang":
                    case "-l":
                        if (i + 1 < args.Length) config.Language = args[++i];
                        break;
                    case "-preprocess":
                    case "-p":
                        if (i + 1 < args.Length) config.ApplyAllPreprocessing = bool.Parse(args[++i]);
                        break;
                    case "-help":
                    case "-h":
                        ShowHelp();
                        Environment.Exit(0);
                        break;
                }
            }

            return config;
        }

        public static void ShowHelp()
        {
            Console.WriteLine("Usage: TextFromImages.exe [options]");
            Console.WriteLine("Options:");
            Console.WriteLine("  -i, -input <path>      Input folder containing images");
            Console.WriteLine("  -o, -output <path>     Output folder for results");
            Console.WriteLine("  -t, -tessdata <path>   Path to tessdata folder");
            Console.WriteLine("  -l, -lang <language>   OCR language (default: eng)");
            Console.WriteLine("  -p, -preprocess <bool> Apply all preprocessing (default: true)");
            Console.WriteLine("  -h, -help              Show this help message");
        }
    }
}