using Domen;
using KorisnickiInterfejs.UIKontroler;

namespace KorisnickiInterfejs
{
    public partial class FrmKreirajRacun : Form
    {
        private Administrator prijavljeniAdmin;
        private List<Clan> listaClanove;
        private List<FitnesUsluga> listaUsluga = new List<FitnesUsluga>();
        private List<StavkaRacuna> stavke = new List<StavkaRacuna>();

        public FrmKreirajRacun(Administrator admin, List<Clan> clanovi)
        {
            InitializeComponent();
            prijavljeniAdmin = admin;
            listaClanove = clanovi;
            PopuniClanove();
            UcitajUsluge();
            dtpIzdavanje.Value = DateTime.Today;
            dtpDospece.Value = DateTime.Today.AddDays(30);
        }

        private void PopuniClanove()
        {
            cmbClan.Items.Clear();
            foreach (var c in listaClanove)
                cmbClan.Items.Add(c);
            cmbClan.DisplayMember = "ImePrezime";
            if (cmbClan.Items.Count > 0)
                cmbClan.SelectedIndex = 0;
        }

        private void UcitajUsluge()
        {
            try
            {
                listaUsluga = Kontroler.Instance.VratiSveUsluge();
                cmbUsluga.Items.Clear();
                foreach (var u in listaUsluga)
                    cmbUsluga.Items.Add(u);
                cmbUsluga.DisplayMember = "Naziv";
                if (cmbUsluga.Items.Count > 0)
                    cmbUsluga.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju usluga: " + ex.Message, "Greška",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDodajStavku_Click(object sender, EventArgs e)
        {
            if (cmbUsluga.SelectedItem == null)
            {
                MessageBox.Show("Odaberite uslugu!", "Validacija", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(txtBrojSati.Text, out int brojSati) || brojSati <= 0)
            {
                MessageBox.Show("Unesite ispravan broj sati (ceo pozitivan broj)!", "Validacija",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBrojSati.Focus();
                return;
            }

            FitnesUsluga usluga = (FitnesUsluga)cmbUsluga.SelectedItem;
            decimal iznos = usluga.CenaPoSatu * brojSati;

            StavkaRacuna stavka = new StavkaRacuna
            {
                Rb = stavke.Count + 1,
                IdFitnesUsluga = usluga.IdFitnesUsluga,
                BrojSati = brojSati,
                Iznos = iznos,
                FitnesUsluga = usluga
            };

            stavke.Add(stavka);
            OsveziTabetuStavki();
            txtBrojSati.Text = "1";
        }

        private void btnUkloniStavku_Click(object sender, EventArgs e)
        {
            if (dgvStavke.SelectedRows.Count == 0)
            {
                MessageBox.Show("Odaberite stavku za uklanjanje.", "Upozorenje",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int rb = (int)dgvStavke.SelectedRows[0].Cells["Rb"].Value;
            stavke.RemoveAll(s => s.Rb == rb);

            // Renumber
            for (int i = 0; i < stavke.Count; i++)
                stavke[i].Rb = i + 1;
            OsveziTabetuStavki();
        }

        private void OsveziTabetuStavki()
        {
            dgvStavke.DataSource = null;
            var prikaz = stavke.Select(s => new
            {
                Rb = s.Rb,
                Usluga = s.FitnesUsluga?.Naziv ?? "N/A",
                BrojSati = s.BrojSati,
                CenaPoSatu = s.FitnesUsluga?.CenaPoSatu.ToString("N2") + " RSD",
                Iznos = s.Iznos.ToString("N2") + " RSD"
            }).ToList();
            dgvStavke.DataSource = prikaz;

            decimal ukupno = stavke.Sum(s => s.Iznos);
            lblUkupno.Text = $"Ukupan iznos: {ukupno:N2} RSD";
        }

        private void cmbUsluga_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbUsluga.SelectedItem is FitnesUsluga u)
                lblCenaPoSatu.Text = $"Cena po satu: {u.CenaPoSatu:N2} RSD";
        }

        private void btnSacuvaj_Click(object sender, EventArgs e)
        {
            if (cmbClan.SelectedItem == null)
            {
                MessageBox.Show("Odaberite člana!", "Validacija", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (stavke.Count == 0)
            {
                MessageBox.Show("Dodajte barem jednu stavku na račun!", "Validacija",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                Clan odabraniClan = (Clan)cmbClan.SelectedItem;

                Racun racun = new Racun
                {
                    DatumIzdavanja = dtpIzdavanje.Value.Date,
                    DatumDospeca = dtpDospece.Value.Date,
                    IdAdministrator = prijavljeniAdmin.IdAdministrator,
                    IdClan = odabraniClan.IdClan,
                    StavkeRacuna = stavke
                };

                Kontroler.Instance.KreirajRacun(racun);
                MessageBox.Show("Račun je uspešno kreiran!", "Uspeh",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOtkazi_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
