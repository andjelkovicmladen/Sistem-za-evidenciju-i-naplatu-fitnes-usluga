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
    public class ObrisiClanaOp:OperacijaBaze
    {
        private BrokerBP broker = new BrokerBP();

        protected override object DeserijalizujPodatke(object podaci)
            => JsonSerializer.Deserialize<int>((JsonElement)podaci);

        protected override object IzvrsiOperaciju(object podaci)
        {
            int idClan = (int)podaci;
            broker.Delete(new Clan { IdClan = idClan });
            return null;
        }

        protected override string PorukaUspesno() => "Clan je uspesno obrisan.";
    }
}
