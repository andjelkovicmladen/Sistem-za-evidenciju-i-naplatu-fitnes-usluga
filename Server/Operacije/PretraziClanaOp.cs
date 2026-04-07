using BrokerBazePodataka;
using Domen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Zajednicki;

namespace Server.Operacije
{
    public class PretraziClanaOp:OperacijaBaze
    {
        private BrokerBP broker = new BrokerBP();

        protected override object DeserijalizujPodatke(object podaci)
            => JsonSerializer.Deserialize<ClanSearchParametri>((JsonElement)podaci);

        protected override object IzvrsiOperaciju(object podaci)
        {
            ClanSearchParametri k = (ClanSearchParametri)podaci;
            string upit = "SELECT * FROM Clan WHERE 1=1";
            var parametri = new Dictionary<string, object>();

            if (!string.IsNullOrEmpty(k.Ime))
            { upit += " AND Ime LIKE @ime"; parametri["@ime"] = "%" + k.Ime + "%"; }
            if (!string.IsNullOrEmpty(k.Prezime))
            { upit += " AND Prezime LIKE @prezime"; parametri["@prezime"] = "%" + k.Prezime + "%"; }
            if (!string.IsNullOrEmpty(k.Email))
            { upit += " AND Email LIKE @email"; parametri["@email"] = "%" + k.Email + "%"; }
            if (k.IdTipClanarine > 0)
            { upit += " AND idTipClanarine = @tip"; parametri["@tip"] = k.IdTipClanarine; }

            return broker.ExecuteQuery(new Clan(), upit, parametri).Cast<Clan>().ToList();
        }

        protected override string PorukaUspesno() => "Pretraga uspesna.";
    }
}
