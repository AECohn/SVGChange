// See https://aka.ms/new-console-template for more information

using System.Drawing;
using System.Drawing.Imaging;
using Svg;

Color color;
string subfolderName;

string PromptMessage(string message)
{
    Console.WriteLine(message);
    return Console.ReadLine() ?? string.Empty;
}


var path = PromptMessage("Drag the folder containing the SVG file here, then press enter").Replace("\"", "");

var response = PromptMessage("What color would you like to use?");


if (response.Contains('#'))
{
    color = ColorTranslator.FromHtml(response);
}
else
{
    color = Color.FromName(response);
}

if (color.IsKnownColor == false)
{
    Console.WriteLine("unknown color, setting to black ");
    color = Color.Black;
}


subfolderName = color.Name;
Directory.CreateDirectory($"{path}/{subfolderName}");

string[] svgFiles = Directory.GetFiles(path, "*.svg");

Bitmap? bitmap = null;
try
{
    for (int i = 0; i < svgFiles.Length; i++)
    {
        bitmap = new Bitmap(800, 600); // Placeholder dimensions

        // Render the SVG onto the Bitmap
        ConvertColor(svgFiles[i], color).Draw(bitmap);

        string outputAddress = $"{Path.GetDirectoryName(svgFiles[i])}/{subfolderName}/{Path.GetFileNameWithoutExtension(svgFiles[i])}.png";

        // Save the Bitmap as a PNG file
        bitmap.Save(outputAddress, ImageFormat.Png);

        // Dispose of the Bitmap
        bitmap.Dispose();
        bitmap = null;
    }
}
finally
{
    // Dispose of the Bitmap if an exception occurs
    bitmap?.Dispose();
}

SvgDocument ConvertColor(string filePath, Color newColor)
{
 
    var svgDocument = SvgDocument.Open(Path.GetFullPath(filePath));

    if (svgDocument != null)
    {
        var svgPath = svgDocument.Children[0] as SvgPath;
        if (svgPath != null)
        {
            svgPath.Fill = new SvgColourServer(newColor);
        }
    }

    return svgDocument;
}

Console.WriteLine("Complete, press any key to exit");
Console.ReadKey();