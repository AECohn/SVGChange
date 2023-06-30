using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Svg;

class SVGChange
{
    static void Main(string[] args)
    {
        try
        {
            string path = PromptFolder("Drag the folder containing the SVG file(s) here, then press enter");
            string colorInput = PromptMessage("What color would you like to use?");
            Color color = ParseColor(colorInput);
            bool useCustomDimensions = PromptYesNo("Would you like to use custom dimensions?");
            int width = useCustomDimensions ? PromptInt("What width in pixels would you like to use?") : 0;
            int height = useCustomDimensions ? PromptInt("What height in pixels would you like to use?") : 0;

            Console.WriteLine("Converting...");

            string subfolderName = color.Name;
            Directory.CreateDirectory($"{path}/{subfolderName}");

            string[] svgFiles = Directory.GetFiles(path, "*.svg");

            for (int i = 0; i < svgFiles.Length; i++)
            {
                SvgDocument svgDocument = ConvertColor(svgFiles[i], color);
                SvgPath svgPath = (SvgPath)svgDocument.Children[0];

                Bitmap bitmap = useCustomDimensions ? new Bitmap(width, height) : new Bitmap((int)svgPath.Bounds.Width, (int)svgPath.Bounds.Height);

                svgDocument.Draw(bitmap);

                string outputAddress = $"{Path.GetDirectoryName(svgFiles[i])}/{subfolderName}/{Path.GetFileNameWithoutExtension(svgFiles[i])}_{color.Name}.png";
                bitmap.Save(outputAddress, ImageFormat.Png);
            }

            Console.WriteLine("Done.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static string PromptFolder(string message)
    {
        Console.WriteLine(message);
        return Console.ReadLine().Replace("\"", "");
    }

    static string PromptMessage(string message)
    {
        Console.WriteLine(message);
        return Console.ReadLine() ?? string.Empty;
    }

    static bool PromptYesNo(string message)
    {
        string response = PromptMessage($"{message} (y/n)");
        return response.ToLower() == "y";
    }

    static int PromptInt(string message)
    {
        int result;
        while (true)
        {
            string input = PromptMessage(message);
            if (int.TryParse(input, out result))
            {
                return result;
            }
            Console.WriteLine("Invalid input. Please enter a valid integer.");
        }
    }

    static Color ParseColor(string input)
    {
        if (input.Contains('#'))
        {
            return ColorTranslator.FromHtml(input);
        }
        else
        {
            Color color = Color.FromName(input);
            if (color.IsKnownColor)
            {
                return color;
            }
            else
            {
                Console.WriteLine("Unknown color, setting to black.");
                return Color.Black;
            }
        }
    }

    static SvgDocument ConvertColor(string svgFile, Color color)
    {
        SvgDocument svgDocument = SvgDocument.Open(svgFile);
        svgDocument.Fill = new SvgColourServer(color);
        return svgDocument;
    }
}