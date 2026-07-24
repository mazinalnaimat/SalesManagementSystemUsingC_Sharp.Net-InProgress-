using System.Drawing;
using System.Drawing.Drawing2D;

namespace SaleOrigin.UI.Controls.Drawing
{
    internal static class PasswordIcons
    {
        public static Bitmap CreateLock(Color color)
        {
            Bitmap bitmap = CreateBitmap();

            using (Graphics graphics = Graphics.FromImage(bitmap))
            using (Pen pen = CreatePen(color))
            {
                Prepare(graphics);
                graphics.DrawArc(pen, 6, 2, 12, 13, 180, 180);
                DrawRoundedRectangle(graphics, pen, new Rectangle(4, 10, 16, 12), 2);
                graphics.DrawLine(pen, 12, 15, 12, 18);
            }

            return bitmap;
        }

        public static Bitmap CreateEye(Color color, bool passwordVisible)
        {
            Bitmap bitmap = CreateBitmap();

            using (Graphics graphics = Graphics.FromImage(bitmap))
            using (Pen pen = CreatePen(color))
            {
                Prepare(graphics);
                graphics.DrawArc(pen, 2, 7, 20, 12, 200, 140);
                graphics.DrawArc(pen, 2, 5, 20, 12, 20, 140);
                graphics.DrawEllipse(pen, 9, 8, 6, 6);

                if (!passwordVisible)
                {
                    graphics.DrawLine(pen, 4, 3, 20, 21);
                }
            }

            return bitmap;
        }

        private static Bitmap CreateBitmap()
        {
            return new Bitmap(24, 24);
        }

        private static Pen CreatePen(Color color)
        {
            Pen pen = new Pen(color, 1.8F);
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;
            pen.LineJoin = LineJoin.Round;
            return pen;
        }

        private static void Prepare(Graphics graphics)
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.Clear(Color.Transparent);
        }

        private static void DrawRoundedRectangle(
            Graphics graphics,
            Pen pen,
            Rectangle rectangle,
            int radius)
        {
            int diameter = radius * 2;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddArc(rectangle.Left, rectangle.Top, diameter, diameter, 180, 90);
                path.AddArc(rectangle.Right - diameter, rectangle.Top, diameter, diameter, 270, 90);
                path.AddArc(rectangle.Right - diameter, rectangle.Bottom - diameter, diameter, diameter, 0, 90);
                path.AddArc(rectangle.Left, rectangle.Bottom - diameter, diameter, diameter, 90, 90);
                path.CloseFigure();
                graphics.DrawPath(pen, path);
            }
        }
    }
}
