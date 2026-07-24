using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using SaleOrigin.UI.Controls.Drawing;

namespace SaleOrigin.UI.Controls
{
    public enum PasswordContentDirection
    {
        LeftToRight,
        RightToLeft
    }

    [DefaultEvent("TextChanged")]
    [DefaultProperty("Text")]
    [DesignerCategory("Code")]
    [ToolboxItem(true)]
    public class PasswordModernTextBox : ModernTextBox
    {
        private bool _isInitialized;
        private bool _isSynchronizingDirection;
        private bool _passwordVisible;
        private bool _showPasswordIcon = true;
        private bool _showVisibilityToggle = true;
        private char _maskCharacter;
        private PasswordContentDirection _contentDirection = PasswordContentDirection.LeftToRight;
        private Color _iconColor = Color.FromArgb(108, 120, 136);
        private string _showPasswordToolTip = "Show password";
        private string _hidePasswordToolTip = "Hide password";

        private Image _customPasswordIcon;
        private Image _customHiddenPasswordIcon;
        private Image _customVisiblePasswordIcon;
        private Bitmap _defaultPasswordIcon;
        private Bitmap _defaultHiddenPasswordIcon;
        private Bitmap _defaultVisiblePasswordIcon;

        public PasswordModernTextBox()
        {
            Multiline = false;
            base.PasswordChar = '\0';
            base.UseSystemPasswordChar = true;

            LeftIconClick += PasswordRoundedTextBox_LeftIconClick;
            RightIconClick += PasswordRoundedTextBox_RightIconClick;

            RecreateDefaultIcons();
            _isInitialized = true;
            ApplyDirectionAndIcons();
            ApplyPasswordMask();
        }

        [Category("Password")]
        [DefaultValue(false)]
        [Description("Controls whether the password text is currently visible.")]
        public bool PasswordVisible
        {
            get { return _passwordVisible; }
            set
            {
                if (_passwordVisible == value)
                {
                    return;
                }

                _passwordVisible = value;
                ApplyPasswordMask();
                ApplyDirectionAndIcons();
                OnPasswordVisibilityChanged(EventArgs.Empty);
            }
        }

        [Category("Password")]
        [DefaultValue(true)]
        [Description("Shows the lock/password icon beside the text.")]
        public bool ShowPasswordIcon
        {
            get { return _showPasswordIcon; }
            set
            {
                if (_showPasswordIcon == value)
                {
                    return;
                }

                _showPasswordIcon = value;
                ApplyDirectionAndIcons();
            }
        }

        [Category("Password")]
        [DefaultValue(true)]
        [Description("Shows the clickable eye icon used to reveal or hide the password.")]
        public bool ShowVisibilityToggle
        {
            get { return _showVisibilityToggle; }
            set
            {
                if (_showVisibilityToggle == value)
                {
                    return;
                }

                _showVisibilityToggle = value;
                ApplyDirectionAndIcons();
            }
        }

        [Category("Password")]
        [DefaultValue('\0')]
        [Description("Optional custom masking character. Leave it empty to use the Windows system password character.")]
        public char MaskCharacter
        {
            get { return _maskCharacter; }
            set
            {
                if (_maskCharacter == value)
                {
                    return;
                }

                _maskCharacter = value;
                ApplyPasswordMask();
            }
        }

        [Category("Layout")]
        [DefaultValue(PasswordContentDirection.LeftToRight)]
        [Description("LTR: password icon on the left and visibility icon on the right. RTL: visibility icon on the left and password icon on the right.")]
        public PasswordContentDirection ContentDirection
        {
            get { return _contentDirection; }
            set
            {
                if (_contentDirection == value)
                {
                    return;
                }

                _contentDirection = value;
                SynchronizeRightToLeftProperty();
                ApplyDirectionAndIcons();
            }
        }

        [Category("Password Appearance")]
        [Description("Color used by the built-in lock and eye icons.")]
        public Color PasswordIconColor
        {
            get { return _iconColor; }
            set
            {
                if (_iconColor == value)
                {
                    return;
                }

                _iconColor = value;
                RecreateDefaultIcons();
                ApplyDirectionAndIcons();
            }
        }

        [Category("Password Appearance")]
        [DefaultValue(null)]
        [Description("Optional custom password/lock icon. Null uses the built-in icon.")]
        public Image PasswordIcon
        {
            get { return _customPasswordIcon; }
            set
            {
                _customPasswordIcon = value;
                ApplyDirectionAndIcons();
            }
        }

        [Category("Password Appearance")]
        [DefaultValue(null)]
        [Description("Optional custom icon displayed while the password is hidden. Null uses the built-in crossed-eye icon.")]
        public Image HiddenPasswordIcon
        {
            get { return _customHiddenPasswordIcon; }
            set
            {
                _customHiddenPasswordIcon = value;
                ApplyDirectionAndIcons();
            }
        }

        [Category("Password Appearance")]
        [DefaultValue(null)]
        [Description("Optional custom icon displayed while the password is visible. Null uses the built-in open-eye icon.")]
        public Image VisiblePasswordIcon
        {
            get { return _customVisiblePasswordIcon; }
            set
            {
                _customVisiblePasswordIcon = value;
                ApplyDirectionAndIcons();
            }
        }

        [Category("Password")]
        [DefaultValue("Show password")]
        public string ShowPasswordToolTip
        {
            get { return _showPasswordToolTip; }
            set
            {
                _showPasswordToolTip = value ?? string.Empty;
                ApplyDirectionAndIcons();
            }
        }

        [Category("Password")]
        [DefaultValue("Hide password")]
        public string HidePasswordToolTip
        {
            get { return _hidePasswordToolTip; }
            set
            {
                _hidePasswordToolTip = value ?? string.Empty;
                ApplyDirectionAndIcons();
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool UseSystemPasswordChar
        {
            get { return !_passwordVisible && _maskCharacter == '\0'; }
            set
            {
                _maskCharacter = '\0';

                bool newVisibleState = !value;
                if (_passwordVisible == newVisibleState)
                {
                    ApplyPasswordMask();
                    ApplyDirectionAndIcons();
                    return;
                }

                PasswordVisible = newVisibleState;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new char PasswordChar
        {
            get { return base.PasswordChar; }
            set
            {
                MaskCharacter = value;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool Multiline
        {
            get { return false; }
            set { base.Multiline = false; }
        }

        [Category("Action")]
        public event EventHandler PasswordVisibilityChanged;

        public void TogglePasswordVisibility()
        {
            if (!Enabled || !ShowVisibilityToggle)
            {
                return;
            }

            PasswordVisible = !PasswordVisible;
            FocusInput();
        }

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            base.OnRightToLeftChanged(e);

            if (!_isInitialized || _isSynchronizingDirection)
            {
                return;
            }

            PasswordContentDirection newDirection =
                RightToLeft == RightToLeft.Yes
                    ? PasswordContentDirection.RightToLeft
                    : PasswordContentDirection.LeftToRight;

            if (_contentDirection != newDirection)
            {
                _contentDirection = newDirection;
                ApplyDirectionAndIcons();
            }
        }

        protected virtual void OnPasswordVisibilityChanged(EventArgs e)
        {
            EventHandler handler = PasswordVisibilityChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeDefaultIcons();
            }

            base.Dispose(disposing);
        }

        private void PasswordRoundedTextBox_LeftIconClick(object sender, EventArgs e)
        {
            if (_contentDirection == PasswordContentDirection.RightToLeft)
            {
                TogglePasswordVisibility();
            }
        }

        private void PasswordRoundedTextBox_RightIconClick(object sender, EventArgs e)
        {
            if (_contentDirection == PasswordContentDirection.LeftToRight)
            {
                TogglePasswordVisibility();
            }
        }

        private void SynchronizeRightToLeftProperty()
        {
            _isSynchronizingDirection = true;

            try
            {
                RightToLeft = _contentDirection == PasswordContentDirection.RightToLeft
                    ? RightToLeft.Yes
                    : RightToLeft.No;
            }
            finally
            {
                _isSynchronizingDirection = false;
            }
        }

        private void ApplyDirectionAndIcons()
        {
            if (!_isInitialized)
            {
                return;
            }

            bool isRightToLeft = _contentDirection == PasswordContentDirection.RightToLeft;

            InnerTextBox.RightToLeft = isRightToLeft ? RightToLeft.Yes : RightToLeft.No;
            TextAlign = isRightToLeft ? HorizontalAlignment.Right : HorizontalAlignment.Left;

            Image passwordIcon = _showPasswordIcon
                ? (_customPasswordIcon ?? _defaultPasswordIcon)
                : null;

            Image visibilityIcon = _showVisibilityToggle
                ? GetCurrentVisibilityIcon()
                : null;

            string visibilityToolTip = _passwordVisible
                ? _hidePasswordToolTip
                : _showPasswordToolTip;

            if (isRightToLeft)
            {
                LeftIcon = visibilityIcon;
                RightIcon = passwordIcon;
                LeftIconCursor = visibilityIcon == null ? Cursors.Default : Cursors.Hand;
                RightIconCursor = Cursors.Default;
                LeftIconToolTip = visibilityIcon == null ? string.Empty : visibilityToolTip;
                RightIconToolTip = string.Empty;
            }
            else
            {
                LeftIcon = passwordIcon;
                RightIcon = visibilityIcon;
                LeftIconCursor = Cursors.Default;
                RightIconCursor = visibilityIcon == null ? Cursors.Default : Cursors.Hand;
                LeftIconToolTip = string.Empty;
                RightIconToolTip = visibilityIcon == null ? string.Empty : visibilityToolTip;
            }

            PerformLayout();
            Invalidate();
        }

        private Image GetCurrentVisibilityIcon()
        {
            if (_passwordVisible)
            {
                return _customVisiblePasswordIcon ?? _defaultVisiblePasswordIcon;
            }

            return _customHiddenPasswordIcon ?? _defaultHiddenPasswordIcon;
        }

        private void ApplyPasswordMask()
        {
            if (!_isInitialized)
            {
                return;
            }

            int selectionStart = InnerTextBox.SelectionStart;
            int selectionLength = InnerTextBox.SelectionLength;

            if (_passwordVisible)
            {
                base.UseSystemPasswordChar = false;
                base.PasswordChar = '\0';
            }
            else if (_maskCharacter == '\0')
            {
                base.PasswordChar = '\0';
                base.UseSystemPasswordChar = true;
            }
            else
            {
                base.UseSystemPasswordChar = false;
                base.PasswordChar = _maskCharacter;
            }

            int safeStart = Math.Min(selectionStart, InnerTextBox.TextLength);
            int safeLength = Math.Min(selectionLength, InnerTextBox.TextLength - safeStart);
            InnerTextBox.Select(safeStart, safeLength);
        }

        private void RecreateDefaultIcons()
        {
            DisposeDefaultIcons();
            _defaultPasswordIcon = PasswordIcons.CreateLock(_iconColor);
            _defaultHiddenPasswordIcon = PasswordIcons.CreateEye(_iconColor, false);
            _defaultVisiblePasswordIcon = PasswordIcons.CreateEye(_iconColor, true);
        }

        private void DisposeDefaultIcons()
        {
            if (_defaultPasswordIcon != null)
            {
                _defaultPasswordIcon.Dispose();
                _defaultPasswordIcon = null;
            }

            if (_defaultHiddenPasswordIcon != null)
            {
                _defaultHiddenPasswordIcon.Dispose();
                _defaultHiddenPasswordIcon = null;
            }

            if (_defaultVisiblePasswordIcon != null)
            {
                _defaultVisiblePasswordIcon.Dispose();
                _defaultVisiblePasswordIcon = null;
            }
        }
    }
}
