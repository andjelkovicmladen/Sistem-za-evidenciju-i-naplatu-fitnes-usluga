using BrokerBazePodataka;
using Domen;
using Zajednicki;

namespace WebAPI.Services
{
    public interface IRacunService
    {
        List<Racun> GetAllRacuns();
        Racun GetRacunById(int id);
        Racun GetRacunWithStavke(int id);
        void CreateRacun(Racun racun);
        void UpdateRacun(Racun racun);
        List<Racun> SearchRacuns(RacunSearchParametri parametri);
    }

    public class RacunService : IRacunService
    {
        private readonly BrokerBP _broker;

        // Bazni SELECT koji popunjava sve kolone koje Racun.GetReaderList očekuje
        // (uključujući imena administratora/člana i ukupan iznos iz stavki).
        private const string RacunSelect = @"
            SELECT r.idRacun, r.DatumIzdavanja, r.DatumDospeca, r.idAdministrator, r.idClan,
                   (a.Ime + ' ' + a.Prezime) AS Administrator,
                   (c.Ime + ' ' + c.Prezime) AS Clan,
                   ISNULL((SELECT SUM(s.Iznos) FROM StavkaRacuna s WHERE s.idRacun = r.idRacun), 0) AS UkupanIznos
            FROM Racun r
            INNER JOIN Administrator a ON a.idAdministrator = r.idAdministrator
            INNER JOIN Clan c ON c.idClan = r.idClan";

        public RacunService()
        {
            _broker = new BrokerBP();
        }

        public List<Racun> GetAllRacuns()
        {
            try
            {
                string upit = RacunSelect + " ORDER BY r.idRacun DESC";
                return _broker.ExecuteQuery(new Racun(), upit).Cast<Racun>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri preuzimanju svih računa: {ex.Message}", ex);
            }
        }

        public Racun GetRacunById(int id)
        {
            try
            {
                string upit = RacunSelect + " WHERE r.idRacun=@id";
                var parametri = new Dictionary<string, object> { { "@id", id } };
                var rezultat = _broker.ExecuteQuery(new Racun(), upit, parametri);

                return rezultat.Count > 0 ? (Racun)rezultat[0] : null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri preuzimanju računa: {ex.Message}", ex);
            }
        }

        public Racun GetRacunWithStavke(int id)
        {
            try
            {
                var racun = GetRacunById(id);
                if (racun == null)
                    return null;

                // StavkaRacuna u bazi koristi idFitnessUsluga; spajamo uslugu za naziv/cenu.
                string upitStavke = @"
                    SELECT s.idRacun, s.rb, s.idFitnessUsluga AS idFitnesUsluga, s.BrojSati, s.Iznos,
                           u.Naziv, u.CenaPoSatu
                    FROM StavkaRacuna s
                    INNER JOIN FitnessUsluga u ON u.idFitnessUsluga = s.idFitnessUsluga
                    WHERE s.idRacun=@id
                    ORDER BY s.rb";
                var parametri = new Dictionary<string, object> { { "@id", id } };
                var stavke = _broker.ExecuteQuery(new StavkaRacuna(), upitStavke, parametri);

                racun.StavkeRacuna = stavke.Cast<StavkaRacuna>().ToList();
                return racun;
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri preuzimanju računa sa stavkama: {ex.Message}", ex);
            }
        }

        public void CreateRacun(Racun racun)
        {
            if (racun == null)
                throw new ArgumentNullException(nameof(racun));

            try
            {
                _broker.BeginTransaction();
                try
                {
                    int idRacun = _broker.InsertWithIdentity(racun, "idRacun");
                    int rb = 1;
                    foreach (var stavka in racun.StavkeRacuna ?? new List<StavkaRacuna>())
                    {
                        // Ručni INSERT zbog naziva kolone idFitnessUsluga u bazi.
                        string upit = @"INSERT INTO StavkaRacuna (idRacun, rb, idFitnessUsluga, BrojSati, Iznos)
                                        VALUES (@idRacun, @rb, @idFitnessUsluga, @BrojSati, @Iznos)";
                        var parametri = new Dictionary<string, object>
                        {
                            { "@idRacun", idRacun },
                            { "@rb", rb++ },
                            { "@idFitnessUsluga", stavka.IdFitnesUsluga },
                            { "@BrojSati", stavka.BrojSati },
                            { "@Iznos", stavka.Iznos }
                        };
                        _broker.ExecuteScalar(upit, parametri);
                    }
                    _broker.Commit();
                    racun.IdRacun = idRacun;
                }
                catch
                {
                    _broker.Rollback();
                    throw;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri kreiranju računa: {ex.Message}", ex);
            }
            finally
            {
                _broker.CloseConnection();
            }
        }

        public void UpdateRacun(Racun racun)
        {
            if (racun == null)
                throw new ArgumentNullException(nameof(racun));

            try
            {
                _broker.Update(racun);
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri ažuriranju računa: {ex.Message}", ex);
            }
        }

        public List<Racun> SearchRacuns(RacunSearchParametri parametri)
        {
            if (parametri == null)
                throw new ArgumentNullException(nameof(parametri));

            try
            {
                string upit = RacunSelect + " WHERE 1=1";
                var queryParams = new Dictionary<string, object>();

                if (parametri.IdClan > 0)
                {
                    upit += " AND r.idClan = @idClan";
                    queryParams["@idClan"] = parametri.IdClan;
                }
                if (parametri.IdAdministrator > 0)
                {
                    upit += " AND r.idAdministrator = @idAdmin";
                    queryParams["@idAdmin"] = parametri.IdAdministrator;
                }
                upit += " ORDER BY r.idRacun DESC";

                return _broker.ExecuteQuery(new Racun(), upit, queryParams).Cast<Racun>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri pretrazi računa: {ex.Message}", ex);
            }
        }
    }
}
