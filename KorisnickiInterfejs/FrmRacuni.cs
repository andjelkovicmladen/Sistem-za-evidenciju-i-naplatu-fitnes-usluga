using Domen;
using KorisnickiInterfejs.UIKontroler;

namespace KorisnickiInterfejs
{
    public partial class FrmRacuni : Form
    {
        private Administrator prijavljeniAdmin;
        private List<Racun> listaRacuna = new List<Racun>();
        private List<Clan> listaClanove = new List<Clan>();
        private Racun odabraniRacun = null;

        public FrmRacuni(Administrator admin)
        {
            InitializeComponent();
            prijavljeniAdmin = admin;
            UcitajClanove();
            UcitajSveRacune();
        }

        private void UcitajClanove()
        {
            try
            {
                listaClanove = Kontroler.Instance.VratiSveClanove();
                cmbClan.Items.Clear();
                cmbClan.Items.Add(new Clan { IdClan = 0, Ime = "-- Svi članovi --", Prezime = "" });
                foreach (var c in listaClanove)
                    cmbClan.Items.Add(c);
                cmbClan.DisplayMember = "ImePrezime";
                cmbClan.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju članova: " + ex.Message, "Greška",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UcitajSveRacune()
        {
            try
            {
                listaRacuna = Kontroler.Instance.VratiSveRacune();
                PrikaziRacune(listaRacuna);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju računa: " + ex.Message, "Greška",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrikaziRacune(List<Racun> racuni)
        {
            dgvRacuni.DataSource = null;
            var prikaz = racuni.Select(r => new
            {
                ID = r.IdRacun,
                DatumIzdavanja = r.DatumIzdavanja.ToString("dd.MM.yyyy"),
                DatumDospeca = r.DatumDospeca.ToString("dd.MM.yyyy"),
                Administrator = r.AdministratorImePrezime,
                Clan = r.ClanImePrezime,
                UkupanIznos = r.UkupanIznos.ToString("N2") + " RSD"
            }).ToList();
            dgvRacuni.DataSource = prikaz;

            decimal ukupno = racuni.Sum(r => r.UkupanIznos);
            lblUkupno.Text = $"Ukupno: {racuni.Count} računa | Vrednost: {ukupno:N2} RSD";
        }

        private void btnPretrazi_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime? datumOd = chkDatumOd.Checked ? (DateTime?)dtpDatumOd.Value.Date : null;
                DateTime? datumDo = chkDatumDo.Checked ? (DateTime?)dtpDatumDo.Value.Date : null;
                int? idClan = null;
                if (cmbClan.SelectedItem is Clan c && c.IdClan > 0)
                    idClan = c.IdClan;

                listaRacuna = Kontroler.Instance.PretraziRacun(datumOd, datumDo, idClan, null);
                PrikaziRacune(listaRacuna);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOsvezi_Click(object sender, EventArgs e)
        {
            chkDatumOd.Checked = false;
            chkDatumDo.Checked = false;
            cmbClan.SelectedIndex = 0;
            UcitajSveRacune();
        }

        private void btnKreirajRacun_Click(object sender, EventArgs e)
        {
            FrmKreirajRacun frm = new FrmKreirajRacun(prijavljeniAdmin, listaClanove);
            if (frm.ShowDialog() == DialogResult.OK)
                UcitajSveRacune();
        }

        private void btnDetalji_Click(object sender, EventArgs e)
        {
            if (odabraniRacun == null)
            {
                MessageBox.Show("Odaberite račun za pregled detalja.", "Upozorenje",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                Racun racunSaStavkama = Kontroler.Instance.VratiRacunSaStavkama(odabraniRacun.IdRacun);
                FrmDetaljiRacuna frm = new FrmDetaljiRacuna(racunSaStavkama);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvRacuni_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRacuni.SelectedRows.Count > 0)
            {
                int id = (int)dgvRacuni.SelectedRows[0].Cells["ID"].Value;
                odabraniRacun = listaRacuna.FirstOrDefault(r => r.IdRacun == id);
            }
            else
            {
                odabraniRacun = null;
            }
        }

        private void chkDatumOd_CheckedChanged(object sender, EventArgs e)
        {
            dtpDatumOd.Enabled = chkDatumOd.Checked;
        }

        private void chkDatumDo_CheckedChanged(object sender, EventArgs e)
        {
            dtpDatumDo.Enabled = chkDatumDo.Checked;
        }
    }
}
