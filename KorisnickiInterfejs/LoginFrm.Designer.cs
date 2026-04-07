namespace KorisnickiInterfejs
{
    partial class LoginFrm
    {
        private void InitializeComponent()
        {
            lblEmail = new Label();
            lblPassword = new Label();
            txtEmail = new TextBox();
            txtPassword = new TextBox();
            btnLogin = new Button();
            SuspendLayout();

            lblEmail.Location = new System.Drawing.Point(30, 30);
            lblEmail.Size = new System.Drawing.Size(80, 23);
            lblEmail.Text = "Email:";

            lblPassword.Location = new System.Drawing.Point(30, 70);
            lblPassword.Size = new System.Drawing.Size(80, 23);
            lblPassword.Text = "Password:";

            txtEmail.Location = new System.Drawing.Point(120, 27);
            txtEmail.Size = new System.Drawing.Size(200, 23);
            txtEmail.Name = "txtEmail";

            txtPassword.Location = new System.Drawing.Point(120, 67);
            txtPassword.Size = new System.Drawing.Size(200, 23);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '*';

            btnLogin.Location = new System.Drawing.Point(120, 110);
            btnLogin.Size = new System.Drawing.Size(100, 30);
            btnLogin.Name = "btnLogin";
            btnLogin.Text = "Prijavi se";
            btnLogin.Click += new System.EventHandler(btnLogin_Click);

            ClientSize = new System.Drawing.Size(380, 180);
            Controls.Add(lblEmail);
            Controls.Add(lblPassword);
            Controls.Add(txtEmail);
            Controls.Add(txtPassword);
            Controls.Add(btnLogin);
            Name = "LoginFrm";
            Text = "Fitness System - Prijava";
            ResumeLayout(false);
        }

        private Label lblEmail;
        private Label lblPassword;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private Button btnLogin;
    }
}