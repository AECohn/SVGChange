using Svg;

namespace SIMPLSVG;

using System.Drawing;
using System.Drawing.Imaging;

public class InternalMethods
{
    static public string Prompt(string message)
    {
        Console.WriteLine(message);
        return Console.ReadLine() ?? string.Empty;
    }

    static public int GetDimension(string message)
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

    static public Color ParseColor(string input)
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

    static public SvgDocument ConvertColor(string svgFile, Color color)
    {
        SvgDocument svgDocument = SvgDocument.Open(svgFile);
        svgDocument.Fill = new SvgColourServer(color);
        return svgDocument;
    }

    static public void ConvertSVG(SvgDocument SVGDoc, string outputPath, Bitmap bitmap, ImageFormat imageFormat)
    {
        SVGDoc.Draw(bitmap);
        bitmap.Save(outputPath, imageFormat);
    }
}
    