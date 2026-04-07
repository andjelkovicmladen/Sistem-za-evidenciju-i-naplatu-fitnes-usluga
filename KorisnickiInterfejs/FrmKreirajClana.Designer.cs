namespace KorisnickiInterfejs
{
    partial class FrmKreirajClana
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblIme = new Label();
            txtIme = new TextBox();
            lblPrezime = new Label();
            txtPrezime = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblTelefon = new Label();
            txtTelefon = new TextBox();
            lblPassword = new Label();
            txtPassword = new TextBox();
            lblTipClanarine = new Label();
            cmbTipClanarine = new ComboBox();
            btnSacuvaj = new Button();
            btnOtkazi = new Button();
            SuspendLayout();

            int lx = 20, tx = 160, w = 220, lw = 130, h = 23, gap = 35;
            int row1 = 20, row2 = row1 + gap, row3 = row2 + gap, row4 = row3 + gap, row5 = row4 + gap, row6 = row5 + gap;

            // lblIme
            lblIme.Text = "Ime:";
            lblIme.Location = new Point(lx, row1 + 3);
            lblIme.Size = new Size(lw, h);

            // txtIme
            txtIme.Location = new Point(tx, row1);
            txtIme.Size = new Size(w, h);
            txtIme.Name = "txtIme";

            // lblPrezime
            lblPrezime.Text = "Prezime:";
            lblPrezime.Location = new Point(lx, row2 + 3);
            lblPrezime.Size = new Size(lw, h);

            // txtPrezime
            txtPrezime.Location = new Point(tx, row2);
            txtPrezime.Size = new Size(w, h);
            txtPrezime.Name = "txtPrezime";

            // lblEmail
            lblEmail.Text = "Email:";
            lblEmail.Location = new Point(lx, row3 + 3);
            lblEmail.Size = new Size(lw, h);

            // txtEmail
            txtEmail.Location = new Point(tx, row3);
            txtEmail.Size = new Size(w, h);
            txtEmail.Name = "txtEmail";

            // lblTelefon
            lblTelefon.Text = "Broj telefona:";
            lblTelefon.Location = new Point(lx, row4 + 3);
            lblTelefon.Size = new Size(lw, h);

            // txtTelefon
            txtTelefon.Location = new Point(tx, row4);
            txtTelefon.Size = new Size(w, h);
            txtTelefon.Name = "txtTelefon";

            // lblPassword
            lblPassword.Text = "Lozinka:";
            lblPassword.Location = new Point(lx, row5 + 3);
            lblPassword.Size = new Size(lw, h);

            // txtPassword
            txtPassword.Location = new Point(tx, row5);
            txtPassword.Size = new Size(w, h);
            txtPassword.Name = "txtPassword";

            // lblTipClanarine
            lblTipClanarine.Text = "Tip članarine:";
            lblTipClanarine.Location = new Point(lx, row6 + 3);
            lblTipClanarine.Size = new Size(lw, h);

            // cmbTipClanarine
            cmbTipClanarine.Location = new Point(tx, row6);
            cmbTipClanarine.Size = new Size(w, h);
            cmbTipClanarine.Name = "cmbTipClanarine";
            cmbTipClanarine.DropDownStyle = ComboBoxStyle.DropDownList;

            // btnSacuvaj
            btnSacuvaj.Text = "Sačuvaj";
            btnSacuvaj.Location = new Point(tx, row6 + 45);
            btnSacuvaj.Size = new Size(100, 30);
            btnSacuvaj.BackColor = Color.FromArgb(40, 167, 69);
            btnSacuvaj.ForeColor = Color.White;
            btnSacuvaj.FlatStyle = FlatStyle.Flat;
            btnSacuvaj.Click += new EventHandler(btnSacuvaj_Click);

            // btnOtkazi
            btnOtkazi.Text = "Otkaži";
            btnOtkazi.Location = new Point(tx + 110, row6 + 45);
            btnOtkazi.Size = new Size(100, 30);
            btnOtkazi.BackColor = Color.FromArgb(108, 117, 125);
            btnOtkazi.ForeColor = Color.White;
            btnOtkazi.FlatStyle = FlatStyle.Flat;
            btnOtkazi.Click += new EventHandler(btnOtkazi_Click);

            // FrmKreirajClana
            ClientSize = new Size(420, row6 + 95);
            Controls.AddRange(new Control[]
            {
                lblIme, txtIme, lblPrezime, txtPrezime, lblEmail, txtEmail,
                lblTelefon, txtTelefon, lblPassword, txtPassword,
                lblTipClanarine, cmbTipClanarine, btnSacuvaj, btnOtkazi
            });
            Name = "FrmKreirajClana";
            Text = "Dodaj novog člana";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ResumeLayout(false);
        }

        private Label lblIme, lblPrezime, lblEmail, lblTelefon, lblPassword, lblTipClanarine;
        private TextBox txtIme, txtPrezime, txtEmail, txtTelefon, txtPassword;
        private ComboBox cmbTipClanarine;
        private Button btnSacuvaj, btnOtkazi;
    }
}
