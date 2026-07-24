using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using SaleOrigin.UI.Controls.Drawing;

namespace SaleOrigin.UI.Controls
{
    [DefaultEvent("TextChanged")]
    [DefaultProperty("Text")]
    [DesignerCategory("Code")]
    public class RoundedTextBox : UserControl
    {
        private readonly TextBox _textBox;
        private readonly Label _placeholderLabel;
        private readonly PictureBox _leftIconBox;
        private readonly PictureBox _rightIconBox;
        private readonly ToolTip _toolTip;
        private Region _controlRegion;

        private bool _isFocused;
        private bool _isHovered;
        private Color _fillColor = Color.White;
        private Color _fillColor2 = Color.White;
        private bool _useGradient;
        private float _gradientAngle;
        private Color _disabledFillColor = Color.FromArgb(245, 247, 250);
        private Color _borderColor = Color.FromArgb(205, 214, 224);
        private Color _focusBorderColor = Color.FromArgb(0, 123, 255);
        private Color _hoverBorderColor = Color.FromArgb(145, 164, 184);
        private Color _disabledBorderColor = Color.FromArgb(218, 224, 231);
        private Color _textColor = Color.FromArgb(40, 50, 65);
        private Color _placeholderColor = Color.FromArgb(145, 153, 164);
        private int _borderThickness = 1;
        private int _borderRadius = 7;
        private int _horizontalPadding = 12;
        private int _verticalPadding = 8;
        private int _iconSize = 18;
        private int _iconSpacing = 8;
        private string _placeholderText = string.Empty;

        private bool _alertVisible;
        private string _alertText = string.Empty;
        private Color _alertBorderColor = Color.FromArgb(239, 68, 68);
        private Color _alertTextColor = Color.FromArgb(220, 53, 69);
        private Image _alertIcon;
        private int _alertIconSize = 18;
        private int _alertIconTextSpacing = 8;
        private int _alertTopSpacing = 6;
        private int _alertBottomSpacing = 2;
        private bool _autoResizeForAlert = true;
        private int _editorHeight = 42;
        private bool _changingHeightForAlert;

        public RoundedTextBox()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.UserPaint,
                true);

            BackColor = Color.Transparent;
            Size = new Size(250, 42);
            MinimumSize = new Size(80, 32);
            _editorHeight = Height;
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            _textBox = new TextBox
            {
                BorderStyle = BorderStyle.None,
                BackColor = _fillColor,
                ForeColor = _textColor,
                Font = Font,
                Multiline = false,
                TabStop = true
            };

            _placeholderLabel = new Label
            {
                AutoSize = false,
                // Keep the placeholder transparent so it cannot paint over the
                // rounded border. Its bounds are also kept inside the editor area.
                BackColor = Color.Transparent,
                ForeColor = _placeholderColor,
                Font = Font,
                TextAlign = ContentAlignment.MiddleLeft,
                Cursor = Cursors.IBeam
            };

            _leftIconBox = CreateIconBox();
            _rightIconBox = CreateIconBox();
            _rightIconBox.Cursor = Cursors.Hand;

            _toolTip = new ToolTip();

            Controls.Add(_textBox);
            Controls.Add(_placeholderLabel);
            Controls.Add(_leftIconBox);
            Controls.Add(_rightIconBox);

            _textBox.TextChanged += InnerTextBox_TextChanged;
            _textBox.Enter += InnerTextBox_Enter;
            _textBox.Leave += InnerTextBox_Leave;
            _textBox.KeyDown += InnerTextBox_KeyDown;
            _textBox.MouseEnter += Child_MouseEnter;
            _textBox.MouseLeave += Child_MouseLeave;

            _placeholderLabel.Click += delegate { FocusInput(); };
            _placeholderLabel.MouseEnter += Child_MouseEnter;
            _placeholderLabel.MouseLeave += Child_MouseLeave;

            _leftIconBox.MouseEnter += Child_MouseEnter;
            _leftIconBox.MouseLeave += Child_MouseLeave;
            _leftIconBox.Click += LeftIconBox_Click;
            _rightIconBox.MouseEnter += Child_MouseEnter;
            _rightIconBox.MouseLeave += Child_MouseLeave;
            _rightIconBox.Click += RightIconBox_Click;

            EnabledChanged += delegate
            {
                ApplyColors();
                Invalidate();
            };

            ApplyColors();
            PerformLayout();
            UpdatePlaceholder();
            UpdateControlRegion();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public TextBox InnerTextBox
        {
            get { return _textBox; }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "White")]
        public Color FillColor
        {
            get { return _fillColor; }
            set
            {
                _fillColor = value;
                ApplyColors();
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Color), "White")]
        [Description("The second background color used when UseGradient is enabled.")]
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
                ApplyColors();
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        [Description("Enables a gradient background between FillColor and FillColor2.")]
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
                ApplyColors();
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(0F)]
        [Description("The angle, in degrees, of the background gradient.")]
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
                ApplyColors();
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color DisabledFillColor
        {
            get { return _disabledFillColor; }
            set
            {
                _disabledFillColor = value;
                ApplyColors();
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
        public Color FocusBorderColor
        {
            get { return _focusBorderColor; }
            set
            {
                _focusBorderColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color HoverBorderColor
        {
            get { return _hoverBorderColor; }
            set
            {
                _hoverBorderColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color DisabledBorderColor
        {
            get { return _disabledBorderColor; }
            set
            {
                _disabledBorderColor = value;
                Invalidate();
            }
        }

        [Category("Appearance")]
        public Color TextColor
        {
            get { return _textColor; }
            set
            {
                _textColor = value;
                ApplyColors();
            }
        }

        [Category("Appearance")]
        public Color PlaceholderColor
        {
            get { return _placeholderColor; }
            set
            {
                _placeholderColor = value;
                ApplyColors();
            }
        }

        [Category("Appearance")]
        [DefaultValue(1)]
        public int BorderThickness
        {
            get { return _borderThickness; }
            set
            {
                _borderThickness = Math.Max(1, value);
                PerformLayout();
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(7)]
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

        [Category("Layout")]
        [DefaultValue(12)]
        public int HorizontalPadding
        {
            get { return _horizontalPadding; }
            set
            {
                _horizontalPadding = Math.Max(0, value);
                PerformLayout();
            }
        }

        [Category("Layout")]
        [DefaultValue(8)]
        public int VerticalPadding
        {
            get { return _verticalPadding; }
            set
            {
                _verticalPadding = Math.Max(0, value);
                PerformLayout();
            }
        }

        [Category("Layout")]
        [DefaultValue(18)]
        public int IconSize
        {
            get { return _iconSize; }
            set
            {
                _iconSize = Math.Max(8, value);
                PerformLayout();
            }
        }

        [Category("Layout")]
        [DefaultValue(8)]
        public int IconSpacing
        {
            get { return _iconSpacing; }
            set
            {
                _iconSpacing = Math.Max(0, value);
                PerformLayout();
            }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string PlaceholderText
        {
            get { return _placeholderText; }
            set
            {
                _placeholderText = value ?? string.Empty;
                _placeholderLabel.Text = _placeholderText;
                UpdatePlaceholder();
            }
        }

        [Category("Alert")]
        [DefaultValue(false)]
        [Description("Gets or sets whether the inline alert is displayed below the text box.")]
        public bool AlertVisible
        {
            get { return _alertVisible; }
            set { SetAlertVisible(value); }
        }

        [Category("Alert")]
        [DefaultValue("")]
        [Localizable(true)]
        [Description("The message displayed below the text box while the alert is visible.")]
        public string AlertText
        {
            get { return _alertText; }
            set
            {
                string newValue = value ?? string.Empty;
                if (_alertText == newValue)
                {
                    return;
                }

                _alertText = newValue;
                UpdateAlertHeight();
                Invalidate();
            }
        }

        [Category("Alert")]
        [Description("The border color used while the alert is visible.")]
        public Color AlertBorderColor
        {
            get { return _alertBorderColor; }
            set
            {
                if (_alertBorderColor == value)
                {
                    return;
                }

                _alertBorderColor = value;
                Invalidate();
            }
        }

        [Category("Alert")]
        [Description("The color used to draw the alert message.")]
        public Color AlertTextColor
        {
            get { return _alertTextColor; }
            set
            {
                if (_alertTextColor == value)
                {
                    return;
                }

                _alertTextColor = value;
                Invalidate();
            }
        }

        [Category("Alert")]
        [DefaultValue(null)]
        [Description("Optional icon displayed beside the alert text. The default value is null, so no icon is shown unless one is supplied.")]
        public Image AlertIcon
        {
            get { return _alertIcon; }
            set
            {
                if (ReferenceEquals(_alertIcon, value))
                {
                    return;
                }

                _alertIcon = value;
                UpdateAlertHeight();
                Invalidate();
            }
        }

        [Category("Alert")]
        [DefaultValue(18)]
        [Description("Width and height of the optional alert icon.")]
        public int AlertIconSize
        {
            get { return _alertIconSize; }
            set
            {
                int newValue = Math.Max(8, value);
                if (_alertIconSize == newValue)
                {
                    return;
                }

                _alertIconSize = newValue;
                UpdateAlertHeight();
                Invalidate();
            }
        }

        [Category("Alert")]
        [DefaultValue(8)]
        [Description("Space between the optional alert icon and the alert text.")]
        public int AlertIconTextSpacing
        {
            get { return _alertIconTextSpacing; }
            set
            {
                int newValue = Math.Max(0, value);
                if (_alertIconTextSpacing == newValue)
                {
                    return;
                }

                _alertIconTextSpacing = newValue;
                UpdateAlertHeight();
                Invalidate();
            }
        }

        [Category("Alert")]
        [DefaultValue(6)]
        [Description("Vertical space between the text box and the alert message.")]
        public int AlertTopSpacing
        {
            get { return _alertTopSpacing; }
            set
            {
                int newValue = Math.Max(0, value);
                if (_alertTopSpacing == newValue)
                {
                    return;
                }

                _alertTopSpacing = newValue;
                UpdateAlertHeight();
                Invalidate();
            }
        }

        [Category("Alert")]
        [DefaultValue(2)]
        [Description("Space retained below the alert message.")]
        public int AlertBottomSpacing
        {
            get { return _alertBottomSpacing; }
            set
            {
                int newValue = Math.Max(0, value);
                if (_alertBottomSpacing == newValue)
                {
                    return;
                }

                _alertBottomSpacing = newValue;
                UpdateAlertHeight();
                Invalidate();
            }
        }

        [Category("Alert")]
        [DefaultValue(true)]
        [Description("Automatically increases the control height when an alert is shown and restores the original height when it is hidden.")]
        public bool AutoResizeForAlert
        {
            get { return _autoResizeForAlert; }
            set
            {
                if (_autoResizeForAlert == value)
                {
                    return;
                }

                _autoResizeForAlert = value;

                if (_alertVisible)
                {
                    if (_autoResizeForAlert)
                    {
                        UpdateAlertHeight();
                    }
                    else
                    {
                        SetControlHeight(_editorHeight);
                    }
                }

                UpdateControlRegion();
                PerformLayout();
                Invalidate();
            }
        }

        [Category("Appearance")]
        [DefaultValue(null)]
        public Image LeftIcon
        {
            get { return _leftIconBox.Image; }
            set
            {
                _leftIconBox.Image = value;
                _leftIconBox.Visible = value != null;
                PerformLayout();
            }
        }

        [Category("Appearance")]
        [DefaultValue(null)]
        public Image RightIcon
        {
            get { return _rightIconBox.Image; }
            set
            {
                _rightIconBox.Image = value;
                _rightIconBox.Visible = value != null;
                PerformLayout();
            }
        }

        [Category("Behavior")]
        [DefaultValue("")]
        public string LeftIconToolTip
        {
            get { return _toolTip.GetToolTip(_leftIconBox); }
            set { _toolTip.SetToolTip(_leftIconBox, value ?? string.Empty); }
        }

        [Category("Behavior")]
        [DefaultValue("")]
        public string RightIconToolTip
        {
            get { return _toolTip.GetToolTip(_rightIconBox); }
            set { _toolTip.SetToolTip(_rightIconBox, value ?? string.Empty); }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Cursor), "Default")]
        public Cursor LeftIconCursor
        {
            get { return _leftIconBox.Cursor; }
            set { _leftIconBox.Cursor = value ?? Cursors.Default; }
        }

        [Category("Appearance")]
        [DefaultValue(typeof(Cursor), "Hand")]
        public Cursor RightIconCursor
        {
            get { return _rightIconBox.Cursor; }
            set { _rightIconBox.Cursor = value ?? Cursors.Default; }
        }

        [Category("Behavior")]
        [DefaultValue(false)]
        public bool ReadOnly
        {
            get { return _textBox.ReadOnly; }
            set
            {
                _textBox.ReadOnly = value;
            }
        }

        [Category("Behavior")]
        [DefaultValue(false)]
        public bool Multiline
        {
            get { return _textBox.Multiline; }
            set
            {
                _textBox.Multiline = value;
                PerformLayout();
            }
        }

        [Category("Behavior")]
        [DefaultValue(false)]
        public bool UseSystemPasswordChar
        {
            get { return _textBox.UseSystemPasswordChar; }
            set { _textBox.UseSystemPasswordChar = value; }
        }

        [Category("Behavior")]
        [DefaultValue('\0')]
        public char PasswordChar
        {
            get { return _textBox.PasswordChar; }
            set { _textBox.PasswordChar = value; }
        }

        [Category("Behavior")]
        [DefaultValue(32767)]
        public int MaxLength
        {
            get { return _textBox.MaxLength; }
            set { _textBox.MaxLength = value; }
        }

        [Category("Behavior")]
        [DefaultValue(CharacterCasing.Normal)]
        public CharacterCasing CharacterCasing
        {
            get { return _textBox.CharacterCasing; }
            set { _textBox.CharacterCasing = value; }
        }

        [Category("Appearance")]
        [DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment TextAlign
        {
            get { return _textBox.TextAlign; }
            set
            {
                _textBox.TextAlign = value;
                _placeholderLabel.TextAlign = ToContentAlignment(value);
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Bindable(true)]
        public override string Text
        {
            get { return _textBox.Text; }
            set
            {
                string safeValue = value ?? string.Empty;
                if (_textBox.Text == safeValue)
                {
                    return;
                }

                _textBox.Text = safeValue;
                UpdatePlaceholder();
            }
        }

        [Category("Action")]
        public event EventHandler LeftIconClick;

        [Category("Action")]
        public event EventHandler RightIconClick;

        [Category("Alert")]
        [Description("Occurs after the inline alert becomes visible.")]
        public event EventHandler AlertShown;

        [Category("Alert")]
        [Description("Occurs after the inline alert is hidden.")]
        public event EventHandler AlertHidden;

        [Category("Alert")]
        [Description("Occurs whenever AlertVisible changes.")]
        public event EventHandler AlertVisibilityChanged;

        public void ShowAlert()
        {
            SetAlertVisible(true);
        }

        public void ShowAlert(string message)
        {
            AlertText = message;
            SetAlertVisible(true);
        }

        public void ShowAlert(
            string message,
            Color borderColor,
            Color textColor,
            Image icon)
        {
            AlertText = message;
            AlertBorderColor = borderColor;
            AlertTextColor = textColor;
            AlertIcon = icon;
            SetAlertVisible(true);
        }

        public void HideAlert()
        {
            SetAlertVisible(false);
        }

        public void ClearAlert()
        {
            SetAlertVisible(false);
            _alertText = string.Empty;
            _alertIcon = null;
            Invalidate();
        }

        protected virtual void OnAlertShown(EventArgs e)
        {
            EventHandler handler = AlertShown;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnAlertHidden(EventArgs e)
        {
            EventHandler handler = AlertHidden;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnAlertVisibilityChanged(EventArgs e)
        {
            EventHandler handler = AlertVisibilityChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void FocusInput()
        {
            if (Enabled)
            {
                _textBox.Focus();
            }
        }

        public void Clear()
        {
            _textBox.Clear();
        }

        public void SelectAll()
        {
            _textBox.SelectAll();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            if (_textBox == null)
            {
                return;
            }

            _textBox.Font = Font;
            _placeholderLabel.Font = Font;
            UpdateAlertHeight();
            PerformLayout();
            Invalidate();
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);

            if (_textBox != null && ForeColor != Color.Empty)
            {
                TextColor = ForeColor;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _isHovered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            Point cursorPoint = PointToClient(Cursor.Position);
            if (!ClientRectangle.Contains(cursorPoint))
            {
                _isHovered = false;
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

            if (!_changingHeightForAlert)
            {
                if (_alertVisible)
                {
                    _editorHeight = Math.Max(
                        MinimumSize.Height,
                        Height - GetAlertAreaHeight());
                }
                else
                {
                    _editorHeight = Height;
                }
            }

            UpdateControlRegion();
            PerformLayout();
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

                if (_toolTip != null)
                {
                    _toolTip.Dispose();
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

            RectangleF surfaceRectangle = GetVisibleSurfaceRectangle();
            if (surfaceRectangle.Width <= 0F || surfaceRectangle.Height <= 0F)
            {
                return;
            }

            Color currentFill = Enabled ? _fillColor : _disabledFillColor;
            Color currentFill2 = Enabled && _useGradient ? _fillColor2 : currentFill;
            Color currentBorder;

            if (!Enabled)
            {
                currentBorder = _disabledBorderColor;
            }
            else if (_alertVisible)
            {
                currentBorder = _alertBorderColor;
            }
            else if (_isFocused)
            {
                currentBorder = _focusBorderColor;
            }
            else if (_isHovered)
            {
                currentBorder = _hoverBorderColor;
            }
            else
            {
                currentBorder = _borderColor;
            }

            using (GraphicsPath fillPath = RoundedRectangle.Create(surfaceRectangle, _borderRadius))
            {
                if (Enabled && _useGradient && currentFill != currentFill2)
                {
                    using (LinearGradientBrush gradientBrush = new LinearGradientBrush(
                        surfaceRectangle,
                        currentFill,
                        currentFill2,
                        _gradientAngle,
                        true))
                    {
                        graphics.FillPath(gradientBrush, fillPath);
                    }
                }
                else
                {
                    using (SolidBrush fillBrush = new SolidBrush(currentFill))
                    {
                        graphics.FillPath(fillBrush, fillPath);
                    }
                }
            }

            if (_borderThickness > 0 && currentBorder != Color.Transparent)
            {
                float inset = _borderThickness / 2F;
                RectangleF borderRectangle = RectangleF.Inflate(
                    surfaceRectangle,
                    -inset,
                    -inset);

                if (borderRectangle.Width > 0F && borderRectangle.Height > 0F)
                {
                    float adjustedRadius = Math.Max(0F, _borderRadius - inset);

                    using (GraphicsPath borderPath = RoundedRectangle.Create(
                        borderRectangle,
                        adjustedRadius))
                    using (Pen borderPen = new Pen(currentBorder, _borderThickness))
                    {
                        borderPen.Alignment = PenAlignment.Center;
                        graphics.DrawPath(borderPen, borderPath);
                    }
                }
            }

            DrawAlert(graphics);
        }

        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);

            if (_textBox == null)
            {
                return;
            }

            int editorHeight = GetEditorHeight();
            int left = _horizontalPadding + _borderThickness;
            int right = Width - _horizontalPadding - _borderThickness;

            if (_leftIconBox.Visible)
            {
                _leftIconBox.Bounds = new Rectangle(
                    left,
                    Math.Max(0, (editorHeight - _iconSize) / 2),
                    _iconSize,
                    _iconSize);

                left = _leftIconBox.Right + _iconSpacing;
            }

            if (_rightIconBox.Visible)
            {
                _rightIconBox.Bounds = new Rectangle(
                    Math.Max(left, right - _iconSize),
                    Math.Max(0, (editorHeight - _iconSize) / 2),
                    _iconSize,
                    _iconSize);

                right = _rightIconBox.Left - _iconSpacing;
            }

            int availableWidth = Math.Max(1, right - left);

            if (_textBox.Multiline)
            {
                int top = _verticalPadding + _borderThickness;
                int height = Math.Max(1, editorHeight - (top * 2));
                _textBox.Bounds = new Rectangle(left, top, availableWidth, height);
                _placeholderLabel.Bounds = _textBox.Bounds;
                _placeholderLabel.TextAlign = ToContentAlignment(_textBox.TextAlign);
            }
            else
            {
                int textHeight = Math.Max(Font.Height + 4, _textBox.PreferredHeight);
                int top = Math.Max(_borderThickness, (editorHeight - textHeight) / 2);
                _textBox.Bounds = new Rectangle(left, top, availableWidth, textHeight);
                // Never stretch the placeholder across the complete control height.
                // Doing that lets its rectangular background cover the middle of the
                // top and bottom rounded border. Keep it exactly inside the text area.
                _placeholderLabel.Bounds = _textBox.Bounds;
                _placeholderLabel.TextAlign = ToContentAlignment(_textBox.TextAlign);
            }

            _placeholderLabel.BringToFront();
            _leftIconBox.BringToFront();
            _rightIconBox.BringToFront();
        }

        private static PictureBox CreateIconBox()
        {
            return new PictureBox
            {
                BackColor = Color.Transparent,
                SizeMode = PictureBoxSizeMode.Zoom,
                Visible = false,
                TabStop = false
            };
        }

        private void ApplyColors()
        {
            Color fill = GetEditorFillColor();
            Color text = Enabled ? _textColor : SystemColors.GrayText;

            _textBox.BackColor = fill;
            _textBox.ForeColor = text;
            // A transparent placeholder lets the already-painted rounded/gradient
            // surface remain visible and prevents the label from erasing the border.
            _placeholderLabel.BackColor = Color.Transparent;
            _placeholderLabel.ForeColor = Enabled ? _placeholderColor : SystemColors.GrayText;
        }

        private void UpdatePlaceholder()
        {
            _placeholderLabel.Text = _placeholderText;
            _placeholderLabel.Visible =
                !_isFocused &&
                string.IsNullOrEmpty(_textBox.Text) &&
                !string.IsNullOrEmpty(_placeholderText);

            if (_placeholderLabel.Visible)
            {
                _placeholderLabel.BringToFront();
            }
        }

        private void InnerTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdatePlaceholder();
            OnTextChanged(e);
        }

        private void InnerTextBox_Enter(object sender, EventArgs e)
        {
            _isFocused = true;
            UpdatePlaceholder();
            Invalidate();
        }

        private void InnerTextBox_Leave(object sender, EventArgs e)
        {
            _isFocused = false;
            UpdatePlaceholder();
            Invalidate();
        }

        private void InnerTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            OnKeyDown(e);
        }

        private void Child_MouseEnter(object sender, EventArgs e)
        {
            _isHovered = true;
            Invalidate();
        }

        private void Child_MouseLeave(object sender, EventArgs e)
        {
            Point cursorPoint = PointToClient(Cursor.Position);
            if (!ClientRectangle.Contains(cursorPoint))
            {
                _isHovered = false;
                Invalidate();
            }
        }

        private void LeftIconBox_Click(object sender, EventArgs e)
        {
            EventHandler handler = LeftIconClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void RightIconBox_Click(object sender, EventArgs e)
        {
            EventHandler handler = RightIconClick;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void SetAlertVisible(bool visible)
        {
            if (_alertVisible == visible)
            {
                return;
            }

            if (visible)
            {
                _editorHeight = Height;
                _alertVisible = true;

                if (_autoResizeForAlert)
                {
                    SetControlHeight(_editorHeight + GetAlertAreaHeight());
                }
            }
            else
            {
                _alertVisible = false;

                if (_autoResizeForAlert)
                {
                    SetControlHeight(_editorHeight);
                }
                else
                {
                    _editorHeight = Height;
                }
            }

            UpdateControlRegion();
            PerformLayout();
            Invalidate();

            if (Parent != null)
            {
                Parent.PerformLayout();
                Parent.Invalidate(Bounds, true);
            }

            OnAlertVisibilityChanged(EventArgs.Empty);

            if (visible)
            {
                OnAlertShown(EventArgs.Empty);
            }
            else
            {
                OnAlertHidden(EventArgs.Empty);
            }
        }

        private void UpdateAlertHeight()
        {
            if (!_alertVisible)
            {
                return;
            }

            if (_autoResizeForAlert)
            {
                SetControlHeight(_editorHeight + GetAlertAreaHeight());
            }

            UpdateControlRegion();
            PerformLayout();
            Invalidate();

            if (Parent != null)
            {
                Parent.PerformLayout();
                Parent.Invalidate(Bounds, true);
            }
        }

        private void SetControlHeight(int height)
        {
            int newHeight = Math.Max(MinimumSize.Height, height);
            if (Height == newHeight)
            {
                return;
            }

            _changingHeightForAlert = true;

            try
            {
                Height = newHeight;
            }
            finally
            {
                _changingHeightForAlert = false;
            }
        }

        private int GetEditorHeight()
        {
            if (!_alertVisible)
            {
                return Math.Max(1, Height);
            }

            return Math.Max(1, Math.Min(_editorHeight, Height));
        }

        private int GetAlertAreaHeight()
        {
            int contentHeight = GetAlertContentHeight();
            if (contentHeight <= 0)
            {
                return 0;
            }

            return _alertTopSpacing + contentHeight + _alertBottomSpacing;
        }

        private int GetAlertContentHeight()
        {
            int textHeight = 0;

            if (!string.IsNullOrEmpty(_alertText))
            {
                Size measured = TextRenderer.MeasureText(
                    _alertText,
                    Font,
                    new Size(int.MaxValue, int.MaxValue),
                    TextFormatFlags.SingleLine |
                    TextFormatFlags.NoPadding);

                textHeight = measured.Height;
            }

            int iconHeight = _alertIcon != null ? _alertIconSize : 0;
            return Math.Max(textHeight, iconHeight);
        }

        private void DrawAlert(Graphics graphics)
        {
            if (!_alertVisible)
            {
                return;
            }

            int contentHeight = GetAlertContentHeight();
            if (contentHeight <= 0)
            {
                return;
            }

            int editorHeight = GetEditorHeight();
            int y = editorHeight + _alertTopSpacing;
            int edgeInset = Math.Max(1, _borderThickness);
            int left = edgeInset;
            int right = Math.Max(left, Width - edgeInset);
            bool rightToLeft = RightToLeft == System.Windows.Forms.RightToLeft.Yes;

            Rectangle iconRectangle = Rectangle.Empty;

            if (_alertIcon != null)
            {
                int iconY = y + Math.Max(0, (contentHeight - _alertIconSize) / 2);

                if (rightToLeft)
                {
                    iconRectangle = new Rectangle(
                        Math.Max(left, right - _alertIconSize),
                        iconY,
                        _alertIconSize,
                        _alertIconSize);

                    right = Math.Max(left, iconRectangle.Left - _alertIconTextSpacing);
                }
                else
                {
                    iconRectangle = new Rectangle(
                        left,
                        iconY,
                        _alertIconSize,
                        _alertIconSize);

                    left = Math.Min(right, iconRectangle.Right + _alertIconTextSpacing);
                }

                graphics.DrawImage(_alertIcon, iconRectangle);
            }

            if (string.IsNullOrEmpty(_alertText) || right <= left)
            {
                return;
            }

            Rectangle textRectangle = new Rectangle(
                left,
                y,
                Math.Max(1, right - left),
                contentHeight);

            TextFormatFlags flags =
                TextFormatFlags.SingleLine |
                TextFormatFlags.VerticalCenter |
                TextFormatFlags.EndEllipsis |
                TextFormatFlags.NoPadding;

            if (rightToLeft)
            {
                flags |= TextFormatFlags.Right |
                         TextFormatFlags.RightToLeft;
            }
            else
            {
                flags |= TextFormatFlags.Left;
            }

            TextRenderer.DrawText(
                graphics,
                _alertText,
                Font,
                textRectangle,
                Enabled ? _alertTextColor : SystemColors.GrayText,
                flags);
        }

        private RectangleF GetVisibleSurfaceRectangle()
        {
            const float inset = 1F;
            int editorHeight = GetEditorHeight();

            return new RectangleF(
                inset,
                inset,
                Math.Max(1F, Width - (inset * 2F)),
                Math.Max(1F, editorHeight - (inset * 2F)));
        }

        private void UpdateControlRegion()
        {
            Region newRegion = null;

            if (Width > 0 &&
                Height > 0 &&
                HasRoundedCorners() &&
                !_alertVisible)
            {
                const float antiAliasEnvelope = 1F;

                RectangleF regionRectangle = new RectangleF(
                    0F,
                    0F,
                    Width,
                    Height);

                using (GraphicsPath path = RoundedRectangle.Create(
                    regionRectangle,
                    _borderRadius + antiAliasEnvelope))
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

        private Color GetEditorFillColor()
        {
            if (!Enabled)
            {
                return _disabledFillColor;
            }

            if (!_useGradient || _fillColor == _fillColor2)
            {
                return _fillColor;
            }

            // A native WinForms TextBox cannot be transparent. Use the midpoint
            // of the gradient beneath the editor so the text area blends naturally
            // with the surrounding painted surface instead of creating a hard patch.
            return Blend(_fillColor, _fillColor2, 0.5F);
        }

        private static Color Blend(Color first, Color second, float amount)
        {
            float value = Math.Max(0F, Math.Min(1F, amount));

            return Color.FromArgb(
                (int)Math.Round(first.A + ((second.A - first.A) * value)),
                (int)Math.Round(first.R + ((second.R - first.R) * value)),
                (int)Math.Round(first.G + ((second.G - first.G) * value)),
                (int)Math.Round(first.B + ((second.B - first.B) * value)));
        }

        private static ContentAlignment ToContentAlignment(HorizontalAlignment alignment)
        {
            switch (alignment)
            {
                case HorizontalAlignment.Center:
                    return ContentAlignment.MiddleCenter;
                case HorizontalAlignment.Right:
                    return ContentAlignment.MiddleRight;
                default:
                    return ContentAlignment.MiddleLeft;
            }
        }
    }
}
