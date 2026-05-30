using BrokerBazePodataka;
using Domen;
using Zajednicki;

namespace WebAPI.Services
{
    public interface ITerminService
    {
        void CreateTermin(TerminTreninga termin, int idAdministrator, string statusOpis);
    }

    public class TerminService : ITerminService
    {
        private readonly BrokerBP _broker;

        public TerminService()
        {
            _broker = new BrokerBP();
        }

        public void CreateTermin(TerminTreninga termin, int idAdministrator, string statusOpis)
        {
            if (termin == null)
                throw new ArgumentNullException(nameof(termin));

            try
            {
                termin.IdAdministrator = idAdministrator;
                termin.StatusOpis = statusOpis;

                // Ručni INSERT zbog naziva kolone idFitnessUsluga u bazi.
                string upit = @"INSERT INTO TerminTreninga (DatumVreme, TrajanjeMinuta, idFitnessUsluga, idAdministrator, StatusOpis)
                                VALUES (@DatumVreme, @TrajanjeMinuta, @idFitnessUsluga, @idAdministrator, @StatusOpis)";
                var parametri = new Dictionary<string, object>
                {
                    { "@DatumVreme", termin.DatumVreme },
                    { "@TrajanjeMinuta", termin.TrajanjeMinuta },
                    { "@idFitnessUsluga", termin.IdFitnesUsluga },
                    { "@idAdministrator", termin.IdAdministrator },
                    { "@StatusOpis", termin.StatusOpis ?? "Zakazan" }
                };
                _broker.ExecuteScalar(upit, parametri);
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri kreiranju termina: {ex.Message}", ex);
            }
        }
    }
}
