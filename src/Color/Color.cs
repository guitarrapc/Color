using System;
using System.Globalization;

namespace Color
{
    public struct Color
    {
        private const float EPSILON = 0.001f;

        public static readonly Color Empty;
        private uint color;
        public byte Alpha => (byte)((color >> 24) & 0xff);
        public byte Red => (byte)((color >> 16) & 0xff);
        public byte Green => (byte)((color >> 8) & 0xff);
        public byte Blue => (byte)((color) & 0xff);
        public float Hue
        {
            get
            {
                ToHsv(out var h, out var s, out var v);
                return h;
            }
        }

        /// <summary>
        /// ARGB
        /// </summary>
        /// <param name="value"></param>
        public Color(uint value) => color = value;
        /// <summary>
        /// RGBA
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        /// <param name="alpha"></param>
        public Color(byte red, byte green, byte blue, byte alpha) => color = (uint)((alpha << 24) | (red << 16) | (green << 8) | blue);
        /// <summary>
        /// RGB (A == 0xFF)
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        public Color(byte red, byte green, byte blue) => color = (0xff000000u | (uint)(red << 16) | (uint)(green << 8) | blue);

        private static float HueToRgb(float v1, float v2, float vH)
        {
            if (vH < 0f)
                vH += 1f;
            if (vH > 1f)
                vH -= 1f;

            if ((6f * vH) < 1f)
                return (v1 + (v2 - v1) * 6f * vH);
            if ((2f * vH) < 1f)
                return (v2);
            if ((3f * vH) < 2f)
                return (v1 + (v2 - v1) * ((2f / 3f) - vH) * 6f);
            return (v1);
        }

        public void ToHsv(out float h, out float s, out float v)
        {
            // RGB from 0 to 255
            var r = (Red / 255f);
            var g = (Green / 255f);
            var b = (Blue / 255f);

            var min = Math.Min(Math.Min(r, g), b); // min value of RGB
            var max = Math.Max(Math.Max(r, g), b); // max value of RGB
            var delta = max - min; // delta RGB value 

            // default to a gray, no chroma...
            h = 0;
            s = 0;
            v = max;

            // chromatic data...
            if (Math.Abs(delta) > EPSILON)
            {
                s = delta / max;

                var deltaR = (((max - r) / 6f) + (delta / 2f)) / delta;
                var deltaG = (((max - g) / 6f) + (delta / 2f)) / delta;
                var deltaB = (((max - b) / 6f) + (delta / 2f)) / delta;

                if (Math.Abs(r - max) < EPSILON)
                {
                    // r == max
                    h = deltaB - deltaG;
                }
                else if (Math.Abs(g - max) < EPSILON)
                {
                    // g == max
                    h = (1f / 3f) + deltaR - deltaB;
                }
                else
                {
                    // b == max
                    h = (2f / 3f) + deltaG - deltaR;
                }

                if (h < 0f)
                {
                    h += 1f;
                }
                if (h > 1f)
                {
                    h -= 1f;
                }
            }

            // convert to percentages
            h = h * 360f;
            s = s * 100f;
            v = v * 100f;
        }

        public static bool operator ==(Color left, Color right) => left.color == right.color;

        public static bool operator !=(Color left, Color right)=> !(left == right);

        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "#{0:x2}{1:x2}{2:x2}{3:x2}", Alpha, Red, Green, Blue);

        public override bool Equals(object other)
        {
            if (!(other is Color))
                return false;

            var c = (Color)other;
            return c.color == this.color;
        }
        public override int GetHashCode() => (int)color;
    }
}
