// See https://aka.ms/new-console-template for more information

using System.Drawing;
using System.Drawing.Imaging;
using Svg;

Color _color;
string subfolderName;

string promptMessage(string message)
{
    Console.WriteLine(message);
    return Console.ReadLine();
}


var path = promptMessage("Drag the folder containing the SVG file here, then press enter").Replace("\"", "");

var response = promptMessage("What color would you like to use?");


if (response.Contains('#'))
{
    _color = ColorTranslator.FromHtml(response);
}
else
{
    _color = Color.FromName(response);
}

if (_color.IsKnownColor == false)
{
    Console.WriteLine("unknown color, setting to black ");
    _color = Color.Black;
}


subfolderName = _color.Name;
Directory.CreateDirectory($"{path}/{subfolderName}");

string[] svgFiles = Directory.GetFiles(path, "*.svg");


for (int i = 0; i < svgFiles.Length; i++)
{
    {
        var bitmap = new Bitmap(800, 600); // Placeholder dimensions

        // Render the SVG onto the Bitmap
        ConvertColor(svgFiles[i], _color).Draw(bitmap);

        string outputAddress = $"{Path.GetDirectoryName(svgFiles[i])}/{subfolderName}/{Path.GetFileNameWithoutExtension(svgFiles[i])}.png";

        // Save the Bitmap as a PNG file
        bitmap.Save(outputAddress, ImageFormat.Png);

        // Dispose of the Bitmap
        bitmap.Dispose();
        bitmap = null;
    }
    
}

SvgDocument ConvertColor(string filePath, Color color)
{
    var svgDocument = SvgDocument.Open(Path.GetFullPath(filePath));

    if (svgDocument != null)
    {
        var svgPath = svgDocument.Children[0] as SvgPath;
        svgPath.Fill = new SvgColourServer(color);
        
    }

    return svgDocument;
}

Console.ReadKey();