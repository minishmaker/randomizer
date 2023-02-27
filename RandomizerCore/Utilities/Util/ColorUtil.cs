using System.Drawing;
using RandomizerCore.Utilities.Logging;

namespace RandomizerCore.Utilities.Util;

public static class ColorUtil
{
    // Hsb converions from https://www.rapidtables.com/convert/color/rgb-to-hsv.html
    public static float GetHsbSaturation(this Color color)
    {
        var divR = color.R / 255f;
        var divG = color.G / 255f;
        var divB = color.B / 255f;

        // Get minimum and maximum color values
        var colorMin = Math.Min(divR, Math.Min(divG, divB));
        var colorMax = Math.Max(divR, Math.Max(divG, divB));

        var colorChange = colorMax - colorMin;

        return colorMax == 0 ? 0 : colorChange / colorMax;
    }

    public static float GetHsbValue(this Color color)
    {
        var divR = color.R / 255f;
        var divG = color.G / 255f;
        var divB = color.B / 255f;

        // Get maximum color value, that's value
        var colorMax = Math.Max(divR, Math.Max(divG, divB));

        return colorMax;
    }

    public static float GetHslSaturation(this Color color)
    {
        return color.GetSaturation();
    }

    public static float GetHslLightness(this Color color)
    {
        return color.GetBrightness();
    }


    // https://stackoverflow.com/questions/4106363/converting-rgb-to-hsb-colors
    /// <summary>
    ///     Creates a Color from alpha, hue, saturation and brightness.
    /// </summary>
    /// <param name="alpha">The alpha channel value.</param>
    /// <param name="hue">The hue value.</param>
    /// <param name="saturation">The saturation value.</param>
    /// <param name="brightness">The brightness value.</param>
    /// <returns>A Color with the given values.</returns>
    public static Color FromAhsb(int alpha, float hue, float saturation, float brightness)
    {
        if (0 > alpha
            || 255 < alpha)
            throw new ArgumentOutOfRangeException(
                nameof(alpha),
                alpha,
                "Value must be within a range of 0 - 255.");

        if (0f > hue
            || 360f < hue)
            throw new ArgumentOutOfRangeException(
                nameof(hue),
                hue,
                "Value must be within a range of 0 - 360.");

        if (0f > saturation
            || 1f < saturation)
            throw new ArgumentOutOfRangeException(
                nameof(saturation),
                saturation,
                "Value must be within a range of 0 - 1.");

        if (0f > brightness
            || 1f < brightness)
            throw new ArgumentOutOfRangeException(
                nameof(brightness),
                brightness,
                "Value must be within a range of 0 - 1.");

        if (0 == saturation)
            return Color.FromArgb(
                alpha,
                Convert.ToInt32(brightness * 255),
                Convert.ToInt32(brightness * 255),
                Convert.ToInt32(brightness * 255));

        float fMax, fMid, fMin;
        int iSextant, iMax, iMid, iMin;

        if (0.5 < brightness)
        {
            fMax = brightness - brightness * saturation + saturation;
            fMin = brightness + brightness * saturation - saturation;
        }
        else
        {
            fMax = brightness + brightness * saturation;
            fMin = brightness - brightness * saturation;
        }

        iSextant = (int)Math.Floor(hue / 60f);
        if (300f <= hue) hue -= 360f;

        hue /= 60f;
        hue -= 2f * (float)Math.Floor((iSextant + 1f) % 6f / 2f);
        if (0 == iSextant % 2)
            fMid = hue * (fMax - fMin) + fMin;
        else
            fMid = fMin - hue * (fMax - fMin);

        Logger.Instance.LogInfo(
            $"Color: Saturation: {saturation}, Brightness: {brightness}, fMax: {fMax}, fMin: {fMin}, fMid: {fMid}");

        iMax = Convert.ToInt32(fMax * 255);
        iMid = Convert.ToInt32(fMid * 255);
        iMin = Convert.ToInt32(fMin * 255);

        switch (iSextant)
        {
            case 1:
                return Color.FromArgb(alpha, iMid, iMax, iMin);
            case 2:
                return Color.FromArgb(alpha, iMin, iMax, iMid);
            case 3:
                return Color.FromArgb(alpha, iMin, iMid, iMax);
            case 4:
                return Color.FromArgb(alpha, iMid, iMin, iMax);
            case 5:
                return Color.FromArgb(alpha, iMax, iMin, iMid);
            default:
                return Color.FromArgb(alpha, iMax, iMid, iMin);
        }
    }

    public static Color AdjustHue(Color change, Color baseColor, Color newColor)
    {
        // Okay, this code kinda uses HSL and HSV incorrectly. But, whatever, it works.
        // At least I think. As of writing this it hasn't worked yet.
        var newHue = newColor.GetHue() - (baseColor.GetHue() - change.GetHue());
        var newSaturation = newColor.GetHslSaturation() - (baseColor.GetHslSaturation() - change.GetHslSaturation());
        var newValue = newColor.GetHslLightness() - (baseColor.GetHslLightness() - change.GetHslLightness());

        Logger.Instance.LogInfo(
            $"Adjust Hue - Hue 1: {newColor.GetHue()}, Saturation 1: {newColor.GetHsbSaturation()}, Brightness 1: {newColor.GetHsbValue()}, Red 1: {newColor.R}, Green 1: {newColor.G}, Blue 1: {newColor.B}");

        newHue %= 360f;
        if (newHue < 0f) newHue += 360f;

        Logger.Instance.LogInfo(
            $"Adjust Hue - Hue 2: {newHue}, Saturation 2: {newSaturation}, Brightness 2: {newValue}");

        newSaturation = Math.Max(0, Math.Min(1, newSaturation));
        newValue = Math.Max(0, Math.Min(1, newValue));

        var outColor = FromAhsb(255, newHue, newSaturation, newValue);

        Logger.Instance.LogInfo($"Adjust Hue - Red 2: {outColor.R}, Green 2: {outColor.G}, Blue 2: {outColor.B}");

        return outColor;
    }
}
