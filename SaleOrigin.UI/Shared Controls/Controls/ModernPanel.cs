using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SaleOrigin.UI.Controls.Drawing;

namespace SaleOrigin.UI.Controls
{
    /// <summary>
    /// Reusable WinForms UserControl with smooth rounded corners, borders,
    /// gradient background, and an optional soft shadow.
    /// </summary>
    [ToolboxItem(true)]
    [DefaultEvent(nameof(Click))]
    [DesignerCategory("Code")]
    public class ModernPanel : UserControl
    {
        private Color _fillColor = Color.FromArgb(45, 45, 55);
        private Color _fillColor2 = Color.FromArgb(75, 65, 145);
        private bool _useGradient = true;
        private float _gradientAngle = 45F;

        private Color _borderColor = Color.FromArgb(110, 100, 230);
        private int _borderThickness = 1;
        private int _borderRadius = 18;

        private bool _shadowEnabled;
        private Color _shadowColor = Color.Black;
        private int _shadowOpacity = 75;
        private int _shadowDepth = 6;
        private int _shadowBlur = 7;

        private Region _controlRegion;

        public ModernPanel()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor,
                true);

            DoubleBuffered = true;
            BackColor = Color.Transparent;
            Size = new Size(300, 180);
            Padding = new Padding(12);
            UpdateControlRegion();
        }

        [Category("Modern Panel - Background")]
        [Description("The first background color.")]
        [DefaultValue(typeof(Color), "45, 45, 55")]
        public Color FillColor
        {
            get { return _fillColor; }
            set
            {
                if (_fillColor == value)
                {
                    return;
                }

                _fillColor = value;
                Invalidate();
            }
        }

        [Category("Modern Panel - Background")]
        [Description("The second color used by the gradient.")]
        [DefaultValue(typeof(Color), "75, 65, 145")]
        public Color FillColor2
        {
            get { return _fillColor2; }
            set
            {
                if (_fillColor2 == value)
                {
                    return;
                }

                _fillColor2 = value;
                Invalidate();
            }
        }

        [Category("Modern Panel - Background")]
        [Description("Enables or disables the gradient background.")]
        [DefaultValue(true)]
        public bool UseGradient
        {
            get { return _useGradient; }
            set
            {
                if (_useGradient == value)
                {
                    return;
                }

                _useGradient = value;
                Invalidate();
            }
        }

        [Category("Modern Panel - Background")]
        [Description("The gradient angle in degrees.")]
        [DefaultValue(45F)]
        public float GradientAngle
        {
            get { return _gradientAngle; }
            set
            {
                if (Math.Abs(_gradientAngle - value) < 0.001F)
                {
                    return;
                }

                _gradientAngle = value;
                Invalidate();
            }
        }

        [Category("Modern Panel - Border")]
        [Description("The panel border color.")]
        [DefaultValue(typeof(Color), "110, 100, 230")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                if (_borderColor == value)
                {
                    return;
                }

                _borderColor = value;
                Invalidate();
            }
        }

        [Category("Modern Panel - Border")]
        [Description("The border thickness in pixels. Set to 0 to hide it.")]
        [DefaultValue(1)]
        public int BorderThickness
        {
            get { return _borderThickness; }
            set
            {
                int newValue = Math.Max(0, value);
                if (_borderThickness == newValue)
                {
                    return;
                }

                _borderThickness = newValue;
                Invalidate();
            }
        }

        [Category("Modern Panel - Border")]
        [Description("The corner radius in pixels. Set to 0 for square corners.")]
        [DefaultValue(18)]
        public int BorderRadius
        {
            get { return _borderRadius; }
            set
            {
                int newValue = Math.Max(0, value);
                if (_borderRadius == newValue)
                {
                    return;
                }

                _borderRadius = newValue;
                UpdateControlRegion();
                Invalidate();
            }
        }

        [Category("Modern Panel - Shadow")]
        [Description("Shows or hides the panel shadow.")]
        [DefaultValue(false)]
        public bool ShadowEnabled
        {
            get { return _shadowEnabled; }
            set
            {
                if (_shadowEnabled == value)
                {
                    return;
                }

                _shadowEnabled = value;
                UpdateControlRegion();
                Invalidate();
            }
        }

        [Category("Modern Panel - Shadow")]
        [Description("The shadow color.")]
        [DefaultValue(typeof(Color), "Black")]
        public Color ShadowColor
        {
            get { return _shadowColor; }
            set
            {
                if (_shadowColor == value)
                {
                    return;
                }

                _shadowColor = value;
                Invalidate();
            }
        }

        [Category("Modern Panel - Shadow")]
        [Description("The shadow opacity from 0 to 255.")]
        [DefaultValue(75)]
        public int ShadowOpacity
        {
            get { return _shadowOpacity; }
            set
            {
                int newValue = Limit(value, 0, 255);
                if (_shadowOpacity == newValue)
                {
                    return;
                }

                _shadowOpacity = newValue;
                UpdateControlRegion();
                Invalidate();
            }
        }

        [Category("Modern Panel - Shadow")]
        [Description("The shadow offset in pixels.")]
        [DefaultValue(6)]
        public int ShadowDepth
        {
            get { return _shadowDepth; }
            set
            {
                int newValue = Math.Max(0, value);
                if (_shadowDepth == newValue)
                {
                    return;
                }

                _shadowDepth = newValue;
                UpdateControlRegion();
                Invalidate();
            }
        }

        [Category("Modern Panel - Shadow")]
        [Description("The approximate shadow softness in pixels.")]
        [DefaultValue(7)]
        public int ShadowBlur
        {
            get { return _shadowBlur; }
            set
            {
                int newValue = Limit(value, 0, 25);
                if (_shadowBlur == newValue)
                {
                    return;
                }

                _shadowBlur = newValue;
                UpdateControlRegion();
                Invalidate();
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateControlRegion();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateControlRegion();
            Invalidate();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            UpdateControlRegion();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Region = null;

                if (_controlRegion != null)
                {
                    _controlRegion.Dispose();
                    _controlRegion = null;
                }
            }

            base.Dispose(disposing);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (HasRoundedCorners() && Parent != null)
            {
                PaintBackgroundBehindControl(e.Graphics);
                return;
            }

            base.OnPaintBackground(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (Width <= 1 || Height <= 1)
            {
                return;
            }

            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            RectangleF panelRectangle = GetPanelRectangle();
            if (panelRectangle.Width <= 0F || panelRectangle.Height <= 0F)
            {
                return;
            }

            if (_shadowEnabled && _shadowOpacity > 0)
            {
                DrawShadow(graphics, panelRectangle);
            }

            using (GraphicsPath panelPath = RoundedRectangle.Create(
                panelRectangle,
                _borderRadius))
            {
                if (_useGradient && _fillColor != _fillColor2)
                {
                    using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                        panelRectangle,
                        _fillColor,
                        _fillColor2,
                        _gradientAngle,
                        true))
                    {
                        graphics.FillPath(gradientBrush, panelPath);
                    }
                }
                else
                {
                    using (SolidBrush solidBrush = new SolidBrush(_fillColor))
                    {
                        graphics.FillPath(solidBrush, panelPath);
                    }
                }
            }

            if (_borderThickness > 0 && _borderColor != Color.Transparent)
            {
                float inset = _borderThickness / 2F;
                RectangleF borderRectangle = RectangleF.Inflate(
                    panelRectangle,
                    -inset,
                    -inset);

                if (borderRectangle.Width > 0F && borderRectangle.Height > 0F)
                {
                    float adjustedRadius = Math.Max(0F, _borderRadius - inset);

                    using (GraphicsPath borderPath = RoundedRectangle.Create(
                        borderRectangle,
                        adjustedRadius))
                    using (Pen borderPen = new Pen(_borderColor, _borderThickness))
                    {
                        borderPen.Alignment = PenAlignment.Center;
                        graphics.DrawPath(borderPen, borderPath);
                    }
                }
            }
        }

        private RectangleF GetPanelRectangle()
        {
            const float inset = 1F;

            int reservedShadowSpace = _shadowEnabled
                ? _shadowDepth + Math.Max(1, _shadowBlur / 2)
                : 0;

            return new RectangleF(
                inset,
                inset,
                Math.Max(0F, Width - reservedShadowSpace - (inset * 2F)),
                Math.Max(0F, Height - reservedShadowSpace - (inset * 2F)));
        }

        private void DrawShadow(Graphics graphics, RectangleF panelRectangle)
        {
            if (_shadowBlur > 0)
            {
                for (int layer = _shadowBlur; layer >= 1; layer--)
                {
                    float expansion = layer * 0.55F;

                    RectangleF shadowRectangle = new RectangleF(
                        panelRectangle.X + _shadowDepth - expansion,
                        panelRectangle.Y + _shadowDepth - expansion,
                        panelRectangle.Width + (expansion * 2F),
                        panelRectangle.Height + (expansion * 2F));

                    int alpha = Math.Max(
                        1,
                        _shadowOpacity / Math.Max(2, _shadowBlur + 2));

                    using (GraphicsPath shadowPath = RoundedRectangle.Create(
                        shadowRectangle,
                        _borderRadius + expansion))
                    using (SolidBrush shadowBrush = new SolidBrush(
                        Color.FromArgb(alpha, _shadowColor)))
                    {
                        graphics.FillPath(shadowBrush, shadowPath);
                    }
                }
            }

            RectangleF coreShadowRectangle = new RectangleF(
                panelRectangle.X + _shadowDepth,
                panelRectangle.Y + _shadowDepth,
                panelRectangle.Width,
                panelRectangle.Height);

            using (GraphicsPath coreShadowPath = RoundedRectangle.Create(
                coreShadowRectangle,
                _borderRadius))
            using (SolidBrush coreShadowBrush = new SolidBrush(
                Color.FromArgb(Math.Max(1, _shadowOpacity / 3), _shadowColor)))
            {
                graphics.FillPath(coreShadowBrush, coreShadowPath);
            }
        }

        private void UpdateControlRegion()
        {
            Region newRegion = null;

            if (Width > 0 && Height > 0 && HasRoundedCorners())
            {
                const float antiAliasEnvelope = 1F;
                int reservedShadowSpace = _shadowEnabled
                    ? _shadowDepth + Math.Max(1, _shadowBlur / 2)
                    : 0;

                RectangleF bodyRegionRectangle = new RectangleF(
                    0F,
                    0F,
                    Math.Max(1F, Width - reservedShadowSpace),
                    Math.Max(1F, Height - reservedShadowSpace));

                using (GraphicsPath bodyPath = RoundedRectangle.Create(
                    bodyRegionRectangle,
                    _borderRadius + antiAliasEnvelope))
                {
                    newRegion = new Region(bodyPath);
                }

                if (_shadowEnabled && _shadowOpacity > 0 && newRegion != null)
                {
                    float expansion = (_shadowBlur * 0.55F) + antiAliasEnvelope;
                    RectangleF panelRectangle = GetPanelRectangle();
                    RectangleF shadowEnvelope = new RectangleF(
                        panelRectangle.X + _shadowDepth - expansion,
                        panelRectangle.Y + _shadowDepth - expansion,
                        panelRectangle.Width + (expansion * 2F),
                        panelRectangle.Height + (expansion * 2F));

                    using (GraphicsPath shadowPath = RoundedRectangle.Create(
                        shadowEnvelope,
                        _borderRadius + expansion + antiAliasEnvelope))
                    {
                        newRegion.Union(shadowPath);
                    }
                }
            }

            Region oldRegion = _controlRegion;
            _controlRegion = newRegion;
            Region = newRegion;

            if (oldRegion != null)
            {
                oldRegion.Dispose();
            }

            if (Parent != null)
            {
                Parent.Invalidate(Bounds, true);
            }
        }

        private bool HasRoundedCorners()
        {
            return _borderRadius > 0;
        }

        private void PaintBackgroundBehindControl(Graphics graphics)
        {
            if (Parent == null)
            {
                return;
            }

            GraphicsState parentState = graphics.Save();

            try
            {
                graphics.TranslateTransform(-Left, -Top);

                using (PaintEventArgs parentPaint = new PaintEventArgs(graphics, Bounds))
                {
                    InvokePaintBackground(Parent, parentPaint);
                    InvokePaint(Parent, parentPaint);
                }
            }
            finally
            {
                graphics.Restore(parentState);
            }

            int controlIndex = Parent.Controls.GetChildIndex(this);

            for (int index = Parent.Controls.Count - 1;
                 index > controlIndex;
                 index--)
            {
                Control sibling = Parent.Controls[index];

                if (!sibling.Visible ||
                    sibling.Width <= 0 ||
                    sibling.Height <= 0 ||
                    !sibling.Bounds.IntersectsWith(Bounds))
                {
                    continue;
                }

                Rectangle overlap = Rectangle.Intersect(Bounds, sibling.Bounds);

                try
                {
                    using (Bitmap bitmap = new Bitmap(sibling.Width, sibling.Height))
                    {
                        sibling.DrawToBitmap(
                            bitmap,
                            new Rectangle(Point.Empty, sibling.Size));

                        Rectangle source = new Rectangle(
                            overlap.Left - sibling.Left,
                            overlap.Top - sibling.Top,
                            overlap.Width,
                            overlap.Height);

                        Rectangle destination = new Rectangle(
                            overlap.Left - Left,
                            overlap.Top - Top,
                            overlap.Width,
                            overlap.Height);

                        graphics.DrawImage(
                            bitmap,
                            destination,
                            source,
                            GraphicsUnit.Pixel);
                    }
                }
                catch (ArgumentException)
                {
                    // Some native controls cannot be captured with DrawToBitmap.
                }
                catch (InvalidOperationException)
                {
                    // The sibling may be changing or disposing during repaint.
                }
            }
        }

        private static int Limit(int value, int minimum, int maximum)
        {
            if (value < minimum)
            {
                return minimum;
            }

            if (value > maximum)
            {
                return maximum;
            }

            return value;
        }
    }
}
