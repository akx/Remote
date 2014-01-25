﻿using System.Drawing;

namespace Remote.UI
{
    internal static class UIUtil
    {
        internal static Color FromHsl(double h, double s, double l)
        {
            double r, g, b;

            r = l;
            g = l;
            b = l;
            double v = (l <= 0.5) ? (l*(1.0 + s)) : (l + s - l*s);
            if (v > 0)
            {
                var m = l + l - v;
                var sv = (v - m)/v;
                h *= 6.0;
                var sextant = (int) h;
                var fract = h - sextant;
                var vsf = v*sv*fract;
                var mid1 = m + vsf;
                var mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }
            return Color.FromArgb(
                (byte) (r*255.0f),
                (byte) (g*255.0f),
                (byte) (b*255.0f)
                );
        }
    }
}