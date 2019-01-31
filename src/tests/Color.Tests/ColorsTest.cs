using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Color.Tests
{
    public class ColorsTest
    {
        private static Lazy<Dictionary<string, Color>> knownColors = new Lazy<Dictionary<string, Color>>(() => typeof(Colors)
            .GetFields(BindingFlags.Static | BindingFlags.Public)
            .ToDictionary(kv => kv.Name, kv => (Color)kv.GetValue(null)));

        [Theory]
        [InlineData(0xFFF0F8FF, 0xF0, 0xF8, 0xFF, 0xFF)]
        public void ConstructorMatchAlpha(uint colorHex, byte r, byte g, byte b, byte a)
        {
            // AliceBlue
            new Color(colorHex).Is(new Color(r, g, b, a));
        }

        [Theory]
        [InlineData(0xFFF0F8FF, 0xF0, 0xF8, 0xFF)]
        public void ConstructorMatchOpaque(uint colorHex, byte r, byte g, byte b)
        {
            // AliceBlue
            new Color(colorHex).Is(new Color(r, g, b));
        }

        [Theory]
        [InlineData("Red", 0xFFFF0000)]
        [InlineData("Green", 0xFF008000)]
        [InlineData("Blue", 0xFF0000FF)]
        [InlineData("White", 0xFFFFFFFF)]
        [InlineData("Black", 0xFF000000)]
        [InlineData("Transparent", 0x00FFFFFF)]
        public void KnownColorMatch(string color, uint colorHex)
        {
            ColorKnown.FromName(color).Is(new Color(colorHex));
        }

        [Theory]
        [InlineData("Red")]
        [InlineData("Blue")]
        [InlineData("Green")]
        [InlineData("White")]
        [InlineData("Black")]
        public void HsvConversion(string color)
        {
            ColorKnown.FromName(color).ToHsv(out var h, out var s, out var v);
            Color.FromHsv(h, s, v).Is(ColorKnown.FromName(color));
        }

        [Theory]
        [InlineData("Red")]
        [InlineData("Blue")]
        [InlineData("Green")]
        [InlineData("White")]
        [InlineData("Black")]
        public void HslConversion(string color)
        {
            ColorKnown.FromName(color).ToHsl(out var h, out var s, out var l);
            Color.FromHsl(h, s, l).Is(ColorKnown.FromName(color));
        }
    }
}
