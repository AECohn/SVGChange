// See https://aka.ms/new-console-template for more information

using System.Drawing;
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

    if(_color.IsKnownColor == false)
    {
        Console.WriteLine("unknown color, setting to black ");
        _color = Color.Black;
    }




subfolderName = _color.Name;
Directory.CreateDirectory($"{path}/{subfolderName}");

string[] svgFiles = Directory.GetFiles(path, "*.svg");


for (int i = 0; i < svgFiles.Length; i++)
{
    ConvertColor(svgFiles[i], _color);
}

void ConvertColor(string filePath, Color color)
{
    var svgDocument = SvgDocument.Open(Path.GetFullPath(filePath));

    if (svgDocument != null)
    {
        var svgPath = svgDocument.Children[0] as SvgPath;
        svgPath.Fill = new SvgColourServer(color);
        
        
        try
        {
            svgDocument.Write($"{Path.GetDirectoryName(filePath)}/{subfolderName}/{Path.GetFileName(filePath)}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}

Console.ReadKey();