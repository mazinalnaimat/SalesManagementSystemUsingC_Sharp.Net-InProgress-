using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SaleOrigin.UI.Controls.Drawing
{
    internal static class RoundedRectangle
    {
        public static GraphicsPath Create(Rectangle rectangle, int radius)
        {
            return Create(
                new RectangleF(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height),
                radius,
                radius,
                radius,
                radius);
        }

        public static GraphicsPath Create(RectangleF rectangle, float radius)
        {
            return Create(rectangle, radius, radius, radius, radius);
        }

        public static GraphicsPath Create(
            RectangleF rectangle,
            float topLeftRadius,
            float topRightRadius,
            float bottomRightRadius,
            float bottomLeftRadius)
        {
            GraphicsPath path = new GraphicsPath();

            if (rectangle.Width <= 0F || rectangle.Height <= 0F)
            {
                return path;
            }

            float tl = Math.Max(0F, topLeftRadius);
            float tr = Math.Max(0F, topRightRadius);
            float br = Math.Max(0F, bottomRightRadius);
            float bl = Math.Max(0F, bottomLeftRadius);

            // Scale the radii when two neighboring corners are too large.
            float scale = 1F;

            if (tl + tr > rectangle.Width)
                scale = Math.Min(scale, rectangle.Width / (tl + tr));

            if (bl + br > rectangle.Width)
                scale = Math.Min(scale, rectangle.Width / (bl + br));

            if (tl + bl > rectangle.Height)
                scale = Math.Min(scale, rectangle.Height / (tl + bl));

            if (tr + br > rectangle.Height)
                scale = Math.Min(scale, rectangle.Height / (tr + br));

            tl *= scale;
            tr *= scale;
            br *= scale;
            bl *= scale;

            path.StartFigure();

            path.AddLine(
                rectangle.Left + tl,
                rectangle.Top,
                rectangle.Right - tr,
                rectangle.Top);

            AddCorner(
                path,
                rectangle.Right - (tr * 2F),
                rectangle.Top,
                tr,
                270F);

            path.AddLine(
                rectangle.Right,
                rectangle.Top + tr,
                rectangle.Right,
                rectangle.Bottom - br);

            AddCorner(
                path,
                rectangle.Right - (br * 2F),
                rectangle.Bottom - (br * 2F),
                br,
                0F);

            path.AddLine(
                rectangle.Right - br,
                rectangle.Bottom,
                rectangle.Left + bl,
                rectangle.Bottom);

            AddCorner(
                path,
                rectangle.Left,
                rectangle.Bottom - (bl * 2F),
                bl,
                90F);

            path.AddLine(
                rectangle.Left,
                rectangle.Bottom - bl,
                rectangle.Left,
                rectangle.Top + tl);

            AddCorner(
                path,
                rectangle.Left,
                rectangle.Top,
                tl,
                180F);

            path.CloseFigure();
            return path;
        }

        private static void AddCorner(
            GraphicsPath path,
            float x,
            float y,
            float radius,
            float startAngle)
        {
            if (radius <= 0.01F)
            {
                return;
            }

            float diameter = radius * 2F;
            path.AddArc(x, y, diameter, diameter, startAngle, 90F);
        }
    }
}
