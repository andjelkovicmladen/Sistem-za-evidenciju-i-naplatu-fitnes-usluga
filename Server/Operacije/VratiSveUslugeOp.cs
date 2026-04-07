using BrokerBazePodataka;
using Domen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Operacije
{
    public class VratiSveUslugeOp:OperacijaBaze
    {
        private BrokerBP broker = new BrokerBP();

        protected override object DeserijalizujPodatke(object podaci) => null;

        protected override object IzvrsiOperaciju(object podaci)
        => broker.GetAll(new FitnesUsluga()).Cast<FitnesUsluga>().ToList();

        protected override string PorukaUspesno() => "Usluge vracene.";
    }
}
