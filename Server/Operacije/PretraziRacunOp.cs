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
    public class PretraziRacunOp:OperacijaBaze
    {
        
            private BrokerBP broker = new BrokerBP();

            protected override object DeserijalizujPodatke(object podaci)
                => JsonSerializer.Deserialize<RacunSearchParametri>((JsonElement)podaci);

            protected override object IzvrsiOperaciju(object podaci)
            {
                RacunSearchParametri k = (RacunSearchParametri)podaci;

                string upit = "SELECT r.*, a.Ime+' '+a.Prezime AS Administrator, " +
                    "c.Ime+' '+c.Prezime AS Clan, ISNULL(SUM(sr.Iznos),0) AS UkupanIznos " +
                    "FROM Racun r " +
                    "JOIN Administrator a ON r.idAdministrator=a.idAdministrator " +
                    "JOIN Clan c ON r.idClan=c.idClan " +
                    "LEFT JOIN StavkaRacuna sr ON r.idRacun=sr.idRacun " +
                    "WHERE 1=1";

                var parametri = new Dictionary<string, object>();

                if (k.DatumOd.HasValue)
                { upit += " AND r.DatumIzdavanja >= @datumOd"; parametri["@datumOd"] = k.DatumOd.Value; }
                if (k.DatumDo.HasValue)
                { upit += " AND r.DatumIzdavanja <= @datumDo"; parametri["@datumDo"] = k.DatumDo.Value; }
                if (k.IdClan.HasValue)
                { upit += " AND r.idClan = @idClan"; parametri["@idClan"] = k.IdClan.Value; }
                if (k.IdAdministrator.HasValue)
                { upit += " AND r.idAdministrator = @idAdmin"; parametri["@idAdmin"] = k.IdAdministrator.Value; }

                upit += " GROUP BY r.idRacun, r.DatumIzdavanja, r.DatumDospeca, " +
                    "r.idAdministrator, r.idClan, a.Ime, a.Prezime, c.Ime, c.Prezime";

                return broker.ExecuteQuery(new Racun(), upit, parametri).Cast<Racun>().ToList();
            }

            protected override string PorukaUspesno() => "Pretraga racuna uspesna.";
        }
}
