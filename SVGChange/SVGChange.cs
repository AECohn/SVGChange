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
            string path = Prompt("Drag the folder containing the SVG file(s) here, then press enter").Replace("\"", "");
            ;
            string colorInput = Prompt("What color would you like to use?");
            Color color = ParseColor(colorInput);
            bool useCustomDimensions = Prompt("Would you like to use custom dimensions? (y/n)").ToLower() == "y"? true: false;
            int width = useCustomDimensions ? GetDimension("What width in pixels would you like to use?") : 0;
            int height = useCustomDimensions ? GetDimension("What height in pixels would you like to use?") : 0;

            Console.WriteLine("Converting...");

            string subfolderName = color.Name;
            Directory.CreateDirectory($"{path}/{subfolderName}");

            string[] svgFiles = Directory.GetFiles(path, "*.svg");

            for (int i = 0; i < svgFiles.Length; i++)
            {
                SvgDocument svgDocument = ConvertColor(svgFiles[i], color);
                SvgPath svgPath = (SvgPath)svgDocument.Children[0];

                Bitmap bitmap = useCustomDimensions
                    ? new Bitmap(width, height)
                    : new Bitmap((int)svgPath.Bounds.Width, (int)svgPath.Bounds.Height);

                svgDocument.Draw(bitmap);

                string outputAddress =
                    $"{Path.GetDirectoryName(svgFiles[i])}/{subfolderName}/{Path.GetFileNameWithoutExtension(svgFiles[i])}_{color.Name}.png";
                bitmap.Save(outputAddress, ImageFormat.Png);
            }

            Console.WriteLine("Done.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    static string Prompt(string message)
    {
        Console.WriteLine(message);
        return Console.ReadLine() ?? string.Empty;
    }

    static int GetDimension(string message)
    {
        int result;
        while (true)
        {
            if (int.TryParse(Prompt(message), out result))
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