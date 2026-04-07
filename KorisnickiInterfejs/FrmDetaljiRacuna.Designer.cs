namespace KorisnickiInterfejs
{
    partial class FrmDetaljiRacuna
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
            lblRacunBr = new Label();
            lblDatumIzdavanja = new Label();
            lblDatumDospeca = new Label();
            lblAdministrator = new Label();
            lblClan = new Label();
            dgvStavke = new DataGridView();
            lblUkupno = new Label();
            btnZatvori = new Button();

            ((System.ComponentModel.ISupportInitialize)dgvStavke).BeginInit();
            SuspendLayout();

            // lblRacunBr
            lblRacunBr.Text = "Račun br. -";
            lblRacunBr.Location = new Point(15, 15);
            lblRacunBr.Size = new Size(200, 26);
            lblRacunBr.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblRacunBr.Name = "lblRacunBr";

            // lblDatumIzdavanja
            lblDatumIzdavanja.Text = "Datum izdavanja:";
            lblDatumIzdavanja.Location = new Point(15, 50);
            lblDatumIzdavanja.Size = new Size(250, 20);
            lblDatumIzdavanja.Name = "lblDatumIzdavanja";

            // lblDatumDospeca
            lblDatumDospeca.Text = "Datum dospeća:";
            lblDatumDospeca.Location = new Point(280, 50);
            lblDatumDospeca.Size = new Size(250, 20);
            lblDatumDospeca.Name = "lblDatumDospeca";

            // lblAdministrator
            lblAdministrator.Text = "Administrator:";
            lblAdministrator.Location = new Point(15, 75);
            lblAdministrator.Size = new Size(280, 20);
            lblAdministrator.Name = "lblAdministrator";

            // lblClan
            lblClan.Text = "Član:";
            lblClan.Location = new Point(310, 75);
            lblClan.Size = new Size(280, 20);
            lblClan.Name = "lblClan";

            // dgvStavke
            dgvStavke.Location = new Point(15, 105);
            dgvStavke.Size = new Size(660, 250);
            dgvStavke.ReadOnly = true;
            dgvStavke.AllowUserToAddRows = false;
            dgvStavke.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStavke.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // lblUkupno
            lblUkupno.Text = "UKUPNO: 0,00 RSD";
            lblUkupno.Location = new Point(15, 365);
            lblUkupno.Size = new Size(300, 26);
            lblUkupno.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblUkupno.Name = "lblUkupno";

            // btnZatvori
            btnZatvori.Text = "Zatvori";
            btnZatvori.Location = new Point(575, 362);
            btnZatvori.Size = new Size(100, 30);
            btnZatvori.BackColor = Color.FromArgb(108, 117, 125);
            btnZatvori.ForeColor = Color.White;
            btnZatvori.FlatStyle = FlatStyle.Flat;
            btnZatvori.Click += new EventHandler(btnZatvori_Click);

            // FrmDetaljiRacuna
            ClientSize = new Size(690, 405);
            Controls.AddRange(new Control[] { lblRacunBr, lblDatumIzdavanja, lblDatumDospeca, lblAdministrator, lblClan, dgvStavke, lblUkupno, btnZatvori });
            Name = "FrmDetaljiRacuna";
            Text = "Detalji računa";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            ((System.ComponentModel.ISupportInitialize)dgvStavke).EndInit();
            ResumeLayout(false);
        }

        private Label lblRacunBr, lblDatumIzdavanja, lblDatumDospeca, lblAdministrator, lblClan, lblUkupno;
        private DataGridView dgvStavke;
        private Button btnZatvori;
    }
}
