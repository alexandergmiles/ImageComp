using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;


namespace ImageCompression
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] paths = { "/Users/alexandergmiles/Desktop/ImageCompression/ImageCompression/Images/1.ppm", "/Users/alexandergmiles/Desktop/ImageCompression/ImageCompression/Images/2.ppm", "/Users/alexandergmiles/Desktop/ImageCompression/ImageCompression/Images/3.ppm"};
            //Just telling the user details of what's going on
            Console.WriteLine($"Using image: {args[0]} for compression. Output at: {args[1]}");
     
            //Console.WriteLine(bytesFromFile.Count);
            int height = 768;
            int width = 1024;
            //List of the found pixels
            List<byte> bytesFromFile = File.ReadAllBytes(args[0]).ToList();
            
            //Will we get a more accurate value by using these instead?
            List<String> pixelValues = new List<String>();
            
            int placeInByteArr = 17;
            Bitmap bmp = new Bitmap(1024, 768);

            
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bmp.SetPixel(x, y, Color.FromArgb(
                        bytesFromFile[placeInByteArr], bytesFromFile[placeInByteArr+1],
                        bytesFromFile[placeInByteArr+2]));
                    placeInByteArr += 3;
                }
            }
            bmp.Save(args[0]  + ".bmp");


            var result = CanWeMakeItSmaller(bmp);

            Bitmap test = new Bitmap(width, height);
            int heightValue = 0;
            int widthValue = 0;
            int pixelValue = 0;
            while (widthValue < 1024)
            {
                if (widthValue == 1024)
                {
                    heightValue++;
                    widthValue = 0;
                }

                for(int i = widthValue; i < (widthValue += result[pixelValue].Item1); i++)
                    test.SetPixel(widthValue, heightValue, Color.FromName(result[pixelValue].Item2));
            }
        }
        
        static List<(int, string)> CanWeMakeItSmaller(Bitmap image)
        {
            
            List<(int, string)> returnList = new List<(int, string)>();
            
            //Take image
            for (int y = 0; y < image.Height; y++)
            {
                var currentPixelCount = 1;
                Color currentPixelColour = image.GetPixel(y, 0);
                
                for (int x = 0; x < image.Width; x++)
                {
                    if (isSimilar(currentPixelColour, image.GetPixel(x, y)))
                    {
                        currentPixelCount++;
                    }
                    else
                    {
                        returnList.Add((currentPixelCount, currentPixelColour.Name));
                        currentPixelColour = image.GetPixel(x, y);
                        currentPixelCount = 1;
                    }
                }
            }
            return returnList;
        }


        static bool isSimilar(System.Drawing.Color valueOne, System.Drawing.Color valueTwo)
        {
            return valueOne.Name == valueTwo.Name;
        }
    }
}