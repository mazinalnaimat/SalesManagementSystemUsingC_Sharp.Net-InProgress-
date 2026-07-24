using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SaleOrigin.UI.Controls
{
    [DefaultEvent("Click")]
    [ToolboxItem(true)]
    public class RoundedAlertControl : Control
    {
        private Image _alertIcon;
        private bool _showIcon = true;

        private float _borderThickness = 1.5f;
        private Color _borderColor = Color.FromArgb(255, 150, 150);

        private bool _useRoundedCorners = true;
        private int _radiusTopLeft = 12;
        private int _radiusTopRight = 12;
        private int _radiusBottomRight = 12;
        private int _radiusBottomLeft = 12;

        private int _iconSize = 22;
        private int _iconTextSpacing = 10;
        private Padding _contentPadding = new Padding(14, 8, 14, 8);

        private bool _useParentBackColorForCorners = true;
        private Color _cornerBackColor = SystemColors.Control;

        public RoundedAlertControl()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Opaque,
                true);

            DoubleBuffered = true;

            Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            Size = new Size(300, 48);

            BackColor = Color.FromArgb(255, 245, 245);
            ForeColor = Color.FromArgb(220, 53, 69);

            TabStop = false;
        }

        #region Appearance properties

        [Category("Appearance")]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(null)]
        public Image AlertIcon
        {
            get { return _alertIcon; }
            set
            {
                _alertIcon = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(true)]
        public bool ShowIcon
        {
            get { return _showIcon; }
            set
            {
                _showIcon = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(1.5f)]
        public float BorderThickness
        {
            get { return _borderThickness; }
            set
            {
                _borderThickness = value < 0F ? 0F : value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(true)]
        public bool UseRoundedCorners
        {
            get { return _useRoundedCorners; }
            set
            {
                _useRoundedCorners = value;
                Invalidate();
            }
        }

        [Category("Corners")]
        [DefaultValue(12)]
        public int RadiusTopLeft
        {
            get { return _radiusTopLeft; }
            set
            {
                _radiusTopLeft = NormalizeRadius(value);
                Invalidate();
            }
        }

        [Category("Corners")]
        [DefaultValue(12)]
        public int RadiusTopRight
        {
            get { return _radiusTopRight; }
            set
            {
                _radiusTopRight = NormalizeRadius(value);
                Invalidate();
            }
        }

        [Category("Corners")]
        [DefaultValue(12)]
        public int RadiusBottomRight
        {
            get { return _radiusBottomRight; }
            set
            {
                _radiusBottomRight = NormalizeRadius(value);
                Invalidate();
            }
        }

        [Category("Corners")]
        [DefaultValue(12)]
        public int RadiusBottomLeft
        {
            get { return _radiusBottomLeft; }
            set
            {
                _radiusBottomLeft = NormalizeRadius(value);
                Invalidate();
            }
        }

        [Category("Corners")]
        [DefaultValue(true)]
        [Description("Uses the parent background color to cover the removed corner pixels. This prevents another control behind the alert from appearing in the rounded corners.")]
        public bool UseParentBackColorForCorners
        {
            get { return _useParentBackColorForCorners; }
            set
            {
                _useParentBackColorForCorners = value;
                Invalidate();
            }
        }

        [Category("Corners")]
        [Description("Color painted in the removed rectangular corner areas when UseParentBackColorForCorners is false.")]
        public Color CornerBackColor
        {
            get { return _cornerBackColor; }
            set
            {
                _cornerBackColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(22)]
        public int IconSize
        {
            get { return _iconSize; }
            set
            {
                _iconSize = value < 8 ? 8 : value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(10)]
        public int IconTextSpacing
        {
            get { return _iconTextSpacing; }
            set
            {
                _iconTextSpacing = value < 0 ? 0 : value;
                Invalidate();
            }
        }

        [Category("Layout")]
        public Padding ContentPadding
        {
            get { return _contentPadding; }
            set
            {
                _contentPadding = value;
                Invalidate();
            }
        }

        #endregion

        #region Painting

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // Intentionally empty.
            // The whole rectangular area is painted in OnPaint so there are
            // no unpainted pixels around the rounded corners.
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.CompositingQuality = CompositingQuality.HighQuality;

            // IMPORTANT:
            // Paint the complete rectangle first. The control keeps a rectangular
            // WinForms region, so controls behind it cannot show through its corners.
            using (SolidBrush outsideBrush = new SolidBrush(GetCornerBackgroundColor()))
            {
                g.FillRectangle(outsideBrush, ClientRectangle);
            }

            if (Width <= 1 || Height <= 1)
            {
                base.OnPaint(e);
                return;
            }

            RectangleF fillRectangle = new RectangleF(
                0F,
                0F,
                Width - 1F,
                Height - 1F);

            using (GraphicsPath fillPath = CreateRoundedPath(
                fillRectangle,
                _radiusTopLeft,
                _radiusTopRight,
                _radiusBottomRight,
                _radiusBottomLeft))
            using (SolidBrush backgroundBrush = new SolidBrush(BackColor))
            {
                g.FillPath(backgroundBrush, fillPath);
            }

            DrawCompleteBorder(g);
            DrawContent(g);

            base.OnPaint(e);
        }

        private void DrawCompleteBorder(Graphics g)
        {
            if (_borderThickness <= 0F || BorderColor.A == 0)
                return;

            float maximumThickness = Math.Max(0F, Math.Min(Width, Height) - 2F);
            float thickness = Math.Min(_borderThickness, maximumThickness);

            if (thickness <= 0F)
                return;

            // Place the pen fully inside the client rectangle.
            // This prevents the top, left, right, or bottom border from being clipped.
            float inset = (thickness / 2F) + 0.5F;

            float borderWidth = Width - 1F - (inset * 2F);
            float borderHeight = Height - 1F - (inset * 2F);

            if (borderWidth <= 0F || borderHeight <= 0F)
                return;

            RectangleF borderRectangle = new RectangleF(
                inset,
                inset,
                borderWidth,
                borderHeight);

            int topLeft = ReduceRadiusForBorder(_radiusTopLeft, inset);
            int topRight = ReduceRadiusForBorder(_radiusTopRight, inset);
            int bottomRight = ReduceRadiusForBorder(_radiusBottomRight, inset);
            int bottomLeft = ReduceRadiusForBorder(_radiusBottomLeft, inset);

            using (GraphicsPath borderPath = CreateRoundedPath(
                borderRectangle,
                topLeft,
                topRight,
                bottomRight,
                bottomLeft))
            using (Pen borderPen = new Pen(BorderColor, thickness))
            {
                borderPen.Alignment = PenAlignment.Center;
                borderPen.LineJoin = LineJoin.Round;

                g.DrawPath(borderPen, borderPath);
            }
        }

        private void DrawContent(Graphics g)
        {
            int contentWidth = Width - _contentPadding.Left - _contentPadding.Right;
            int contentHeight = Height - _contentPadding.Top - _contentPadding.Bottom;

            if (contentWidth <= 0 || contentHeight <= 0)
                return;

            Rectangle contentRectangle = new Rectangle(
                _contentPadding.Left,
                _contentPadding.Top,
                contentWidth,
                contentHeight);

            int currentX = contentRectangle.Left;

            if (ShowIcon)
            {
                int actualIconSize = Math.Min(_iconSize, contentRectangle.Height);

                Rectangle iconRectangle = new Rectangle(
                    currentX,
                    contentRectangle.Top + ((contentRectangle.Height - actualIconSize) / 2),
                    actualIconSize,
                    actualIconSize);

                if (AlertIcon != null)
                {
                    g.DrawImage(AlertIcon, iconRectangle);
                    currentX += actualIconSize + _iconTextSpacing;

                }

            }

            int textWidth = contentRectangle.Right - currentX;
            if (textWidth <= 0)
                return;

            Rectangle textRectangle = new Rectangle(
                currentX,
                contentRectangle.Top,
                textWidth,
                contentRectangle.Height);

            TextFormatFlags flags = TextFormatFlags.Left |
                                    TextFormatFlags.VerticalCenter |
                                    TextFormatFlags.EndEllipsis |
                                    TextFormatFlags.NoPadding |
                                    TextFormatFlags.SingleLine;

            TextRenderer.DrawText(
                g,
                Text,
                Font,
                textRectangle,
                ForeColor,
                flags);
        }

        #endregion

        #region Event handling

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            Invalidate();
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            base.OnParentBackColorChanged(e);
            Invalidate();
        }

        #endregion

        #region Helpers

        private Color GetCornerBackgroundColor()
        {
            if (_useParentBackColorForCorners && Parent != null)
                return Parent.BackColor;

            return _cornerBackColor;
        }


        private int NormalizeRadius(int value)
        {
            return value < 0 ? 0 : value;
        }

        private int ReduceRadiusForBorder(int radius, float inset)
        {
            return Math.Max(0, (int)Math.Round(radius - inset));
        }

        private GraphicsPath CreateRoundedPath(
            RectangleF rectangle,
            int topLeftRadius,
            int topRightRadius,
            int bottomRightRadius,
            int bottomLeftRadius)
        {
            GraphicsPath path = new GraphicsPath();

            if (!_useRoundedCorners)
            {
                path.AddRectangle(rectangle);
                path.CloseFigure();
                return path;
            }

            float maximumRadius = Math.Max(
                0F,
                Math.Min(rectangle.Width, rectangle.Height) / 2F);

            float topLeft = Math.Min(Math.Max(0, topLeftRadius), maximumRadius);
            float topRight = Math.Min(Math.Max(0, topRightRadius), maximumRadius);
            float bottomRight = Math.Min(Math.Max(0, bottomRightRadius), maximumRadius);
            float bottomLeft = Math.Min(Math.Max(0, bottomLeftRadius), maximumRadius);

            float left = rectangle.Left;
            float top = rectangle.Top;
            float right = rectangle.Right;
            float bottom = rectangle.Bottom;

            path.StartFigure();

            // Top edge and top-right corner.
            path.AddLine(left + topLeft, top, right - topRight, top);

            if (topRight > 0F)
            {
                path.AddArc(
                    right - (topRight * 2F),
                    top,
                    topRight * 2F,
                    topRight * 2F,
                    270F,
                    90F);
            }
            else
            {
                path.AddLine(right, top, right, top);
            }

            // Right edge and bottom-right corner.
            path.AddLine(right, top + topRight, right, bottom - bottomRight);

            if (bottomRight > 0F)
            {
                path.AddArc(
                    right - (bottomRight * 2F),
                    bottom - (bottomRight * 2F),
                    bottomRight * 2F,
                    bottomRight * 2F,
                    0F,
                    90F);
            }
            else
            {
                path.AddLine(right, bottom, right, bottom);
            }

            // Bottom edge and bottom-left corner.
            path.AddLine(right - bottomRight, bottom, left + bottomLeft, bottom);

            if (bottomLeft > 0F)
            {
                path.AddArc(
                    left,
                    bottom - (bottomLeft * 2F),
                    bottomLeft * 2F,
                    bottomLeft * 2F,
                    90F,
                    90F);
            }
            else
            {
                path.AddLine(left, bottom, left, bottom);
            }

            // Left edge and top-left corner.
            path.AddLine(left, bottom - bottomLeft, left, top + topLeft);

            if (topLeft > 0F)
            {
                path.AddArc(
                    left,
                    top,
                    topLeft * 2F,
                    topLeft * 2F,
                    180F,
                    90F);
            }
            else
            {
                path.AddLine(left, top, left, top);
            }

            path.CloseFigure();
            return path;
        }

        #endregion
    }
}