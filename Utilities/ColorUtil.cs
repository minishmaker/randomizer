using System;
using System.Drawing;

namespace MinishRandomizer.Utilities {
  public static class ColorExtensions {
  }

  public static class ColorUtil {
    // Hsb converions from https://www.rapidtables.com/convert/color/rgb-to-hsv.html
    public static float GetHsbSaturation( this Color color ) {
      float divR = color.R / 255f;
      float divG = color.G / 255f;
      float divB = color.B / 255f;

      // Get minimum and maximum color values
      float colorMin = Math.Min( divR, Math.Min( divG, divB ) );
      float colorMax = Math.Max( divR, Math.Max( divG, divB ) );

      float colorChange = colorMax - colorMin;

      return colorMax == 0 ? 0 : colorChange / colorMax;
    }

    public static float GetHsbValue( this Color color ) {
      float divR = color.R / 255f;
      float divG = color.G / 255f;
      float divB = color.B / 255f;

      // Get maximum color value, that's value
      float colorMax = Math.Max( divR, Math.Max( divG, divB ) );

      return colorMax;
    }

    public static float GetHslSaturation( this Color color ) {
      return color.GetSaturation();
    }

    public static float GetHslLightness( this Color color ) {
      return color.GetBrightness();
    }


    // https://stackoverflow.com/questions/4106363/converting-rgb-to-hsb-colors
    /// <summary>
    /// Creates a Color from alpha, hue, saturation and brightness.
    /// </summary>
    /// <param name="alpha">The alpha channel value.</param>
    /// <param name="hue">The hue value.</param>
    /// <param name="saturation">The saturation value.</param>
    /// <param name="brightness">The brightness value.</param>
    /// <returns>A Color with the given values.</returns>
    public static Color FromAhsb( int alpha, float hue, float saturation, float brightness ) {
      if ( 0 > alpha
          || 255 < alpha ) {
        throw new ArgumentOutOfRangeException(
            "alpha",
            alpha,
            "Value must be within a range of 0 - 255." );
      }

      if ( 0f > hue
          || 360f < hue ) {
        throw new ArgumentOutOfRangeException(
            "hue",
            hue,
            "Value must be within a range of 0 - 360." );
      }

      if ( 0f > saturation
          || 1f < saturation ) {
        throw new ArgumentOutOfRangeException(
            "saturation",
            saturation,
            "Value must be within a range of 0 - 1." );
      }

      if ( 0f > brightness
          || 1f < brightness ) {
        throw new ArgumentOutOfRangeException(
            "brightness",
            brightness,
            "Value must be within a range of 0 - 1." );
      }

      if ( 0 == saturation ) {
        return Color.FromArgb(
                            alpha,
                            Convert.ToInt32( brightness * 255 ),
                            Convert.ToInt32( brightness * 255 ),
                            Convert.ToInt32( brightness * 255 ) );
      }

      float fMax, fMid, fMin;
      int iSextant, iMax, iMid, iMin;

      if ( 0.5 < brightness ) {
        fMax = brightness - ( brightness * saturation ) + saturation;
        fMin = brightness + ( brightness * saturation ) - saturation;
      }
      else {
        fMax = brightness + ( brightness * saturation );
        fMin = brightness - ( brightness * saturation );
      }

      iSextant = (int)Math.Floor( hue / 60f );
      if ( 300f <= hue ) {
        hue -= 360f;
      }

      hue /= 60f;
      hue -= 2f * (float)Math.Floor( ( ( iSextant + 1f ) % 6f ) / 2f );
      if ( 0 == iSextant % 2 ) {
        fMid = ( hue * ( fMax - fMin ) ) + fMin;
      }
      else {
        fMid = fMin - ( hue * ( fMax - fMin ) );
      }

      Console.WriteLine( "MidS_" + saturation );
      Console.WriteLine( "MidL_" + brightness );
      Console.WriteLine( "Max_" + fMax );
      Console.WriteLine( "Min_" + fMin );
      Console.WriteLine( "Mid_" + fMid );

      iMax = Convert.ToInt32( fMax * 255 );
      iMid = Convert.ToInt32( fMid * 255 );
      iMin = Convert.ToInt32( fMin * 255 );

      switch ( iSextant ) {
        case 1:
          return Color.FromArgb( alpha, iMid, iMax, iMin );
        case 2:
          return Color.FromArgb( alpha, iMin, iMax, iMid );
        case 3:
          return Color.FromArgb( alpha, iMin, iMid, iMax );
        case 4:
          return Color.FromArgb( alpha, iMid, iMin, iMax );
        case 5:
          return Color.FromArgb( alpha, iMax, iMin, iMid );
        default:
          return Color.FromArgb( alpha, iMax, iMid, iMin );
      }
    }

    public static Color AdjustHue( Color change, Color baseColor, Color newColor ) {
      // Okay, this code kinda uses HSL and HSV incorrectly. But, whatever, it works.
      // At least I think. As of writing this it hasn't worked yet.
      float newHue = newColor.GetHue() - ( baseColor.GetHue() - change.GetHue() );
      float newSaturation = newColor.GetHslSaturation() - ( baseColor.GetHslSaturation() - change.GetHslSaturation() );
      float newValue = newColor.GetHslLightness() - ( baseColor.GetHslLightness() - change.GetHslLightness() );

      Console.WriteLine( "Hue1_" + newColor.GetHue() );
      Console.WriteLine( "Sat1_" + newColor.GetHsbSaturation() );
      Console.WriteLine( "Bri1_" + newColor.GetHsbValue() );
      Console.WriteLine( "R1_" + newColor.R );
      Console.WriteLine( "G1_" + newColor.G );
      Console.WriteLine( "B1_" + newColor.B );

      newHue %= 360f;
      if ( newHue < 0f ) {
        newHue += 360f;
      };

      Console.WriteLine( "Hue2_" + newHue );
      Console.WriteLine( "Sat2_" + newSaturation );
      Console.WriteLine( "Bri2_" + newValue );

      newSaturation = Math.Max( 0, Math.Min( 1, newSaturation ) );
      newValue = Math.Max( 0, Math.Min( 1, newValue ) );

      Color outColor = FromAhsb( 255, newHue, newSaturation, newValue );
      Console.WriteLine( "R2_" + outColor.R );
      Console.WriteLine( "G2_" + outColor.G );
      Console.WriteLine( "B2_" + outColor.B );

      return FromAhsb( 255, newHue, newSaturation, newValue );
    }

    public struct GBAColor {
      public int R;
      public int G;
      public int B;
      public short CombinedValue {
        get {
          // Each component needs to be shifted to the proper place in the short
          return (short)( ( B << 10 ) | ( G << 5 ) | ( R << 0 ) );
        }
      }

      public GBAColor( Color initializerColor ) {
        if ( initializerColor == Color.Empty ) {
          initializerColor = Color.FromArgb( 0, 0, 0 );
        }

        // Only the top five bits are used for each color
        R = ( initializerColor.R & 0xF8 ) >> 3;
        G = ( initializerColor.G & 0xF8 ) >> 3;
        B = ( initializerColor.B & 0xF8 ) >> 3;
      }

      public GBAColor( int r, int g, int b ) {
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

      public Color ToColor() {
        return Color.FromArgb( R << 3, G << 3, B << 3 );
      }
    }
  }
}
