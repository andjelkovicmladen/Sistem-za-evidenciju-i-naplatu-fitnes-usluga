using BrokerBazePodataka;
using Domen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Operacije
{
    public class KreirajRacunOp:OperacijaBaze
    {
        private BrokerBP broker = new BrokerBP();

        protected override object DeserijalizujPodatke(object podaci)
            => JsonSerializer.Deserialize<Racun>((JsonElement)podaci);

        protected override object IzvrsiOperaciju(object podaci)
        {
            Racun racun = (Racun)podaci;
            broker.BeginTransaction();
            try
            {
                int idRacun = broker.InsertWithIdentity(racun, "idRacun");
                int rb = 1;
                foreach (var stavka in racun.StavkeRacuna)
                {
                    stavka.IdRacun = idRacun;
                    stavka.Rb = rb++;
                    broker.Insert(stavka);
                }
                broker.Commit();
            }
            catch
            {
                broker.Rollback();
                throw;
            }
            finally
            {
                broker.CloseConnection();
            }
            return null;
        }

        protected override string PorukaUspesno() => "Racun je uspesno kreiran.";
    }
}
