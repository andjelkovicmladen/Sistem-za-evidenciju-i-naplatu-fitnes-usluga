using BrokerBazePodataka;
using Domen;

namespace WebAPI.Services
{
    public interface ILookupService
    {
        List<TipClanarine> GetAllTipoviClanarina();
        List<FitnesUsluga> GetAllUsluge();
    }

    public class LookupService : ILookupService
    {
        private readonly BrokerBP _broker;

        public LookupService()
        {
            _broker = new BrokerBP();
        }

        public List<TipClanarine> GetAllTipoviClanarina()
        {
            try
            {
                return _broker.GetAll(new TipClanarine()).Cast<TipClanarine>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri preuzimanju tipova članarina: {ex.Message}", ex);
            }
        }

        public List<FitnesUsluga> GetAllUsluge()
        {
            try
            {
                // Tabela u bazi se zove "FitnessUsluga" (idFitnessUsluga); mapiramo na domen aliasima.
                string upit = "SELECT idFitnessUsluga AS idFitnesUsluga, Naziv, TipUsluge, CenaPoSatu, MaxKapacitet FROM FitnessUsluga";
                return _broker.ExecuteQuery(new FitnesUsluga(), upit).Cast<FitnesUsluga>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri preuzimanju fitnes usluga: {ex.Message}", ex);
            }
        }
    }
}
