using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Color
{
    public static class ColorKnown
    {
        private static Lazy<Dictionary<string, Color>> knownColors = new Lazy<Dictionary<string, Color>>(() => typeof(Colors)
            .GetFields(BindingFlags.Static | BindingFlags.Public)
            .ToDictionary(kv => kv.Name, kv => (Color)kv.GetValue(null)));

        public static Color FromName(string nameOrHex) => FromName(nameOrHex, Color.Empty);
        public static Color FromName(string nameOrHex, Color defaultColor)
        {
            if (knownColors.Value.TryGetValue(nameOrHex, out var color))
            {
                return color;
            }
            else
            {
                var hex = nameOrHex.StartsWith("#") 
                    ? nameOrHex.Substring(1, nameOrHex.Length -1) 
                    : nameOrHex;
                try
                {
                    return new Color(Convert.ToUInt32(hex, 16));
                }
                catch (Exception)
                {
                    return defaultColor;
                }
            }
        }
        public static bool IsKnownColor(this Color color) => color != Color.Empty;
    }
}
