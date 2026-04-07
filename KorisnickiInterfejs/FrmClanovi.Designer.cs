namespace KorisnickiInterfejs
{
    partial class FrmClanovi
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
            grpPretraga = new GroupBox();
            lblIme = new Label();
            txtIme = new TextBox();
            lblPrezime = new Label();
            txtPrezime = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblTip = new Label();
            cmbTip = new ComboBox();
            btnPretrazi = new Button();
            btnOsvezi = new Button();
            dgvClanovi = new DataGridView();
            pnlDugmad = new Panel();
            btnDodaj = new Button();
            btnIzmeni = new Button();
            btnObrisi = new Button();
            lblBroj = new Label();

            grpPretraga.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvClanovi).BeginInit();
            pnlDugmad.SuspendLayout();
            SuspendLayout();

            // grpPretraga
            grpPretraga.Text = "Pretraga";
            grpPretraga.Location = new Point(10, 10);
            grpPretraga.Size = new Size(960, 80);
            grpPretraga.Controls.AddRange(new Control[] { lblIme, txtIme, lblPrezime, txtPrezime, lblEmail, txtEmail, lblTip, cmbTip, btnPretrazi, btnOsvezi });

            // lblIme
            lblIme.Text = "Ime:";
            lblIme.Location = new Point(10, 28);
            lblIme.Size = new Size(40, 23);

            // txtIme
            txtIme.Location = new Point(55, 25);
            txtIme.Size = new Size(130, 23);
            txtIme.Name = "txtIme";

            // lblPrezime
            lblPrezime.Text = "Prezime:";
            lblPrezime.Location = new Point(200, 28);
            lblPrezime.Size = new Size(60, 23);

            // txtPrezime
            txtPrezime.Location = new Point(265, 25);
            txtPrezime.Size = new Size(130, 23);
            txtPrezime.Name = "txtPrezime";

            // lblEmail
            lblEmail.Text = "Email:";
            lblEmail.Location = new Point(410, 28);
            lblEmail.Size = new Size(45, 23);

            // txtEmail
            txtEmail.Location = new Point(460, 25);
            txtEmail.Size = new Size(150, 23);
            txtEmail.Name = "txtEmail";

            // lblTip
            lblTip.Text = "Tip čl.:";
            lblTip.Location = new Point(625, 28);
            lblTip.Size = new Size(55, 23);

            // cmbTip
            cmbTip.Location = new Point(685, 25);
            cmbTip.Size = new Size(140, 23);
            cmbTip.Name = "cmbTip";
            cmbTip.DropDownStyle = ComboBoxStyle.DropDownList;

            // btnPretrazi
            btnPretrazi.Text = "Pretraži";
            btnPretrazi.Location = new Point(840, 24);
            btnPretrazi.Size = new Size(60, 26);
            btnPretrazi.Click += new EventHandler(btnPretrazi_Click);

            // btnOsvezi
            btnOsvezi.Text = "↺";
            btnOsvezi.Location = new Point(905, 24);
            btnOsvezi.Size = new Size(40, 26);
            btnOsvezi.Click += new EventHandler(btnOsvezi_Click);

            // dgvClanovi
            dgvClanovi.Location = new Point(10, 100);
            dgvClanovi.Size = new Size(960, 380);
            dgvClanovi.Name = "dgvClanovi";
            dgvClanovi.ReadOnly = true;
            dgvClanovi.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvClanovi.MultiSelect = false;
            dgvClanovi.AllowUserToAddRows = false;
            dgvClanovi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvClanovi.SelectionChanged += new EventHandler(dgvClanovi_SelectionChanged);

            // lblBroj
            lblBroj.Text = "Ukupno: 0 članova";
            lblBroj.Location = new Point(10, 490);
            lblBroj.Size = new Size(200, 23);
            lblBroj.Name = "lblBroj";

            // pnlDugmad
            pnlDugmad.Location = new Point(770, 485);
            pnlDugmad.Size = new Size(200, 35);
            pnlDugmad.Controls.AddRange(new Control[] { btnDodaj, btnIzmeni, btnObrisi });

            // btnDodaj
            btnDodaj.Text = "Dodaj";
            btnDodaj.Location = new Point(0, 5);
            btnDodaj.Size = new Size(60, 26);
            btnDodaj.BackColor = Color.FromArgb(40, 167, 69);
            btnDodaj.ForeColor = Color.White;
            btnDodaj.FlatStyle = FlatStyle.Flat;
            btnDodaj.Click += new EventHandler(btnDodaj_Click);

            // btnIzmeni
            btnIzmeni.Text = "Izmeni";
            btnIzmeni.Location = new Point(65, 5);
            btnIzmeni.Size = new Size(65, 26);
            btnIzmeni.BackColor = Color.FromArgb(0, 123, 255);
            btnIzmeni.ForeColor = Color.White;
            btnIzmeni.FlatStyle = FlatStyle.Flat;
            btnIzmeni.Click += new EventHandler(btnIzmeni_Click);

            // btnObrisi
            btnObrisi.Text = "Obriši";
            btnObrisi.Location = new Point(135, 5);
            btnObrisi.Size = new Size(65, 26);
            btnObrisi.BackColor = Color.FromArgb(220, 53, 69);
            btnObrisi.ForeColor = Color.White;
            btnObrisi.FlatStyle = FlatStyle.Flat;
            btnObrisi.Click += new EventHandler(btnObrisi_Click);

            // FrmClanovi
            ClientSize = new Size(985, 530);
            Controls.AddRange(new Control[] { grpPretraga, dgvClanovi, lblBroj, pnlDugmad });
            Name = "FrmClanovi";
            Text = "Upravljanje članovima";
            StartPosition = FormStartPosition.CenterParent;

            grpPretraga.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvClanovi).EndInit();
            pnlDugmad.ResumeLayout(false);
            ResumeLayout(false);
        }

        private GroupBox grpPretraga;
        private Label lblIme, lblPrezime, lblEmail, lblTip, lblBroj;
        private TextBox txtIme, txtPrezime, txtEmail;
        private ComboBox cmbTip;
        private Button btnPretrazi, btnOsvezi, btnDodaj, btnIzmeni, btnObrisi;
        private DataGridView dgvClanovi;
        private Panel pnlDugmad;
    }
}
