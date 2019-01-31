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

        public static Color FromName(string name) => knownColors.Value.TryGetValue(name, out var color) ? color : Color.Empty;
        public static bool IsKnownColor(this Color color)=> color != Color.Empty;
    }
}
