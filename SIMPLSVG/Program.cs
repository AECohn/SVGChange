// See https://aka.ms/new-console-template for more information

using System.Drawing;
using Svg;
Console.WriteLine("Drag the folder containing the SVG file here, then press enter");
var path = Console.ReadLine();

var svgDocument = SvgDocument.Open(path);
var svgPath = svgDocument.Children[0] as SvgPath;
//Change color of an SVG
svgPath.Fill = new SvgColourServer(Color.Blue);

//save the svg to a new file
svgDocument.Write("svgtest.svg");
Console.WriteLine("success?");
Console.ReadKey();

