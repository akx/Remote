using System;
using System.Drawing;

namespace Remote.UI
{
    internal static class UiUtil
    {
        internal static void ColorToHsv(Color color, out double hue, out double saturation, out double value)
        {
            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            hue = color.GetHue();
            saturation = (max == 0) ? 0 : 1d - (1d*min/max);
            value = max/255d;
        }

        internal static Color ColorFromHsv(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue/60))%6;
            double f = hue/60 - Math.Floor(hue/60);

            value = value*255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value*(1 - saturation));
            int q = Convert.ToInt32(value*(1 - f*saturation));
            int t = Convert.ToInt32(value*(1 - (1 - f)*saturation));

            switch (hi)
            {
                case 0:
                    return Color.FromArgb(255, v, t, p);
                case 1:
                    return Color.FromArgb(255, q, v, p);
                case 2:
                    return Color.FromArgb(255, p, v, t);
                case 3:
                    return Color.FromArgb(255, p, q, v);
                case 4:
                    return Color.FromArgb(255, t, p, v);
                default:
                    return Color.FromArgb(255, v, p, q);
            }
        }

		private static int Clamp(int value, int min, int max) {
			if (value < min) return min;
			if (value > max) return max;
			return value;
		}

		internal static Color Modulate(Color color, float multiplier) {
			return Color.FromArgb(
				color.A,
				Clamp((int) (color.R*multiplier), 0, 255),
				Clamp((int) (color.G*multiplier), 0, 255),
				Clamp((int) (color.B * multiplier), 0, 255)
			);
		}
    }
}