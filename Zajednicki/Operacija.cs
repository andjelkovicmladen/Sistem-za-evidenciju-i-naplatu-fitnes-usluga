using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zajednicki
{
    public enum Operacija
    {
        // Autentifikacija
        PrijaviAdministratora,

        // Upravljanje clanovima
        KreirajClana,
        PretraziClana,
        PromeniClana,
        ObrisiClana,

        // Upravljanje racunima
        KreirajRacun,
        PretraziRacun,
        PromeniRacun,

        // Termini treninga
        DodajTerminTreninga,

        // Pomocne operacije
        VratiSveTipoveClanarine,
        VratiSveClanove,
        VratiSveUsluge,
        VratiSveRacune,
        VratiRacunSaStavkama,
    }
}
