using System.Drawing;

namespace RandomizerCore.Utilities.Models;

public struct GbaColor
{
    public int R;
    public int G;
    public int B;

    public short CombinedValue =>
        // Each component needs to be shifted to the proper place in the short
        (short)((B << 10) | (G << 5) | (R << 0));

    public GbaColor(Color initializerColor)
    {
        if (initializerColor == Color.Empty) initializerColor = Color.FromArgb(0, 0, 0);

        // Only the top five bits are used for each color
        R = (initializerColor.R & 0xF8) >> 3;
        G = (initializerColor.G & 0xF8) >> 3;
        B = (initializerColor.B & 0xF8) >> 3;
    }

    public GbaColor(int r, int g, int b)
    {
        R = r & 0x1F;
        G = g & 0x1F;
        B = b & 0x1F;
    }

    /*public GBAColor(short combinedColor)
    {
        // Each color part comes from a different 5 bits of the combined color
        R = (combinedColor >> 10) & 0x1F;
        G = (combinedColor >> 05) & 0x1F;
        B = (combinedColor >> 00) & 0x1F;
    }*/
    public Color ToColor()
    {
        return Color.FromArgb(R << 3, G << 3, B << 3);
    }
}
