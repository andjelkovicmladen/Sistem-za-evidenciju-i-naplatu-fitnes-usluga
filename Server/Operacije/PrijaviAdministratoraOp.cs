using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text.Json;
using BrokerBazePodataka;
using Domen;

namespace Server.Operacije
{
    /// <summary>
    /// Operacija prijave administratora.
    ///
    /// ISPRAVKE:
    /// 1. BrokerBP se više ne kreira kao polje instance (new BrokerBP() po pozivu),
    ///    nego kao statički shared objekat — konzistentno sa Singleton pattern-om
    ///    koji koristi Broker.Instance ispod.
    /// 2. Dodata validacija ulaznih podataka pre upita na bazu.
    /// 3. Poboljšane poruke grešaka da klijent tačno zna šta je pošlo naopako.
    /// 4. Provera "već prijavljen" ostaje ispravna.
    /// </summary>
    public class PrijaviAdministratoraOp : OperacijaBaze
    {
        // ISPRAVKA: Jedan deljeni BrokerBP koji interno koristi Broker.Instance.
        // Stari kod je kreirao new BrokerBP() kao polje, što je potencijalno
        // kreiralo novu instancu pri svakom Handle pozivu — opasno u višenitnom okruženju.
        private static readonly BrokerBP broker = new BrokerBP();

        private readonly Server server;
        private readonly Socket klijentskiSocket;

        public PrijaviAdministratoraOp(Server server, Socket socket)
        {
            this.server = server;
            this.klijentskiSocket = socket;
        }

        protected override object DeserijalizujPodatke(object podaci)
        {
            // Robusna deserijalizacija sa boljom porukom greške
            if (podaci is JsonElement jsonEl)
            {
                var admin = JsonSerializer.Deserialize<Administrator>(jsonEl);
                if (admin == null)
                    throw new Exception("Deserijalizacija administratora vratila je null. Proverite format podataka.");
                return admin;
            }

            throw new Exception($"Neočekivani tip podataka: {podaci?.GetType().Name ?? "null"}. Očekivan JsonElement.");
        }

        protected override object IzvrsiOperaciju(object podaci)
        {
            Administrator admin = (Administrator)podaci;

            // 1. Validacija ulaznih podataka
            if (string.IsNullOrWhiteSpace(admin.Email))
                throw new Exception("Email adresa je obavezna.");
            if (string.IsNullOrWhiteSpace(admin.Password))
                throw new Exception("Lozinka je obavezna.");

            // 2. Proveravamo da li je administrator već prijavljen
            //    (ovo koristi IdAdministrator iz zahteva - može biti 0 ako nije poznat unapred,
            //     pa proveravamo po Email-u nakon što pronađemo admina u bazi)
            string upit = "SELECT * FROM Administrator WHERE Email=@email AND Password=@pw";
            var parametri = new Dictionary<string, object>
            {
                { "@email", admin.Email },
                { "@pw", admin.Password }
            };

            // 3. Izvršavamo upit — sada koristi ispravljeni BrokerBP koji konzistentno
            //    koristi Broker.Instance za konekciju
            List<IEntity> rezultat = broker.ExecuteQuery(new Administrator(), upit, parametri);

            if (rezultat.Count == 0)
                throw new Exception("Pogrešni kredencijali. Proverite email i lozinku.");

            Administrator prijavljeni = (Administrator)rezultat[0];

            // 4. Proveravamo da li je već prijavljen (sada sa stvarnim ID-em iz baze)
            if (server.DaliJeKorisnikPrijavljen(prijavljeni.IdAdministrator))
                throw new Exception("Administrator je već prijavljen na drugom računaru!");

            // 5. Registrujemo prijavu
            server.DodajPrijavljenogKorisnika(prijavljeni.IdAdministrator, klijentskiSocket);

            return prijavljeni;
        }

        protected override string PorukaUspesno() => "Uspešna prijava!";
    }
}