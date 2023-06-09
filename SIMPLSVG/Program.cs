// See https://aka.ms/new-console-template for more information

using System.Drawing;
using Svg;

Color _color;

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

string[] svgFiles = Directory.GetFiles(path, "*.svg");


foreach (string file in svgFiles)
{
    ConvertColor(file, _color);
}


static void ConvertColor(string path, Color color)
{
    var svgDocument = SvgDocument.Open(Path.GetFullPath(path));

    var svgPath = svgDocument.Children[0] as SvgPath;
    //Change color of an SVG
    svgPath.Fill = new SvgColourServer(color);

    //save the svg to a new file
    svgDocument.Write($"{Path.GetDirectoryName(path)}/temp_new.svg");
}