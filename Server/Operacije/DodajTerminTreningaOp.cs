using BrokerBazePodataka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Zajednicki;

namespace Server.Operacije
{
    public class DodajTerminTreningaOp:OperacijaBaze
    {
        private BrokerBP broker = new BrokerBP();

        protected override object DeserijalizujPodatke(object podaci)
            => JsonSerializer.Deserialize<DodajTerminPodaci>((JsonElement)podaci);

        protected override object IzvrsiOperaciju(object podaci)
        {
            DodajTerminPodaci dp = (DodajTerminPodaci)podaci;
            dp.Termin.IdAdministrator = dp.IdAdministrator;
            dp.Termin.StatusOpis = dp.StatusOpis;
            broker.Insert(dp.Termin);
            return null;
        }

        protected override string PorukaUspesno() => "Termin treninga je uspesno dodat.";
    }
}
