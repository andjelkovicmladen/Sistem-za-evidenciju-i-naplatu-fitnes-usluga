using BrokerBazePodataka;
using Domen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Operacije
{
    public class VratiSveClanoveOp:OperacijaBaze
    {
        private BrokerBP broker = new BrokerBP();

        protected override object DeserijalizujPodatke(object podaci) => null;

        protected override object IzvrsiOperaciju(object podaci)
            => broker.GetAll(new Clan()).Cast<Clan>().ToList();

        protected override string PorukaUspesno() => "Lista clanova vracena.";
    }
}
