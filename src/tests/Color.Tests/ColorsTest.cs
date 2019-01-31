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

        [Fact]
        public void ConstructorMatch()
        {
            // AliceBlue
            new Color(0xFFF0F8FF).Is(new Color(0xF0, 0xF8, 0xFF, 0xFF));
            new Color(0xFFF0F8FF).Is(new Color(0xF0, 0xF8, 0xFF));
        }

        [Fact]
        public void KnownColorMatch()
        {
            ColorKnown.FromName("Red").Is(new Color(0xFFFF0000));
            ColorKnown.FromName("Green").Is(new Color(0xFF008000));
            ColorKnown.FromName("Blue").Is(new Color(0xFF0000FF));
            ColorKnown.FromName("White").Is(new Color(0xFFFFFFFF));
            ColorKnown.FromName("Black").Is(new Color(0xFF000000));
            ColorKnown.FromName("Transparent").Is(new Color(0x00FFFFFF));
        }
    }
}
