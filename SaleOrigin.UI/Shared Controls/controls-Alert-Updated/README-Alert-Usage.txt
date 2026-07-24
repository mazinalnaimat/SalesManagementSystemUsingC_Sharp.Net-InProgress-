RoundedTextBox / PasswordRoundedTextBox inline alert support
============================================================

The alert feature is implemented in RoundedTextBox.
PasswordRoundedTextBox inherits all alert properties, methods, and events.

The default AlertIcon is null. No built-in alert icon is drawn.

Designer properties
-------------------
AlertVisible
AlertText
AlertBorderColor
AlertTextColor
AlertIcon
AlertIconSize
AlertIconTextSpacing
AlertTopSpacing
AlertBottomSpacing
AutoResizeForAlert

Show a message without an icon
------------------------------
roundedTextBox1.AlertBorderColor = Color.FromArgb(239, 68, 68);
roundedTextBox1.AlertTextColor = Color.FromArgb(220, 53, 69);
roundedTextBox1.AlertIcon = null;
roundedTextBox1.ShowAlert("Username is required");

Show a password message with your own icon
------------------------------------------
passwordRoundedTextBox1.ShowAlert(
    "Password is required",
    Color.FromArgb(239, 68, 68),
    Color.FromArgb(220, 53, 69),
    Properties.Resources.ErrorIcon);

Hide or clear
-------------
roundedTextBox1.HideAlert();
roundedTextBox1.ClearAlert();

Events
------
roundedTextBox1.AlertShown += RoundedTextBox1_AlertShown;
roundedTextBox1.AlertHidden += RoundedTextBox1_AlertHidden;
roundedTextBox1.AlertVisibilityChanged += RoundedTextBox1_AlertVisibilityChanged;

Login validation example
------------------------
private void btnLogin_Click(object sender, EventArgs e)
{
    txtUsername.HideAlert();
    txtPassword.HideAlert();

    bool valid = true;

    if (string.IsNullOrWhiteSpace(txtUsername.Text))
    {
        txtUsername.ShowAlert("Username is required");
        valid = false;
    }

    if (string.IsNullOrWhiteSpace(txtPassword.Text))
    {
        txtPassword.ShowAlert(
            "Password is required",
            Color.FromArgb(239, 68, 68),
            Color.FromArgb(220, 53, 69),
            Properties.Resources.ErrorIcon);

        valid = false;
    }

    if (!valid)
        return;

    // Continue login.
}

AutoResizeForAlert is true by default. The control grows downward while the
alert is visible and returns to its original height when the alert is hidden.
Leave enough space below the control, or place controls inside a layout panel
that can respond to the changed height.
