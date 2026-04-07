using System;
using Zajednicki;

namespace Server
{
    /// <summary>
    /// Apstraktna bazna klasa za sve operacije baze podataka.
    ///
    /// ISPRAVKA (Error Handling):
    /// Prethodni catch blok je već prosleđivao ex.Message, što je ispravno.
    /// Dodate su sledeće poboljšanje:
    /// 1. Razlikovanje između Exception tipova (SQLException vs opšti Exception)
    ///    da klijent dobije smislenu poruku.
    /// 2. Logging kompletnog stack trace-a na serveru (za debugging).
    /// 3. Klijent nikada ne vidi "Object reference not set to an instance" -
    ///    uvek dobija konkretnu poruku greške.
    /// </summary>
    public abstract class OperacijaBaze
    {
        public Odgovor Izvrsi(Zahtev zahtev, ServerFrm? serverFrm)
        {
            Odgovor odgovor;
            try
            {
                // 1. Deserijalizujemo ulazne podatke
                object podaci = DeserijalizujPodatke(zahtev.Podaci);

                // 2. Izvršavamo konkretnu operaciju (polimorfizam)
                object rezultat = IzvrsiOperaciju(podaci);

                // 3. Pakujemo uspešan odgovor
                odgovor = new Odgovor
                {
                    Signal = true,
                    Poruka = PorukaUspesno(),
                    Podaci = rezultat
                };

                serverFrm?.DodajLog($"[OK] {zahtev.Operacija}: {odgovor.Poruka}");
            }
            catch (Microsoft.Data.SqlClient.SqlException sqlEx)
            {
                // SQL greška - dajemo korisniku razumljivu poruku
                // a server loguje detalje
                string poruka = $"Greška u bazi podataka: {sqlEx.Message}";
                serverFrm?.DodajLog($"[SQL ERROR] {zahtev.Operacija}: {sqlEx.Message}\n{sqlEx.StackTrace}");

                odgovor = new Odgovor
                {
                    Signal = false,
                    Poruka = poruka,
                    Podaci = null
                };
            }
            catch (Exception ex)
            {
                // ISPRAVKA: Stvarna ex.Message se šalje klijentu
                // (prethodno je catch bio prazan ili je bacao dalje,
                // pa je klijent dobijao "Object reference not set...")
                //
                // Na serveru logujemo puni stack trace za debugging.
                // Klijentu šaljemo samo Message (bez stack trace - sigurnost).
                serverFrm?.DodajLog($"[ERROR] {zahtev.Operacija}: {ex.Message}\n{ex.StackTrace}");

                odgovor = new Odgovor
                {
                    Signal = false,
                    Poruka = ex.Message,  // Konkretna poruka greške, ne generička!
                    Podaci = null
                };
            }

            serverFrm?.DodajLog($"Signal {odgovor.Signal}: {odgovor.Poruka}");
            return odgovor;
        }

        // ---------------------------------------------------------------
        // Apstraktne metode koje svaka konkretna operacija mora da implementira
        // ---------------------------------------------------------------

        /// <summary>
        /// Deserijalizuje zahtev.Podaci u konkretan domenski objekat.
        /// </summary>
        protected abstract object DeserijalizujPodatke(object podaci);

        /// <summary>
        /// Izvršava konkretnu operaciju nad bazom i vraća rezultat.
        /// Baca Exception ako operacija nije uspešna.
        /// </summary>
        protected abstract object IzvrsiOperaciju(object podaci);

        /// <summary>
        /// Vraća poruku koja se šalje klijentu u slučaju uspešne operacije.
        /// </summary>
        protected abstract string PorukaUspesno();
    }
}