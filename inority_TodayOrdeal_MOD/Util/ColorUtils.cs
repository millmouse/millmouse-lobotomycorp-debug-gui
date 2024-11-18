using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SageMod.Util
{

    public static class ColorUtils
    {
        public static Color HexToColor(string hex)
        {
            hex = hex.Replace("#", "");

            if (hex.Length == 6)
            {
                byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                return new Color(r / 255f, g / 255f, b / 255f);
            }
            else if (hex.Length == 8)
            {
                byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                byte a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
            }
            else
            {
                Debug.LogError("Invalid hex code length!");
                return Color.white;
            }
        }
    }

}