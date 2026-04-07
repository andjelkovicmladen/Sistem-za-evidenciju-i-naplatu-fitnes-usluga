namespace KorisnickiInterfejs
{
    partial class FrmKreirajRacun
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
            grpGlava = new GroupBox();
            lblClan = new Label();
            cmbClan = new ComboBox();
            lblIzdavanje = new Label();
            dtpIzdavanje = new DateTimePicker();
            lblDospece = new Label();
            dtpDospece = new DateTimePicker();
            grpStavke = new GroupBox();
            lblUsluga = new Label();
            cmbUsluga = new ComboBox();
            lblCenaPoSatu = new Label();
            lblBrojSati = new Label();
            txtBrojSati = new TextBox();
            btnDodajStavku = new Button();
            btnUkloniStavku = new Button();
            dgvStavke = new DataGridView();
            lblUkupno = new Label();
            pnlDugmad = new Panel();
            btnSacuvaj = new Button();
            btnOtkazi = new Button();

            grpGlava.SuspendLayout();
            grpStavke.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStavke).BeginInit();
            pnlDugmad.SuspendLayout();
            SuspendLayout();

            // grpGlava
            grpGlava.Text = "Podaci o računu";
            grpGlava.Location = new Point(10, 10);
            grpGlava.Size = new Size(660, 80);
            grpGlava.Controls.AddRange(new Control[] { lblClan, cmbClan, lblIzdavanje, dtpIzdavanje, lblDospece, dtpDospece });

            // lblClan
            lblClan.Text = "Član:";
            lblClan.Location = new Point(10, 30);
            lblClan.Size = new Size(45, 23);

            // cmbClan
            cmbClan.Location = new Point(60, 27);
            cmbClan.Size = new Size(180, 23);
            cmbClan.DropDownStyle = ComboBoxStyle.DropDownList;

            // lblIzdavanje
            lblIzdavanje.Text = "Datum izdavanja:";
            lblIzdavanje.Location = new Point(255, 30);
            lblIzdavanje.Size = new Size(110, 23);

            // dtpIzdavanje
            dtpIzdavanje.Location = new Point(370, 27);
            dtpIzdavanje.Size = new Size(120, 23);
            dtpIzdavanje.Format = DateTimePickerFormat.Short;

            // lblDospece
            lblDospece.Text = "Datum dospeća:";
            lblDospece.Location = new Point(500, 30);
            lblDospece.Size = new Size(105, 23);

            // dtpDospece
            dtpDospece.Location = new Point(610, 27);
            dtpDospece.Size = new Size(120, 23);
            dtpDospece.Format = DateTimePickerFormat.Short;
            grpGlava.Size = new Size(750, 60);

            // grpStavke
            grpStavke.Text = "Stavke računa";
            grpStavke.Location = new Point(10, 100);
            grpStavke.Size = new Size(750, 70);
            grpStavke.Controls.AddRange(new Control[] { lblUsluga, cmbUsluga, lblCenaPoSatu, lblBrojSati, txtBrojSati, btnDodajStavku, btnUkloniStavku });

            // lblUsluga
            lblUsluga.Text = "Usluga:";
            lblUsluga.Location = new Point(10, 32);
            lblUsluga.Size = new Size(55, 23);

            // cmbUsluga
            cmbUsluga.Location = new Point(70, 29);
            cmbUsluga.Size = new Size(200, 23);
            cmbUsluga.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbUsluga.SelectedIndexChanged += new EventHandler(cmbUsluga_SelectedIndexChanged);

            // lblCenaPoSatu
            lblCenaPoSatu.Text = "Cena po satu: -";
            lblCenaPoSatu.Location = new Point(280, 32);
            lblCenaPoSatu.Size = new Size(160, 23);
            lblCenaPoSatu.Name = "lblCenaPoSatu";

            // lblBrojSati
            lblBrojSati.Text = "Broj sati:";
            lblBrojSati.Location = new Point(450, 32);
            lblBrojSati.Size = new Size(65, 23);

            // txtBrojSati
            txtBrojSati.Location = new Point(520, 29);
            txtBrojSati.Size = new Size(60, 23);
            txtBrojSati.Text = "1";
            txtBrojSati.Name = "txtBrojSati";

            // btnDodajStavku
            btnDodajStavku.Text = "+ Dodaj";
            btnDodajStavku.Location = new Point(590, 28);
            btnDodajStavku.Size = new Size(70, 26);
            btnDodajStavku.BackColor = Color.FromArgb(40, 167, 69);
            btnDodajStavku.ForeColor = Color.White;
            btnDodajStavku.FlatStyle = FlatStyle.Flat;
            btnDodajStavku.Click += new EventHandler(btnDodajStavku_Click);

            // btnUkloniStavku
            btnUkloniStavku.Text = "- Ukloni";
            btnUkloniStavku.Location = new Point(665, 28);
            btnUkloniStavku.Size = new Size(70, 26);
            btnUkloniStavku.BackColor = Color.FromArgb(220, 53, 69);
            btnUkloniStavku.ForeColor = Color.White;
            btnUkloniStavku.FlatStyle = FlatStyle.Flat;
            btnUkloniStavku.Click += new EventHandler(btnUkloniStavku_Click);

            // dgvStavke
            dgvStavke.Location = new Point(10, 180);
            dgvStavke.Size = new Size(750, 250);
            dgvStavke.Name = "dgvStavke";
            dgvStavke.ReadOnly = true;
            dgvStavke.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStavke.MultiSelect = false;
            dgvStavke.AllowUserToAddRows = false;
            dgvStavke.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // lblUkupno
            lblUkupno.Text = "Ukupan iznos: 0,00 RSD";
            lblUkupno.Location = new Point(10, 440);
            lblUkupno.Size = new Size(300, 23);
            lblUkupno.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblUkupno.Name = "lblUkupno";

            // pnlDugmad
            pnlDugmad.Location = new Point(540, 435);
            pnlDugmad.Size = new Size(220, 35);
            pnlDugmad.Controls.AddRange(new Control[] { btnSacuvaj, btnOtkazi });

            // btnSacuvaj
            btnSacuvaj.Text = "Sačuvaj račun";
            btnSacuvaj.Location = new Point(0, 5);
            btnSacuvaj.Size = new Size(110, 28);
            btnSacuvaj.BackColor = Color.FromArgb(40, 167, 69);
            btnSacuvaj.ForeColor = Color.White;
            btnSacuvaj.FlatStyle = FlatStyle.Flat;
            btnSacuvaj.Click += new EventHandler(btnSacuvaj_Click);

            // btnOtkazi
            btnOtkazi.Text = "Otkaži";
            btnOtkazi.Location = new Point(115, 5);
            btnOtkazi.Size = new Size(100, 28);
            btnOtkazi.BackColor = Color.FromArgb(108, 117, 125);
            btnOtkazi.ForeColor = Color.White;
            btnOtkazi.FlatStyle = FlatStyle.Flat;
            btnOtkazi.Click += new EventHandler(btnOtkazi_Click);

            // FrmKreirajRacun
            ClientSize = new Size(775, 480);
            Controls.AddRange(new Control[] { grpGlava, grpStavke, dgvStavke, lblUkupno, pnlDugmad });
            Name = "FrmKreirajRacun";
            Text = "Kreiraj novi račun";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            grpGlava.ResumeLayout(false);
            grpStavke.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvStavke).EndInit();
            pnlDugmad.ResumeLayout(false);
            ResumeLayout(false);
        }

        private GroupBox grpGlava, grpStavke;
        private Label lblClan, lblIzdavanje, lblDospece, lblUsluga, lblCenaPoSatu, lblBrojSati, lblUkupno;
        private ComboBox cmbClan, cmbUsluga;
        private DateTimePicker dtpIzdavanje, dtpDospece;
        private TextBox txtBrojSati;
        private Button btnDodajStavku, btnUkloniStavku, btnSacuvaj, btnOtkazi;
        private DataGridView dgvStavke;
        private Panel pnlDugmad;
    }
}
