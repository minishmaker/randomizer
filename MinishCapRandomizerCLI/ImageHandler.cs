using System.Globalization;
using System.Reflection;
using RandomizerCore.Controllers;
using SkiaSharp;

namespace MinishCapRandomizerCLI;

public static class ImageHandler
{
    public static SKBitmap GetHashImage(string[] eventDefines)
    {    
        const byte hashMask = 0b111111;

        var badBgColor = SKColor.Parse("#30A0AC");
        var otherBadBgColor = SKColor.Parse("#30A078");
        var newBgColor = SKColor.Parse("#0819AD");
            
        var customRng = uint.Parse(eventDefines.First(line => line.Contains("customRNG")).Split(' ')[2][2..], 
            NumberStyles.HexNumber);
        var seed = uint.Parse(eventDefines.First(line => line.Contains("seedHashed")).Split(' ')[2][2..],
            NumberStyles.HexNumber);
        var settings = uint.Parse(eventDefines.First(line => line.Contains("settingHash")).Split(' ')[2][2..],
            NumberStyles.HexNumber);
        
        using var stream = Assembly.GetAssembly(typeof(ShufflerController))?.GetManifestResourceStream("RandomizerCore.Resources.hashicons.png");
        using var bitmap = SKBitmap.Decode(stream);
        using var hashBitmap = new SKBitmap(128, 16, SKColorType.Argb4444, SKAlphaType.Opaque);
        
        for (var imageNum = 0; imageNum < 8; ++imageNum)
        {

            var imageIndex = imageNum switch
            {
                0 => (seed >> 24) & hashMask,
                1 => (seed >> 16) & hashMask,
                2 => (seed >> 8) & hashMask,
                3 => seed & hashMask,
                4 => (customRng >> 8) & hashMask,
                5 => 64U,
                6 => (settings >> 8) & hashMask,
                7 => (settings >> 16) & hashMask
            };

            var k = 16 * (int)imageIndex;
            var l = 16 * imageNum;
            for (var i = 0; i < 16; ++i)
            {
                for (var j = 0; j < 16; ++j)
                {
                    var color = bitmap.GetPixel(j, i + k);
                    if (color == badBgColor || color == otherBadBgColor)
                        color = newBgColor;
                    hashBitmap.SetPixel(j + l, i, color);
                }
            }
        }

        var finalBitmap = new SKBitmap(384, 48, SKColorType.Argb4444, SKAlphaType.Opaque);
        hashBitmap.ScalePixels(finalBitmap, SKFilterQuality.High);
        return finalBitmap;
    }
}
