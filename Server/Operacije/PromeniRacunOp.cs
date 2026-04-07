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
    public class PromeniRacunOp:OperacijaBaze
    {
        private BrokerBP broker = new BrokerBP();

        protected override object DeserijalizujPodatke(object podaci)
            => JsonSerializer.Deserialize<Racun>((JsonElement)podaci);

        protected override object IzvrsiOperaciju(object podaci)
        {
            Racun racun = (Racun)podaci;
            broker.Update(racun);
            return null;
        }

        protected override string PorukaUspesno() => "Racun je uspesno izmenjen.";
    }
}

