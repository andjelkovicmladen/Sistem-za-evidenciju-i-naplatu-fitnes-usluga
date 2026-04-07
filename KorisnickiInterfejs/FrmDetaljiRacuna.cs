using Domen;

namespace KorisnickiInterfejs
{
    public partial class FrmDetaljiRacuna : Form
    {
        public FrmDetaljiRacuna(Racun racun)
        {
            InitializeComponent();
            PrikaziDetalje(racun);
        }

        private void PrikaziDetalje(Racun racun)
        {
            lblRacunBr.Text = $"Račun br. {racun.IdRacun}";
            lblDatumIzdavanja.Text = $"Datum izdavanja: {racun.DatumIzdavanja:dd.MM.yyyy}";
            lblDatumDospeca.Text = $"Datum dospeća: {racun.DatumDospeca:dd.MM.yyyy}";
            lblAdministrator.Text = $"Administrator: {racun.AdministratorImePrezime}";
            lblClan.Text = $"Član: {racun.ClanImePrezime}";

            var prikaz = racun.StavkeRacuna.Select(s => new
            {
                Rb = s.Rb,
                Usluga = s.FitnesUsluga?.Naziv ?? "N/A",
                BrojSati = s.BrojSati,
                CenaPoSatu = s.FitnesUsluga?.CenaPoSatu.ToString("N2") + " RSD",
                Iznos = s.Iznos.ToString("N2") + " RSD"
            }).ToList();
            dgvStavke.DataSource = prikaz;

            decimal ukupno = racun.StavkeRacuna.Sum(s => s.Iznos);
            lblUkupno.Text = $"UKUPNO: {ukupno:N2} RSD";
        }

        private void btnZatvori_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
