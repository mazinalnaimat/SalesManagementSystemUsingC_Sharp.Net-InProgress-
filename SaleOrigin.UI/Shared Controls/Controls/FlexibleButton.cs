using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SaleOrigin.UI.Controls.Drawing;

namespace SaleOrigin.UI.Controls
{
    [DefaultEvent("Click")]
    [DefaultProperty("Text")]
    [DesignerCategory("Code")]
    public class FlexibleButton : Control, IButtonControl
    {
        private readonly Timer _animationTimer;
        private Region _controlRegion;

        private bool _mouseOver;
        private bool _mouseDown;
        private bool _keyboardPressed;
        private bool _isDefault;
        private float _hoverProgress;
        private float _hoverTarget;

        private Color _startColor = Color.FromArgb(0, 102, 255);
        private Color _endColor = Color.FromArgb(0, 188, 212);
        private Color _hoverStartColor = Color.FromArgb(0, 91, 235);
        private Color _hoverEndColor = Color.FromArgb(0, 174, 199);
        private Color _pressedStartColor = Color.FromArgb(0, 78, 205);
        private Color _pressedEndColor = Color.FromArgb(0, 148, 174);
        private Color _disabledStartColor = Color.FromArgb(211, 217, 224);
        private Color _disabledEndColor = Color.FromArgb(190, 198, 207);
        private Color _borderColor = Color.Transparent;
        private Color _focusBorderColor = Color.FromArgb(130, 255, 255, 255);
        private Color _hoverTextColor = Color.White;
        private Color _pressedTextColor = Color.White;
        private Color _disabledTextColor = Color.FromArgb(120, 128, 138);

        private int _topLeftRadius = 10;
        private int _topRightRadius = 10;
        private int _bottomRightRadius = 10;
        private int _bottomLeftRadius = 10;
        private int _borderThickness;
        private float _gradientAngle;
        private bool _useGradient = true;
        private bool _showFocusBorder;
        private int _highlightOpacity = 18;
        private float _animationStep = 0.14F;
        private int _imageSpacing = 8;
        private Size _imageSize = new Size(20, 20);
        private Image _image;
        private ContentAlignment _imageAlign = ContentAlignment.MiddleCenter;
        private ContentAlignment _textAlign = ContentAlignment.MiddleCenter;
        private TextImageRelation _textImageRelation = TextImageRelation.ImageBeforeText;
        private DialogResult _dialogResult = DialogResult.None;

        public FlexibleButton()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.UserPaint |
                ControlStyles.Selectable,
                true);

            DoubleBuffered = true;
            BackColor = Color.Transparent;
            ForeColor = Color.White;
            Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point);
            Cursor = Cursors.Hand;
            Size = new Size(180, 44);
            MinimumSize = new Size(30, 24);
            TabStop = true;

            _animationTimer = new Timer
            {
                Interval = 15
            };

            _animationTimer.Tick += AnimationTimer_Tick;
            UpdateControlRegion();
        }

        [Category("Gradient")]
        public Color StartColor
        {
            get { return _startColor; }
            set { _startColor = value; Invalidate(); }
        }

        [Category("Gradient")]
        public Color EndColor
        {
            get { return _endColor; }
            set { _endColor = value; Invalidate(); }
        }

        [Category("Gradient")]
        public Color HoverStartColor
        {
            get { return _hoverStartColor; }
            set { _hoverStartColor = value; Invalidate(); }
        }

        [Category("Gradient")]
        public Color HoverEndColor
        {
            get { return _hoverEndColor; }
            set { _hoverEndColor = value; Invalidate(); }
        }

        [Category("Gradient")]
        public Color PressedStartColor
        {
            get { return _pressedStartColor; }
            set { _pressedStartColor = value; Invalidate(); }
        }

        [Category("Gradient")]
        public Color PressedEndColor
        {
            get { return _pressedEndColor; }
            set { _pressedEndColor = value; Invalidate(); }
        }

        [Category("Gradient")]
        public Color DisabledStartColor
        {
            get { return _disabledStartColor; }
            set { _disabledStartColor = value; Invalidate(); }
        }

        [Category("Gradient")]
        public Color DisabledEndColor
        {
            get { return _disabledEndColor; }
            set { _disabledEndColor = value; Invalidate(); }
        }

        [Category("Gradient")]
        [DefaultValue(0F)]
        public float GradientAngle
        {
            get { return _gradientAngle; }
            set { _gradientAngle = value; Invalidate(); }
        }

        [Category("Gradient")]
        [DefaultValue(true)]
        public bool UseGradient
        {
            get { return _useGradient; }
            set { _useGradient = value; Invalidate(); }
        }

        [Category("Corners")]
        [DefaultValue(10)]
        [RefreshProperties(RefreshProperties.All)]
        [Description("Sets the same radius for all four corners.")]
        public int BorderRadius
        {
            get
            {
                if (_topLeftRadius == _topRightRadius &&
                    _topLeftRadius == _bottomRightRadius &&
                    _topLeftRadius == _bottomLeftRadius)
                {
                    return _topLeftRadius;
                }

                return 0;
            }
            set
            {
                int radius = Math.Max(0, value);
                _topLeftRadius = radius;
                _topRightRadius = radius;
                _bottomRightRadius = radius;
                _bottomLeftRadius = radius;
                OnShapeChanged();
            }
        }

        [Category("Corners")]
        [DefaultValue(10)]
        public int TopLeftRadius
        {
            get { return _topLeftRadius; }
            set { _topLeftRadius = Math.Max(0, value); OnShapeChanged(); }
        }

        [Category("Corners")]
        [DefaultValue(10)]
        public int TopRightRadius
        {
            get { return _topRightRadius; }
            set { _topRightRadius = Math.Max(0, value); OnShapeChanged(); }
        }

        [Category("Corners")]
        [DefaultValue(10)]
        public int BottomRightRadius
        {
            get { return _bottomRightRadius; }
            set { _bottomRightRadius = Math.Max(0, value); OnShapeChanged(); }
        }

        [Category("Corners")]
        [DefaultValue(10)]
        public int BottomLeftRadius
        {
            get { return _bottomLeftRadius; }
            set { _bottomLeftRadius = Math.Max(0, value); OnShapeChanged(); }
        }

        [Category("Appearance")]
        [DefaultValue(0)]
        public int BorderThickness
        {
            get { return _borderThickness; }
            set { _borderThickness = Math.Max(0, value); Invalidate(); }
        }

        [Category("Appearance")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set { _borderColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color FocusBorderColor
        {
            get { return _focusBorderColor; }
            set { _focusBorderColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        public bool ShowFocusBorder
        {
            get { return _showFocusBorder; }
            set { _showFocusBorder = value; Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(18)]
        public int HighlightOpacity
        {
            get { return _highlightOpacity; }
            set
            {
                _highlightOpacity = Math.Max(0, Math.Min(80, value));
                Invalidate();
            }
        }

        [Category("Behavior")]
        [DefaultValue(0.14F)]
        public float AnimationStep
        {
            get { return _animationStep; }
            set { _animationStep = Math.Max(0.01F, Math.Min(1F, value)); }
        }

        [Category("Appearance")]
        public Color HoverTextColor
        {
            get { return _hoverTextColor; }
            set { _hoverTextColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color PressedTextColor
        {
            get { return _pressedTextColor; }
            set { _pressedTextColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        public Color DisabledTextColor
        {
            get { return _disabledTextColor; }
            set { _disabledTextColor = value; Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(null)]
        public Image Image
        {
            get { return _image; }
            set { _image = value; Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(ContentAlignment.MiddleCenter)]
        public ContentAlignment ImageAlign
        {
            get { return _imageAlign; }
            set { _imageAlign = value; Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(ContentAlignment.MiddleCenter)]
        public ContentAlignment TextAlign
        {
            get { return _textAlign; }
            set { _textAlign = value; Invalidate(); }
        }

        [Category("Appearance")]
        [DefaultValue(TextImageRelation.ImageBeforeText)]
        public TextImageRelation TextImageRelation
        {
            get { return _textImageRelation; }
            set { _textImageRelation = value; Invalidate(); }
        }

        [Category("Layout")]
        [DefaultValue(8)]
        public int ImageSpacing
        {
            get { return _imageSpacing; }
            set { _imageSpacing = Math.Max(0, value); Invalidate(); }
        }

        [Category("Layout")]
        public Size ImageSize
        {
            get { return _imageSize; }
            set
            {
                _imageSize = new Size(Math.Max(1, value.Width), Math.Max(1, value.Height));
                Invalidate();
            }
        }

        [Category("Behavior")]
        [DefaultValue(DialogResult.None)]
        public DialogResult DialogResult
        {
            get { return _dialogResult; }
            set { _dialogResult = value; }
        }

        public void NotifyDefault(bool value)
        {
            _isDefault = value;
            Invalidate();
        }

        public void PerformClick()
        {
            if (CanSelect && Enabled)
            {
                OnClick(EventArgs.Empty);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _animationTimer.Stop();
                _animationTimer.Dispose();

                Region = null;

                if (_controlRegion != null)
                {
                    _controlRegion.Dispose();
                    _controlRegion = null;
                }
            }

            base.Dispose(disposing);
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

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Cursor = Enabled ? Cursors.Hand : Cursors.Default;

            if (!Enabled)
            {
                _animationTimer.Stop();
                _hoverProgress = 0F;
                _hoverTarget = 0F;
                _mouseDown = false;
                _keyboardPressed = false;
            }

            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _mouseOver = true;
            StartHoverAnimation(1F);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _mouseOver = false;
            _mouseDown = false;
            Capture = false;
            StartHoverAnimation(0F);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left &&
                Enabled &&
                IsPointInsideShape(e.Location))
            {
                Focus();
                _mouseDown = true;
                Capture = true;
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            bool shouldClick =
                _mouseDown &&
                e.Button == MouseButtons.Left &&
                IsPointInsideShape(e.Location);

            _mouseDown = false;
            Capture = false;
            Invalidate();

            base.OnMouseUp(e);

            if (shouldClick)
            {
                OnClick(EventArgs.Empty);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if ((e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter) && !_keyboardPressed)
            {
                _keyboardPressed = true;
                e.Handled = true;
                Invalidate();
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            bool shouldClick =
                _keyboardPressed &&
                (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter);

            _keyboardPressed = false;
            Invalidate();
            base.OnKeyUp(e);

            if (shouldClick)
            {
                e.Handled = true;
                PerformClick();
            }
        }

        protected override bool IsInputKey(Keys keyData)
        {
            Keys keyCode = keyData & Keys.KeyCode;

            if (keyCode == Keys.Space || keyCode == Keys.Enter)
            {
                return true;
            }

            return base.IsInputKey(keyData);
        }

        protected override void OnClick(EventArgs e)
        {
            Form form = FindForm();

            if (form != null && _dialogResult != DialogResult.None)
            {
                form.DialogResult = _dialogResult;
            }

            base.OnClick(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // A normal transparent WinForms control only repaints its parent. It does
            // not include sibling controls that are behind it. For a rounded button
            // that would leave a rectangular or wrong-colour halo around the smooth
            // anti-aliased edge. Paint the real background first, including any
            // overlapping siblings behind this button.
            if (HasRoundedCorners() && Parent != null)
            {
                PaintBackgroundBehindButton(e.Graphics);
                return;
            }

            base.OnPaintBackground(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graphics = e.Graphics;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.TextRenderingHint =
                System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // Keep the visible path one pixel inside the native Region. A Windows
            // Region has a hard, non-antialiased edge, so drawing directly on that
            // edge clips the smoothing pixels and makes the corners look stepped.
            // The small safety envelope in UpdateControlRegion keeps those smoothing
            // pixels alive while still removing the original rectangular corners.
            RectangleF buttonRectangle = GetVisibleButtonRectangle();

            Color start;
            Color end;
            Color textColor;

            bool pressed = _mouseDown || _keyboardPressed;

            if (!Enabled)
            {
                start = _disabledStartColor;
                end = _disabledEndColor;
                textColor = _disabledTextColor;
            }
            else if (pressed)
            {
                start = _pressedStartColor;
                end = _pressedEndColor;
                textColor = _pressedTextColor;
            }
            else
            {
                start = Blend(_startColor, _hoverStartColor, _hoverProgress);
                end = Blend(_endColor, _hoverEndColor, _hoverProgress);
                textColor = Blend(ForeColor, _hoverTextColor, _hoverProgress);
            }

            using (GraphicsPath path = RoundedRectangle.Create(
                buttonRectangle,
                _topLeftRadius,
                _topRightRadius,
                _bottomRightRadius,
                _bottomLeftRadius))
            {
                PaintButtonFill(graphics, path, buttonRectangle, start, end);
                PaintHighlight(graphics, path, buttonRectangle);
                PaintBorder(graphics, path);
            }

            PaintFocusBorder(graphics, buttonRectangle);

            Rectangle contentBounds = Rectangle.Round(buttonRectangle);

            if (pressed)
            {
                contentBounds.Offset(0, 1);
            }

            DrawContent(graphics, contentBounds, textColor);
        }

        private void PaintBackgroundBehindButton(Graphics graphics)
        {
            if (Parent == null)
            {
                return;
            }

            GraphicsState parentState = graphics.Save();

            try
            {
                // Repaint the parent in the button's graphics surface.
                graphics.TranslateTransform(-Left, -Top);

                using (PaintEventArgs parentPaint =
                    new PaintEventArgs(graphics, Bounds))
                {
                    InvokePaintBackground(Parent, parentPaint);
                    InvokePaint(Parent, parentPaint);
                }
            }
            finally
            {
                graphics.Restore(parentState);
            }

            int buttonIndex = Parent.Controls.GetChildIndex(this);

            // Index zero is the front-most child. Controls with a larger index are
            // behind this button, so draw them from back to front. This is important
            // when the button overlaps a PictureBox, logo, panel, or another control.
            for (int index = Parent.Controls.Count - 1;
                 index > buttonIndex;
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
                    using (Bitmap bitmap =
                        new Bitmap(sibling.Width, sibling.Height))
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
                    // The already-painted parent background remains a safe fallback.
                }
                catch (InvalidOperationException)
                {
                    // The sibling may be changing or disposing during repaint.
                }
            }
        }

        private RectangleF GetVisibleButtonRectangle()
        {
            const float inset = 1F;

            return new RectangleF(
                inset,
                inset,
                Math.Max(1F, Width - (inset * 2F)),
                Math.Max(1F, Height - (inset * 2F)));
        }

        private void PaintButtonFill(
            Graphics graphics,
            GraphicsPath path,
            RectangleF rectangle,
            Color start,
            Color end)
        {
            if (_useGradient && start != end)
            {
                using (LinearGradientBrush brush =
                    new LinearGradientBrush(rectangle, start, end, _gradientAngle))
                {
                    graphics.FillPath(brush, path);
                }
            }
            else
            {
                using (SolidBrush brush = new SolidBrush(start))
                {
                    graphics.FillPath(brush, path);
                }
            }
        }

        private void PaintHighlight(
            Graphics graphics,
            GraphicsPath path,
            RectangleF rectangle)
        {
            if (!Enabled || _highlightOpacity <= 0)
            {
                return;
            }

            using (LinearGradientBrush highlightBrush =
                new LinearGradientBrush(
                    rectangle,
                    Color.FromArgb(_highlightOpacity, Color.White),
                    Color.FromArgb(0, Color.White),
                    90F))
            {
                graphics.FillPath(highlightBrush, path);
            }
        }

        private void PaintBorder(Graphics graphics, GraphicsPath path)
        {
            if (_borderThickness <= 0 || _borderColor == Color.Transparent)
            {
                return;
            }

            using (Pen pen = new Pen(_borderColor, _borderThickness))
            {
                pen.Alignment = PenAlignment.Inset;
                pen.LineJoin = LineJoin.Round;
                graphics.DrawPath(pen, path);
            }
        }

        private void PaintFocusBorder(Graphics graphics, RectangleF rectangle)
        {
            if (!_showFocusBorder || !Focused || !ShowFocusCues)
            {
                return;
            }

            RectangleF focusRectangle = RectangleF.Inflate(rectangle, -4F, -4F);

            using (GraphicsPath focusPath = RoundedRectangle.Create(
                focusRectangle,
                Math.Max(0, _topLeftRadius - 4),
                Math.Max(0, _topRightRadius - 4),
                Math.Max(0, _bottomRightRadius - 4),
                Math.Max(0, _bottomLeftRadius - 4)))
            using (Pen focusPen = new Pen(_focusBorderColor, 1F))
            {
                focusPen.DashStyle = DashStyle.Dot;
                graphics.DrawPath(focusPen, focusPath);
            }
        }

        private void DrawContent(Graphics graphics, Rectangle bounds, Color textColor)
        {
            Rectangle content = new Rectangle(
                bounds.Left + Padding.Left,
                bounds.Top + Padding.Top,
                Math.Max(1, bounds.Width - Padding.Horizontal),
                Math.Max(1, bounds.Height - Padding.Vertical));

            bool hasImage = _image != null;
            bool hasText = !string.IsNullOrEmpty(Text);

            Size textSize = hasText
                ? TextRenderer.MeasureText(
                    Text,
                    Font,
                    content.Size,
                    TextFormatFlags.SingleLine | TextFormatFlags.NoPadding)
                : Size.Empty;

            Size imageSize = hasImage
                ? new Size(
                    Math.Min(_imageSize.Width, content.Width),
                    Math.Min(_imageSize.Height, content.Height))
                : Size.Empty;

            Rectangle imageRectangle = Rectangle.Empty;
            Rectangle textRectangle = Rectangle.Empty;

            if (hasImage && hasText)
            {
                LayoutImageAndText(
                    content,
                    textSize,
                    imageSize,
                    out imageRectangle,
                    out textRectangle);
            }
            else if (hasImage)
            {
                imageRectangle = AlignRectangle(content, imageSize, _imageAlign);
            }
            else if (hasText)
            {
                textRectangle = content;
            }

            if (hasImage && imageRectangle.Width > 0 && imageRectangle.Height > 0)
            {
                graphics.DrawImage(_image, imageRectangle);
            }

            if (hasText)
            {
                TextFormatFlags flags =
                    TextFormatFlags.SingleLine |
                    TextFormatFlags.EndEllipsis |
                    TextFormatFlags.VerticalCenter |
                    TextFormatFlags.NoPadding |
                    GetHorizontalTextFlag(_textAlign);

                if (RightToLeft == RightToLeft.Yes)
                {
                    flags |= TextFormatFlags.RightToLeft;
                }

                TextRenderer.DrawText(
                    graphics,
                    Text,
                    Font,
                    textRectangle,
                    textColor,
                    flags);
            }
        }

        private void LayoutImageAndText(
            Rectangle content,
            Size textSize,
            Size imageSize,
            out Rectangle imageRectangle,
            out Rectangle textRectangle)
        {
            imageRectangle = Rectangle.Empty;
            textRectangle = Rectangle.Empty;

            if (_textImageRelation == TextImageRelation.Overlay)
            {
                imageRectangle = AlignRectangle(content, imageSize, _imageAlign);
                textRectangle = content;
                return;
            }

            bool horizontal =
                _textImageRelation == TextImageRelation.ImageBeforeText ||
                _textImageRelation == TextImageRelation.TextBeforeImage;

            if (horizontal)
            {
                int totalWidth = imageSize.Width + _imageSpacing + textSize.Width;
                int groupX = content.Left + Math.Max(0, (content.Width - totalWidth) / 2);
                int imageY = content.Top + Math.Max(0, (content.Height - imageSize.Height) / 2);
                int textY = content.Top + Math.Max(0, (content.Height - textSize.Height) / 2);

                if (_textImageRelation == TextImageRelation.ImageBeforeText)
                {
                    imageRectangle = new Rectangle(
                        groupX,
                        imageY,
                        imageSize.Width,
                        imageSize.Height);

                    textRectangle = new Rectangle(
                        imageRectangle.Right + _imageSpacing,
                        textY,
                        Math.Max(1, content.Right - imageRectangle.Right - _imageSpacing),
                        Math.Max(1, textSize.Height));
                }
                else
                {
                    textRectangle = new Rectangle(
                        groupX,
                        textY,
                        Math.Max(1, textSize.Width),
                        Math.Max(1, textSize.Height));

                    imageRectangle = new Rectangle(
                        textRectangle.Right + _imageSpacing,
                        imageY,
                        imageSize.Width,
                        imageSize.Height);
                }
            }
            else
            {
                int totalHeight = imageSize.Height + _imageSpacing + textSize.Height;
                int groupY = content.Top + Math.Max(0, (content.Height - totalHeight) / 2);
                int imageX = content.Left + Math.Max(0, (content.Width - imageSize.Width) / 2);

                if (_textImageRelation == TextImageRelation.ImageAboveText)
                {
                    imageRectangle = new Rectangle(
                        imageX,
                        groupY,
                        imageSize.Width,
                        imageSize.Height);

                    textRectangle = new Rectangle(
                        content.Left,
                        imageRectangle.Bottom + _imageSpacing,
                        content.Width,
                        Math.Max(1, textSize.Height));
                }
                else
                {
                    textRectangle = new Rectangle(
                        content.Left,
                        groupY,
                        content.Width,
                        Math.Max(1, textSize.Height));

                    imageRectangle = new Rectangle(
                        imageX,
                        textRectangle.Bottom + _imageSpacing,
                        imageSize.Width,
                        imageSize.Height);
                }
            }
        }

        private static Rectangle AlignRectangle(
            Rectangle bounds,
            Size size,
            ContentAlignment alignment)
        {
            int x;
            int y;

            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    x = bounds.Left;
                    break;

                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    x = bounds.Right - size.Width;
                    break;

                default:
                    x = bounds.Left + ((bounds.Width - size.Width) / 2);
                    break;
            }

            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    y = bounds.Top;
                    break;

                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    y = bounds.Bottom - size.Height;
                    break;

                default:
                    y = bounds.Top + ((bounds.Height - size.Height) / 2);
                    break;
            }

            return new Rectangle(x, y, size.Width, size.Height);
        }

        private static TextFormatFlags GetHorizontalTextFlag(
            ContentAlignment alignment)
        {
            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.BottomLeft:
                    return TextFormatFlags.Left;

                case ContentAlignment.TopRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.BottomRight:
                    return TextFormatFlags.Right;

                default:
                    return TextFormatFlags.HorizontalCenter;
            }
        }

        private void OnShapeChanged()
        {
            UpdateControlRegion();
            Invalidate();
        }

        private void UpdateControlRegion()
        {
            Region newRegion = null;

            if (Width > 0 && Height > 0 && HasRoundedCorners())
            {
                // The native Region is deliberately a one-pixel outer envelope of
                // the visible button. Regions are binary and cannot be anti-aliased.
                // This envelope prevents them from cutting off the smooth GDI+ edge.
                const float antiAliasEnvelope = 1F;

                RectangleF regionRectangle = new RectangleF(
                    0F,
                    0F,
                    Width,
                    Height);

                using (GraphicsPath path = RoundedRectangle.Create(
                    regionRectangle,
                    ExpandRegionRadius(_topLeftRadius, antiAliasEnvelope),
                    ExpandRegionRadius(_topRightRadius, antiAliasEnvelope),
                    ExpandRegionRadius(_bottomRightRadius, antiAliasEnvelope),
                    ExpandRegionRadius(_bottomLeftRadius, antiAliasEnvelope)))
                {
                    newRegion = new Region(path);
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

        private static float ExpandRegionRadius(int radius, float amount)
        {
            return radius > 0 ? radius + amount : 0F;
        }

        private bool IsPointInsideShape(Point point)
        {
            if (!ClientRectangle.Contains(point))
            {
                return false;
            }

            if (!HasRoundedCorners())
            {
                return true;
            }

            using (GraphicsPath path = RoundedRectangle.Create(
                GetVisibleButtonRectangle(),
                _topLeftRadius,
                _topRightRadius,
                _bottomRightRadius,
                _bottomLeftRadius))
            {
                return path.IsVisible(point);
            }
        }

        private bool HasRoundedCorners()
        {
            return
                _topLeftRadius > 0 ||
                _topRightRadius > 0 ||
                _bottomRightRadius > 0 ||
                _bottomLeftRadius > 0;
        }

        private void StartHoverAnimation(float target)
        {
            _hoverTarget = target;

            if (Math.Abs(_hoverProgress - _hoverTarget) < 0.001F)
            {
                _animationTimer.Stop();
                return;
            }

            _animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (_hoverProgress < _hoverTarget)
            {
                _hoverProgress = Math.Min(
                    _hoverTarget,
                    _hoverProgress + _animationStep);
            }
            else
            {
                _hoverProgress = Math.Max(
                    _hoverTarget,
                    _hoverProgress - _animationStep);
            }

            Invalidate();

            if (Math.Abs(_hoverProgress - _hoverTarget) < 0.001F)
            {
                _hoverProgress = _hoverTarget;
                _animationTimer.Stop();
            }
        }

        private static Color Blend(Color from, Color to, float amount)
        {
            amount = Math.Max(0F, Math.Min(1F, amount));

            return Color.FromArgb(
                (int)Math.Round(from.A + ((to.A - from.A) * amount)),
                (int)Math.Round(from.R + ((to.R - from.R) * amount)),
                (int)Math.Round(from.G + ((to.G - from.G) * amount)),
                (int)Math.Round(from.B + ((to.B - from.B) * amount)));
        }
    }
}
