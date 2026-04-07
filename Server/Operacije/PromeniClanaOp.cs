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
    public class PromeniClanaOp:OperacijaBaze
    {
        private BrokerBP broker = new BrokerBP();

        protected override object DeserijalizujPodatke(object podaci)
            => JsonSerializer.Deserialize<Clan>((JsonElement)podaci);

        protected override object IzvrsiOperaciju(object podaci)
        {
            Clan clan = (Clan)podaci;
            broker.Update(clan);
            return null;
        }

        protected override string PorukaUspesno() => "Clan je uspesno izmenjen.";
    }
}
