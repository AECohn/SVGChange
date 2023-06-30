using System.Drawing;
using System.Drawing.Imaging;
using SIMPLSVG;
using Svg;

class SVGChange
{
    static void Main(string[] args)
    {
        try
        {
            string path = InternalMethods.Prompt("Drag the folder containing the SVG file(s) here, then press enter")
                .Replace("\"", "");
            ;
            string colorInput = InternalMethods.Prompt("What color would you like to use?");
            Color color = InternalMethods.ParseColor(colorInput);
            bool useCustomDimensions =
                InternalMethods.Prompt("Would you like to use custom dimensions? (y/n)").ToLower() == "y";
            int width = useCustomDimensions
                ? InternalMethods.GetDimension("What width in pixels would you like to use?")
                : 0;
            int height = useCustomDimensions
                ? InternalMethods.GetDimension("What height in pixels would you like to use?")
                : 0;
            Bitmap bitmap;

            Console.WriteLine("Converting...");

            string subfolderName = color.Name;
            Directory.CreateDirectory($"{path}/{subfolderName}");

            string[] svgFiles = Directory.GetFiles(path, "*.svg");

            for (int i = 0; i < svgFiles.Length; i++)
            {
                SvgDocument svgDocument = InternalMethods.ConvertColor(svgFiles[i], color);
                bitmap = useCustomDimensions? new Bitmap(width, height) : new Bitmap((int)svgDocument.Width, (int)svgDocument.Height);
               
                InternalMethods.ConvertSVG(svgDocument,
                    $"{Path.GetDirectoryName(svgFiles[i])}/{subfolderName}/{Path.GetFileNameWithoutExtension(svgFiles[i])}_{color.Name}.png",
                    bitmap, ImageFormat.Png);
            }

            Console.WriteLine("Done.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}