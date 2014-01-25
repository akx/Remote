using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace Remote.UI
{
    internal static class IconGenerator
    {
        internal static Image GenerateIcon(Color background, string text, int size = 16)
        {
            var foreground = background.GetBrightness() < 0.6 ? Color.White : Color.Black;
            var bmp = new Bitmap(size, size, PixelFormat.Format32bppPArgb);

            using (var font = new Font("Tahoma", size - 6, FontStyle.Bold))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.TextRenderingHint = TextRenderingHint.AntiAlias;
                    g.FillRectangle(new SolidBrush(background), 0, 0, bmp.Width, bmp.Height);
                    var measure = g.MeasureString(text, font);
                    g.DrawString(text, font, new SolidBrush(foreground), (bmp.Width - measure.Width)*0.5f,
                        (bmp.Height - measure.Height)*0.5f);
                }
            }
            return bmp;
        }
    }
}