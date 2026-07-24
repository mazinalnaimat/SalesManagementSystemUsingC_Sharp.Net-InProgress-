using System.Drawing;
using System;

using SaleOrigin.UI.Controls;

namespace SaleOrigin.UI.Login
{
    partial class frmLogin
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbBottomRights = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.modernPanel1 = new SaleOrigin.UI.Controls.ModernPanel();
            this.pnLoginInfo = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.pmtbPassword = new SaleOrigin.UI.Controls.PasswordModernTextBox();
            this.mtbUserName = new SaleOrigin.UI.Controls.ModernTextBox();
            this.fbtnLogin = new SaleOrigin.UI.Controls.FlexibleButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.pnLoginInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Yu Gothic UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(129, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(291, 31);
            this.label1.TabIndex = 10;
            this.label1.Text = "Sales Management System";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbBottomRights);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 773);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(560, 55);
            this.panel1.TabIndex = 23;
            // 
            // lbBottomRights
            // 
            this.lbBottomRights.AutoSize = true;
            this.lbBottomRights.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbBottomRights.ForeColor = System.Drawing.Color.Black;
            this.lbBottomRights.Location = new System.Drawing.Point(134, 14);
            this.lbBottomRights.Name = "lbBottomRights";
            this.lbBottomRights.Size = new System.Drawing.Size(260, 20);
            this.lbBottomRights.TabIndex = 31;
            this.lbBottomRights.Text = "© 2026 SaleOrigin. All rights reserved.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(105)))), ((int)(((byte)(242)))));
            this.label2.Location = new System.Drawing.Point(79, 226);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 28);
            this.label2.TabIndex = 28;
            this.label2.Text = "Sign In";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Yu Gothic UI", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(143, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(244, 17);
            this.label3.TabIndex = 29;
            this.label3.Text = "Manage your slaes. Grow your business.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 590);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(560, 183);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(107, 27);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(323, 88);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            // 
            // modernPanel1
            // 
            this.modernPanel1.BackColor = System.Drawing.Color.Transparent;
            this.modernPanel1.BorderThickness = 0;
            this.modernPanel1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.modernPanel1.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.modernPanel1.Location = new System.Drawing.Point(48, 191);
            this.modernPanel1.Name = "modernPanel1";
            this.modernPanel1.Padding = new System.Windows.Forms.Padding(12);
            this.modernPanel1.ShadowDepth = 2;
            this.modernPanel1.ShadowEnabled = true;
            this.modernPanel1.Size = new System.Drawing.Size(458, 393);
            this.modernPanel1.TabIndex = 18;
            // 
            // pnLoginInfo
            // 
            this.pnLoginInfo.Controls.Add(this.label5);
            this.pnLoginInfo.Controls.Add(this.label4);
            this.pnLoginInfo.Controls.Add(this.pmtbPassword);
            this.pnLoginInfo.Controls.Add(this.mtbUserName);
            this.pnLoginInfo.Controls.Add(this.fbtnLogin);
            this.pnLoginInfo.Location = new System.Drawing.Point(67, 255);
            this.pnLoginInfo.Name = "pnLoginInfo";
            this.pnLoginInfo.Size = new System.Drawing.Size(420, 254);
            this.pnLoginInfo.TabIndex = 32;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Yu Gothic UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(21, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 23);
            this.label5.TabIndex = 36;
            this.label5.Text = "Password";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Yu Gothic UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(22, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 23);
            this.label4.TabIndex = 35;
            this.label4.Text = "Username";
            // 
            // pmtbPassword
            // 
            this.pmtbPassword.AlertBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.pmtbPassword.AlertIcon = ((System.Drawing.Image)(resources.GetObject("pmtbPassword.AlertIcon")));
            this.pmtbPassword.AlertText = "Password is required";
            this.pmtbPassword.AlertTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.pmtbPassword.BackColor = System.Drawing.Color.Transparent;
            this.pmtbPassword.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(214)))), ((int)(((byte)(224)))));
            this.pmtbPassword.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(224)))), ((int)(((byte)(231)))));
            this.pmtbPassword.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.pmtbPassword.FocusBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.pmtbPassword.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pmtbPassword.HiddenPasswordIcon = ((System.Drawing.Image)(resources.GetObject("pmtbPassword.HiddenPasswordIcon")));
            this.pmtbPassword.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(145)))), ((int)(((byte)(164)))), ((int)(((byte)(184)))));
            this.pmtbPassword.LeftIcon = ((System.Drawing.Image)(resources.GetObject("pmtbPassword.LeftIcon")));
            this.pmtbPassword.Location = new System.Drawing.Point(19, 133);
            this.pmtbPassword.MinimumSize = new System.Drawing.Size(80, 32);
            this.pmtbPassword.Name = "pmtbPassword";
            this.pmtbPassword.PasswordIconColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(120)))), ((int)(((byte)(136)))));
            this.pmtbPassword.PlaceholderColor = System.Drawing.Color.FromArgb(((int)(((byte)(145)))), ((int)(((byte)(153)))), ((int)(((byte)(164)))));
            this.pmtbPassword.PlaceholderText = "Password";
            this.pmtbPassword.RightIcon = ((System.Drawing.Image)(resources.GetObject("pmtbPassword.RightIcon")));
            this.pmtbPassword.RightIconToolTip = "Show password";
            this.pmtbPassword.Size = new System.Drawing.Size(382, 42);
            this.pmtbPassword.TabIndex = 33;
            this.pmtbPassword.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(65)))));
            this.pmtbPassword.VisiblePasswordIcon = ((System.Drawing.Image)(resources.GetObject("pmtbPassword.VisiblePasswordIcon")));
            this.pmtbPassword.AlertShown += new System.EventHandler(this.pmtbPassword_AlertShown);
            this.pmtbPassword.TextChanged += new System.EventHandler(this.pmtbPassword_TextChanged);
            this.pmtbPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.pmtbPassword_KeyDown);
            // 
            // mtbUserName
            // 
            this.mtbUserName.AlertBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(68)))), ((int)(((byte)(68)))));
            this.mtbUserName.AlertIcon = ((System.Drawing.Image)(resources.GetObject("mtbUserName.AlertIcon")));
            this.mtbUserName.AlertText = "Username is required";
            this.mtbUserName.AlertTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(53)))), ((int)(((byte)(69)))));
            this.mtbUserName.BackColor = System.Drawing.Color.Black;
            this.mtbUserName.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(214)))), ((int)(((byte)(224)))));
            this.mtbUserName.DisabledBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(224)))), ((int)(((byte)(231)))));
            this.mtbUserName.DisabledFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.mtbUserName.FocusBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.mtbUserName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.mtbUserName.HoverBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(145)))), ((int)(((byte)(164)))), ((int)(((byte)(184)))));
            this.mtbUserName.LeftIcon = ((System.Drawing.Image)(resources.GetObject("mtbUserName.LeftIcon")));
            this.mtbUserName.Location = new System.Drawing.Point(19, 35);
            this.mtbUserName.MinimumSize = new System.Drawing.Size(80, 32);
            this.mtbUserName.Name = "mtbUserName";
            this.mtbUserName.PlaceholderColor = System.Drawing.Color.FromArgb(((int)(((byte)(145)))), ((int)(((byte)(153)))), ((int)(((byte)(164)))));
            this.mtbUserName.PlaceholderText = "Username";
            this.mtbUserName.Size = new System.Drawing.Size(382, 42);
            this.mtbUserName.TabIndex = 32;
            this.mtbUserName.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(50)))), ((int)(((byte)(65)))));
            this.mtbUserName.AlertShown += new System.EventHandler(this.mtbUserName_AlertShown);
            this.mtbUserName.TextChanged += new System.EventHandler(this.mtbUserName_TextChanged);
            this.mtbUserName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mtbUserName_KeyDown);
            // 
            // fbtnLogin
            // 
            this.fbtnLogin.BackColor = System.Drawing.Color.Transparent;
            this.fbtnLogin.BorderColor = System.Drawing.Color.Transparent;
            this.fbtnLogin.BorderRadius = 20;
            this.fbtnLogin.BottomLeftRadius = 20;
            this.fbtnLogin.BottomRightRadius = 20;
            this.fbtnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.fbtnLogin.DisabledEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(198)))), ((int)(((byte)(207)))));
            this.fbtnLogin.DisabledStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(217)))), ((int)(((byte)(224)))));
            this.fbtnLogin.DisabledTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(128)))), ((int)(((byte)(138)))));
            this.fbtnLogin.EndColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(188)))), ((int)(((byte)(212)))));
            this.fbtnLogin.FocusBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.fbtnLogin.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.fbtnLogin.ForeColor = System.Drawing.Color.Transparent;
            this.fbtnLogin.HoverEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(199)))));
            this.fbtnLogin.HoverStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(91)))), ((int)(((byte)(235)))));
            this.fbtnLogin.HoverTextColor = System.Drawing.Color.White;
            this.fbtnLogin.Image = ((System.Drawing.Image)(resources.GetObject("fbtnLogin.Image")));
            this.fbtnLogin.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.fbtnLogin.ImageSize = new System.Drawing.Size(20, 20);
            this.fbtnLogin.Location = new System.Drawing.Point(25, 202);
            this.fbtnLogin.MinimumSize = new System.Drawing.Size(30, 24);
            this.fbtnLogin.Name = "fbtnLogin";
            this.fbtnLogin.PressedEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(148)))), ((int)(((byte)(174)))));
            this.fbtnLogin.PressedStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(78)))), ((int)(((byte)(205)))));
            this.fbtnLogin.PressedTextColor = System.Drawing.Color.White;
            this.fbtnLogin.Size = new System.Drawing.Size(377, 44);
            this.fbtnLogin.StartColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(255)))));
            this.fbtnLogin.TabIndex = 34;
            this.fbtnLogin.Text = "Login";
            this.fbtnLogin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.fbtnLogin.TopLeftRadius = 20;
            this.fbtnLogin.TopRightRadius = 20;
            this.fbtnLogin.Click += new System.EventHandler(this.fbtnLogin_Click);
            // 
            // frmLogin
            // 
            this.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(560, 828);
            this.Controls.Add(this.pnLoginInfo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.modernPanel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(578, 875);
            this.MinimumSize = new System.Drawing.Size(578, 875);
            this.Name = "frmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SaleOrigin - Sales Mangement Sytem - Login";
            this.Load += new System.EventHandler(this.frmLogin_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.pnLoginInfo.ResumeLayout(false);
            this.pnLoginInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private Controls.ModernPanel modernPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbBottomRights;
        private System.Windows.Forms.Panel pnLoginInfo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private PasswordModernTextBox pmtbPassword;
        private ModernTextBox mtbUserName;
        private FlexibleButton fbtnLogin;
    }
}