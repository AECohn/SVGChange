﻿using System.Drawing;
using System.Drawing.Imaging;
using Svg;

Color color;
string subfolderName;

string PromptMessage(string message)
{
    Console.WriteLine(message);
    return Console.ReadLine() ?? string.Empty;
}

var path = PromptMessage("Drag the folder containing the SVG file(s) here, then press enter").Replace("\"", "");
var response = PromptMessage("What color would you like to use?");

Console.WriteLine("Converting...");

if (response.Contains('#'))
    color = ColorTranslator.FromHtml(response);
else
{
    color = Color.FromName(response);

    if (color.IsKnownColor == false)
    {
        Console.WriteLine("unknown color, setting to black ");
        color = Color.Black;
    }
}

subfolderName = color.Name;

Directory.CreateDirectory($"{path}/{subfolderName}");

string[] svgFiles = Directory.GetFiles(path, "*.svg");

Bitmap? bitmap = null;

try
{
    for (int i = 0; i < svgFiles.Length; i++)
    {
        SvgDocument svgDocument = ConvertColor(svgFiles[i], color);
        var svgPath = svgDocument.Children[0] as SvgPath;
        bitmap = new Bitmap((int)svgPath.Bounds.Width, (int)svgPath.Bounds.Height);


        ConvertColor(svgFiles[i], color).Draw(bitmap);

        string outputAddress =
            $"{Path.GetDirectoryName(svgFiles[i])}/{subfolderName}/{Path.GetFileNameWithoutExtension(svgFiles[i])}_{color.Name}.png";

        bitmap.Save(outputAddress, ImageFormat.Png);
        bitmap.Dispose();
        bitmap = null;
    }
}
finally
{
    bitmap?.Dispose(); // Dispose of the Bitmap if an exception occurs
}

SvgDocument ConvertColor(string filePath, Color newColor)
{
    var svgDocument = SvgDocument.Open(Path.GetFullPath(filePath));
    //get svg document dimensions in pixels

    if (svgDocument != null)
    {
        var svgPath = svgDocument.Children[0] as SvgPath;
        //var Width = svgPath.Bounds.Width;
        //var Height = svgPath.Bounds.Height; 

        if (svgPath != null)
        {
            svgPath.Fill = new SvgColourServer(newColor);
        }
    }

    return svgDocument;
}

Console.WriteLine("Complete, press any key to exit");
Console.ReadKey();