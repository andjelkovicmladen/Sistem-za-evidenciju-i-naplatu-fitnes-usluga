using BrokerBazePodataka;
using Domen;
using Zajednicki;

namespace WebAPI.Services
{
    public interface IClanService
    {
        List<Clan> GetAllClans();
        Clan GetClanById(int id);
        void CreateClan(Clan clan);
        void UpdateClan(Clan clan);
        void DeleteClan(int id);
        List<Clan> SearchClans(ClanSearchParametri parametri);
    }

    public class ClanService : IClanService
    {
        private readonly BrokerBP _broker;

        public ClanService()
        {
            _broker = new BrokerBP();
        }

        public List<Clan> GetAllClans()
        {
            try
            {
                return _broker.GetAll(new Clan()).Cast<Clan>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri preuzimanju svih članova: {ex.Message}", ex);
            }
        }

        public Clan GetClanById(int id)
        {
            try
            {
                string upit = "SELECT * FROM Clan WHERE idClan=@id";
                var parametri = new Dictionary<string, object> { { "@id", id } };
                var rezultat = _broker.ExecuteQuery(new Clan(), upit, parametri);

                return rezultat.Count > 0 ? (Clan)rezultat[0] : null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri preuzimanju člana: {ex.Message}", ex);
            }
        }

        public void CreateClan(Clan clan)
        {
            if (clan == null)
                throw new ArgumentNullException(nameof(clan));

            try
            {
                string upitProvera = "SELECT COUNT(*) FROM Clan WHERE Email=@email OR BrojTelefona=@tel";
                var prov = new Dictionary<string, object>
                {
                    { "@email", clan.Email },
                    { "@tel", clan.BrojTelefona }
                };
                int count = Convert.ToInt32(_broker.ExecuteScalar(upitProvera, prov));
                if (count > 0)
                    throw new Exception("Član sa ovim emailom ili telefonom već postoji!");

                _broker.Insert(clan);
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri kreiranju člana: {ex.Message}", ex);
            }
        }

        public void UpdateClan(Clan clan)
        {
            if (clan == null)
                throw new ArgumentNullException(nameof(clan));

            try
            {
                _broker.Update(clan);
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri ažuriranju člana: {ex.Message}", ex);
            }
        }

        public void DeleteClan(int id)
        {
            try
            {
                var clan = new Clan { IdClan = id };
                _broker.Delete(clan);
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri brisanju člana: {ex.Message}", ex);
            }
        }

        public List<Clan> SearchClans(ClanSearchParametri parametri)
        {
            if (parametri == null)
                throw new ArgumentNullException(nameof(parametri));

            try
            {
                string upit = "SELECT * FROM Clan WHERE 1=1";
                var queryParams = new Dictionary<string, object>();

                if (!string.IsNullOrEmpty(parametri.Ime))
                {
                    upit += " AND Ime LIKE @ime";
                    queryParams["@ime"] = "%" + parametri.Ime + "%";
                }
                if (!string.IsNullOrEmpty(parametri.Prezime))
                {
                    upit += " AND Prezime LIKE @prezime";
                    queryParams["@prezime"] = "%" + parametri.Prezime + "%";
                }
                if (!string.IsNullOrEmpty(parametri.Email))
                {
                    upit += " AND Email LIKE @email";
                    queryParams["@email"] = "%" + parametri.Email + "%";
                }
                if (parametri.IdTipClanarine > 0)
                {
                    upit += " AND idTipClanarine = @tip";
                    queryParams["@tip"] = parametri.IdTipClanarine;
                }

                return _broker.ExecuteQuery(new Clan(), upit, queryParams).Cast<Clan>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri pretrazi članova: {ex.Message}", ex);
            }
        }
    }
}
