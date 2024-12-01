using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyMod
{

    public static class ColorUtils
    {

        public const string Blue = "#61B5C2";
        public const string Red = "#E18E8E";
        public const string Green = "#7FB536";
        public const string White = "#edf4ff";
        public const string MymessageColor = "#ffbd7a";

        public static string RColor = "#CC2743";  
        public static string WColor = "#EFEBBF";  
        public static string BColor = "#A05EAA"; 
        public static string PColor = "#40CCBE"; 
        public static string GetStatColor(RwbpType statType)
        {
            switch (statType)
            {
                case RwbpType.R:
                    return RColor;
                case RwbpType.W:
                    return WColor;
                case RwbpType.B:
                    return BColor;
                case RwbpType.P:
                    return PColor;
                default:
                    return White; 
            }
        }
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