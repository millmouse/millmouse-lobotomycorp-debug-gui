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

        public static string StrengthColor = "#FF4500";  // OrangeRed for Strength
        public static string IntelligenceColor = "#4169E1";  // RoyalBlue for Intelligence
        public static string DexterityColor = "#32CD32";  // LimeGreen for Dexterity
        public static string VitalityColor = "#8A2BE2";  // BlueViolet for Vitality
        public static string GetStatColor(RwbpType statType)
        {
            switch (statType)
            {
                case RwbpType.R:
                    return StrengthColor;
                case RwbpType.W:
                    return IntelligenceColor;
                case RwbpType.B:
                    return DexterityColor;
                case RwbpType.P:
                    return VitalityColor;
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