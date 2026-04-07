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
    public class KreirajClanaOp:OperacijaBaze
    {
        private BrokerBP broker = new BrokerBP();

        protected override object DeserijalizujPodatke(object podaci)
            => JsonSerializer.Deserialize<Clan>((JsonElement)podaci);

        protected override object IzvrsiOperaciju(object podaci)
        {
            Clan clan = (Clan)podaci;

            string upitProvera = "SELECT COUNT(*) FROM Clan WHERE Email=@email OR BrojTelefona=@tel";
            var prov = new Dictionary<string, object>
            {
                { "@email", clan.Email },
                { "@tel", clan.BrojTelefona }
            };
            int count = Convert.ToInt32(broker.ExecuteScalar(upitProvera, prov));
            if (count > 0)
                throw new Exception("Clan sa ovim emailom ili telefonom vec postoji!");

            broker.Insert(clan);
            return null;
        }

        protected override string PorukaUspesno() => "Clan je uspesno dodat.";
    }
}
