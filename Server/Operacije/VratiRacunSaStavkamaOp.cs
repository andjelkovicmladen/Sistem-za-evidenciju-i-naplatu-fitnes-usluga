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
    public class VratiRacunSaStavkamaOp:OperacijaBaze
    {
        private BrokerBP broker = new BrokerBP();

        protected override object DeserijalizujPodatke(object podaci)
            => JsonSerializer.Deserialize<int>((JsonElement)podaci);

        protected override object IzvrsiOperaciju(object podaci)
        {
            int idRacun = (int)podaci;

            string qRacun = "SELECT r.*, a.Ime+' '+a.Prezime AS Administrator, " +
                "c.Ime+' '+c.Prezime AS Clan, 0 AS UkupanIznos " +
                "FROM Racun r " +
                "JOIN Administrator a ON r.idAdministrator=a.idAdministrator " +
                "JOIN Clan c ON r.idClan=c.idClan " +
                "WHERE r.idRacun=@id";

            var p = new Dictionary<string, object> { { "@id", idRacun } };
            var lista = broker.ExecuteQuery(new Racun(), qRacun, p);
            if (lista.Count == 0)
                throw new Exception("Racun nije nadjen.");

            Racun racun = (Racun)lista[0];

            string qStavke = "SELECT sr.*, fu.Naziv, fu.CenaPoSatu FROM StavkaRacuna sr " +
                "JOIN FitnesUsluga fu ON sr.idFitnesUsluga=fu.idFitnesUsluga " +
                "WHERE sr.idRacun=@id";

            var stavke = broker.ExecuteQuery(new StavkaRacuna(), qStavke, p);
            racun.StavkeRacuna = stavke.Cast<StavkaRacuna>().ToList();
            return racun;
        }

        protected override string PorukaUspesno() => "Racun sa stavkama vrацен.";
    }
}
