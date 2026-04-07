using BrokerBazePodataka;
using Domen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Operacije
{
    internal class VratiSveRacuneOp:OperacijaBaze
    {
        private BrokerBP broker = new BrokerBP();

        protected override object DeserijalizujPodatke(object podaci) => null;

        protected override object IzvrsiOperaciju(object podaci)
        {
            string upit = "SELECT r.*, a.Ime+' '+a.Prezime AS Administrator, " +
                "c.Ime+' '+c.Prezime AS Clan, ISNULL(SUM(sr.Iznos),0) AS UkupanIznos " +
                "FROM Racun r " +
                "JOIN Administrator a ON r.idAdministrator=a.idAdministrator " +
                "JOIN Clan c ON r.idClan=c.idClan " +
                "LEFT JOIN StavkaRacuna sr ON r.idRacun=sr.idRacun " +
                "GROUP BY r.idRacun, r.DatumIzdavanja, r.DatumDospeca, " +
                "r.idAdministrator, r.idClan, a.Ime, a.Prezime, c.Ime, c.Prezime";

            return broker.ExecuteQuery(new Racun(), upit).Cast<Racun>().ToList();
        }

        protected override string PorukaUspesno() => "Lista racuna vracena.";
    }
}
