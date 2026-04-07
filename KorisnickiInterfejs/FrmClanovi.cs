using Domen;
using KorisnickiInterfejs.UIKontroler;

namespace KorisnickiInterfejs
{
    public partial class FrmClanovi : Form
    {
        private Administrator prijavljeniAdmin;
        private List<Clan> listaClanova = new List<Clan>();
        private List<TipClanarine> listaTipova = new List<TipClanarine>();
        private Clan odabraniClan = null;

        public FrmClanovi(Administrator admin)
        {
            InitializeComponent();
            prijavljeniAdmin = admin;
            UcitajTipoveClanarine();
            UcitajSveClanove();
        }

        private void UcitajTipoveClanarine()
        {
            try
            {
                listaTipova = Kontroler.Instance.VratiSveTipoveClanarine();
                cmbTip.Items.Clear();
                cmbTip.Items.Add(new TipClanarine { IdTipClanarine = 0, Naziv = "-- Svi tipovi --" });
                foreach (var tip in listaTipova)
                    cmbTip.Items.Add(tip);
                cmbTip.DisplayMember = "Naziv";
                cmbTip.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju tipova članarine: " + ex.Message, "Greška",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UcitajSveClanove()
        {
            try
            {
                listaClanova = Kontroler.Instance.VratiSveClanove();
                PrikaziClanove(listaClanova);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greška pri učitavanju članova: " + ex.Message, "Greška",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PrikaziClanove(List<Clan> clanovi)
        {
            dgvClanovi.DataSource = null;
            var prikaz = clanovi.Select(c => new
            {
                ID = c.IdClan,
                Ime = c.Ime,
                Prezime = c.Prezime,
                Email = c.Email,
                Telefon = c.BrojTelefona,
                TipClanarine = listaTipova.FirstOrDefault(t => t.IdTipClanarine == c.IdTipClanarine)?.Naziv ?? "N/A"
            }).ToList();
            dgvClanovi.DataSource = prikaz;
            lblBroj.Text = $"Ukupno: {clanovi.Count} članova";
        }

        private void btnPretrazi_Click(object sender, EventArgs e)
        {
            try
            {
                int? tipId = null;
                if (cmbTip.SelectedItem is TipClanarine odabraniTip && odabraniTip.IdTipClanarine > 0)
                    tipId = odabraniTip.IdTipClanarine;

                var rezultat = Kontroler.Instance.PretraziClana(
                    txtIme.Text.Trim(),
                    txtPrezime.Text.Trim(),
                    txtEmail.Text.Trim(),
                    tipId);
                listaClanova = rezultat;
                PrikaziClanove(listaClanova);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOsvezi_Click(object sender, EventArgs e)
        {
            txtIme.Text = "";
            txtPrezime.Text = "";
            txtEmail.Text = "";
            cmbTip.SelectedIndex = 0;
            UcitajSveClanove();
        }

        private void btnDodaj_Click(object sender, EventArgs e)
        {
            FrmKreirajClana frm = new FrmKreirajClana(listaTipova);
            if (frm.ShowDialog() == DialogResult.OK)
                UcitajSveClanove();
        }

        private void btnIzmeni_Click(object sender, EventArgs e)
        {
            if (odabraniClan == null)
            {
                MessageBox.Show("Molimo odaberite člana za izmenu.", "Upozorenje",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            FrmKreirajClana frm = new FrmKreirajClana(listaTipova, odabraniClan);
            if (frm.ShowDialog() == DialogResult.OK)
                UcitajSveClanove();
        }

        private void btnObrisi_Click(object sender, EventArgs e)
        {
            if (odabraniClan == null)
            {
                MessageBox.Show("Molimo odaberite člana za brisanje.", "Upozorenje",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var potvrda = MessageBox.Show(
                $"Da li ste sigurni da želite da obrišete člana {odabraniClan.ImePrezime}?",
                "Potvrda brisanja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (potvrda == DialogResult.Yes)
            {
                try
                {
                    Kontroler.Instance.ObrisiClana(odabraniClan.IdClan);
                    MessageBox.Show("Član je uspešno obrisan.", "Uspeh",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    odabraniClan = null;
                    UcitajSveClanove();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvClanovi_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvClanovi.SelectedRows.Count > 0)
            {
                int id = (int)dgvClanovi.SelectedRows[0].Cells["ID"].Value;
                odabraniClan = listaClanova.FirstOrDefault(c => c.IdClan == id);
            }
            else
            {
                odabraniClan = null;
            }
        }
    }
}
