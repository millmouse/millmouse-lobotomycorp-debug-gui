using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MyMod
{

    public static class ColorUtils
    {

        public const string Blue = "#0000FF";
        public const string Red = "#FF0000";
        public const string Green = "#00FF00";
        public const string Gold = "#FFD700";
        public const string White = "#edf4ff";

        public static string RColor = "#FF4500";  
        public static string WColor = "#4169E1";  
        public static string BColor = "#32CD32"; 
        public static string PColor = "#8A2BE2"; 
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
                    return Gold;  // Default color if no match
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