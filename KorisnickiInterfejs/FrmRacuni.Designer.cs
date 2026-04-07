namespace KorisnickiInterfejs
{
    partial class FrmRacuni
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
            chkDatumOd = new CheckBox();
            dtpDatumOd = new DateTimePicker();
            chkDatumDo = new CheckBox();
            dtpDatumDo = new DateTimePicker();
            lblClan = new Label();
            cmbClan = new ComboBox();
            btnPretrazi = new Button();
            btnOsvezi = new Button();
            dgvRacuni = new DataGridView();
            lblUkupno = new Label();
            pnlDugmad = new Panel();
            btnKreirajRacun = new Button();
            btnDetalji = new Button();

            grpPretraga.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRacuni).BeginInit();
            pnlDugmad.SuspendLayout();
            SuspendLayout();

            // grpPretraga
            grpPretraga.Text = "Pretraga";
            grpPretraga.Location = new Point(10, 10);
            grpPretraga.Size = new Size(960, 80);
            grpPretraga.Controls.AddRange(new Control[]
            { chkDatumOd, dtpDatumOd, chkDatumDo, dtpDatumDo, lblClan, cmbClan, btnPretrazi, btnOsvezi });

            // chkDatumOd
            chkDatumOd.Text = "Datum od:";
            chkDatumOd.Location = new Point(10, 28);
            chkDatumOd.Size = new Size(90, 23);
            chkDatumOd.CheckedChanged += new EventHandler(chkDatumOd_CheckedChanged);

            // dtpDatumOd
            dtpDatumOd.Location = new Point(105, 25);
            dtpDatumOd.Size = new Size(140, 23);
            dtpDatumOd.Format = DateTimePickerFormat.Short;
            dtpDatumOd.Enabled = false;

            // chkDatumDo
            chkDatumDo.Text = "Datum do:";
            chkDatumDo.Location = new Point(260, 28);
            chkDatumDo.Size = new Size(90, 23);
            chkDatumDo.CheckedChanged += new EventHandler(chkDatumDo_CheckedChanged);

            // dtpDatumDo
            dtpDatumDo.Location = new Point(355, 25);
            dtpDatumDo.Size = new Size(140, 23);
            dtpDatumDo.Format = DateTimePickerFormat.Short;
            dtpDatumDo.Enabled = false;

            // lblClan
            lblClan.Text = "Član:";
            lblClan.Location = new Point(510, 28);
            lblClan.Size = new Size(40, 23);

            // cmbClan
            cmbClan.Location = new Point(555, 25);
            cmbClan.Size = new Size(200, 23);
            cmbClan.Name = "cmbClan";
            cmbClan.DropDownStyle = ComboBoxStyle.DropDownList;

            // btnPretrazi
            btnPretrazi.Text = "Pretraži";
            btnPretrazi.Location = new Point(770, 24);
            btnPretrazi.Size = new Size(80, 26);
            btnPretrazi.Click += new EventHandler(btnPretrazi_Click);

            // btnOsvezi
            btnOsvezi.Text = "↺";
            btnOsvezi.Location = new Point(855, 24);
            btnOsvezi.Size = new Size(40, 26);
            btnOsvezi.Click += new EventHandler(btnOsvezi_Click);

            // dgvRacuni
            dgvRacuni.Location = new Point(10, 100);
            dgvRacuni.Size = new Size(960, 380);
            dgvRacuni.Name = "dgvRacuni";
            dgvRacuni.ReadOnly = true;
            dgvRacuni.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRacuni.MultiSelect = false;
            dgvRacuni.AllowUserToAddRows = false;
            dgvRacuni.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRacuni.SelectionChanged += new EventHandler(dgvRacuni_SelectionChanged);

            // lblUkupno
            lblUkupno.Text = "Ukupno: 0 računa";
            lblUkupno.Location = new Point(10, 490);
            lblUkupno.Size = new Size(400, 23);
            lblUkupno.Name = "lblUkupno";

            // pnlDugmad
            pnlDugmad.Location = new Point(760, 485);
            pnlDugmad.Size = new Size(210, 35);
            pnlDugmad.Controls.AddRange(new Control[] { btnKreirajRacun, btnDetalji });

            // btnKreirajRacun
            btnKreirajRacun.Text = "Novi račun";
            btnKreirajRacun.Location = new Point(0, 5);
            btnKreirajRacun.Size = new Size(100, 26);
            btnKreirajRacun.BackColor = Color.FromArgb(40, 167, 69);
            btnKreirajRacun.ForeColor = Color.White;
            btnKreirajRacun.FlatStyle = FlatStyle.Flat;
            btnKreirajRacun.Click += new EventHandler(btnKreirajRacun_Click);

            // btnDetalji
            btnDetalji.Text = "Detalji";
            btnDetalji.Location = new Point(108, 5);
            btnDetalji.Size = new Size(100, 26);
            btnDetalji.BackColor = Color.FromArgb(0, 123, 255);
            btnDetalji.ForeColor = Color.White;
            btnDetalji.FlatStyle = FlatStyle.Flat;
            btnDetalji.Click += new EventHandler(btnDetalji_Click);

            // FrmRacuni
            ClientSize = new Size(985, 530);
            Controls.AddRange(new Control[] { grpPretraga, dgvRacuni, lblUkupno, pnlDugmad });
            Name = "FrmRacuni";
            Text = "Upravljanje računima";
            StartPosition = FormStartPosition.CenterParent;

            grpPretraga.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvRacuni).EndInit();
            pnlDugmad.ResumeLayout(false);
            ResumeLayout(false);
        }

        private GroupBox grpPretraga;
        private CheckBox chkDatumOd, chkDatumDo;
        private DateTimePicker dtpDatumOd, dtpDatumDo;
        private Label lblClan, lblUkupno;
        private ComboBox cmbClan;
        private Button btnPretrazi, btnOsvezi, btnKreirajRacun, btnDetalji;
        private DataGridView dgvRacuni;
        private Panel pnlDugmad;
    }
}
