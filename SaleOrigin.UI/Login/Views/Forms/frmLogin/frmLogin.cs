using SaleOrigin.Business.Authentication;
using SaleOrigin.Business.Authentication.Login;
using SaleOrigin.UI.Controls;
using SaleOrigin.UI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SaleOrigin.UI.Login
{
    public partial class frmLogin : Form
    {
        private RoundedAlertControl alertLogin;

        public frmLogin()
        {
            InitializeComponent();
            CreateLoginAlert();
        }

        private void CreateLoginAlert()
        {
            alertLogin = new RoundedAlertControl();
            alertLogin.Name = "alertLogin";
            alertLogin.Text = "Invalid username or password";
            alertLogin.Size = new Size(386, 42);
            alertLogin.Location = new Point(84, 272); // change as needed

            alertLogin.BackColor = Color.FromArgb(255, 245, 245);
            alertLogin.ForeColor = Color.FromArgb(220, 53, 69);
            alertLogin.BorderColor = Color.FromArgb(244, 180, 180);
            alertLogin.BorderThickness = 1;

            alertLogin.UseRoundedCorners = true;
            alertLogin.RadiusTopLeft = 5;
            alertLogin.RadiusTopRight = 5;
            alertLogin.RadiusBottomRight = 5;
            alertLogin.RadiusBottomLeft = 5;

            alertLogin.IconSize = 22;
            alertLogin.IconTextSpacing = 12;
            alertLogin.Visible = false;

            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            alertLogin.AlertIcon = (System.Drawing.Image)(resources.GetObject("exclamation-mark_triangle"));


            this.Controls.Add(alertLogin);
            alertLogin.BringToFront();
        }

        private void ShowInvalidCredentialsAlert(string text)
        {
            alertLogin.Text = text;
            alertLogin.Visible = true;
            pnLoginInfo.Location = new Point(67, 320);
        }

        private void HideLoginAlert()
        {
            alertLogin.Visible = false;
            pnLoginInfo.Location = new Point(67, 255);

        }

        private bool CheckUsernameAndPasswordNotEmpty()
        {
            bool notEmpty = true;

            if (string.IsNullOrEmpty(mtbUserName.Text))
            {
                notEmpty = false;
                mtbUserName.ShowAlert();
            }

            if (string.IsNullOrEmpty(pmtbPassword.Text))
            {
                notEmpty = false;
                pmtbPassword.ShowAlert();
            }

            return notEmpty;
        }
        private async void Login()
        {
            fbtnLogin.Enabled = false;
            mtbUserName.Enabled = false;
            pmtbPassword.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            AuthenticationServices authenticationServices = new AuthenticationServices();

            string username = mtbUserName.Text;
            string password = pmtbPassword.Text;

            LoginRequestModel loginRequestModel = new LoginRequestModel(username, password);
            LoginResultDto loginResultDto = await Task.Run(() =>
            {
                return authenticationServices.Login(LoginRequestMapper.ToDto(loginRequestModel));
            });

            this.Cursor = Cursors.Default;

            LoginResultModel loginResultModel = LoginResultMapper.ToModel(loginResultDto);

            if (loginResultModel.IsSuccess)
            {
                Form frmMain = new frmMain();
                this.Hide();
                frmMain.ShowDialog();
            }
            else
            {
                string alertText = "username or password is wrong!!!";
                ShowInvalidCredentialsAlert(alertText);
                SystemSounds.Asterisk.Play();
                fbtnLogin.Enabled = true;
                mtbUserName.Enabled = true;
                pmtbPassword.Enabled = true;

            }


        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            this.lbBottomRights.Text = "© " + DateTime.Now.Year + " SaleOrigin. All rights reserved.";

        }


        private void fbtnLogin_Click(object sender, EventArgs e)
        {
            if (CheckUsernameAndPasswordNotEmpty())
            {
                Login();
            }
            else
            {
                SystemSounds.Asterisk.Play();

            }

        }

        private void mtbUserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // This line completely blocks and stops the Windows "ding" error sound
                e.SuppressKeyPress = true;

                if (CheckUsernameAndPasswordNotEmpty())
                {
                    Login();
                }

                if (mtbUserName.AlertVisible || pmtbPassword.AlertVisible)
                {
                    SystemSounds.Asterisk.Play();
                }
            }
        }
        private void pmtbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // This line completely blocks and stops the Windows "ding" error sound
                e.SuppressKeyPress = true;

                if (CheckUsernameAndPasswordNotEmpty())
                {
                    Login();
                }
            }

            if (mtbUserName.AlertVisible || pmtbPassword.AlertVisible)
            {
                SystemSounds.Asterisk.Play();
            }
        }



        private void mtbUserName_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(mtbUserName.Text.ToString()))
            {
                mtbUserName.HideAlert();
            }

        }

        private void pmtbPassword_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(pmtbPassword.Text.ToString()))
            {
                pmtbPassword.HideAlert();
            }

        }

        private void mtbUserName_AlertShown(object sender, EventArgs e)
        {
            HideLoginAlert();
            SystemSounds.Asterisk.Play();


        }
        private void pmtbPassword_AlertShown(object sender, EventArgs e)
        {
            HideLoginAlert();
            SystemSounds.Asterisk.Play();


        }


    }
    
}
