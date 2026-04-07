using Domen;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text.Json;
using Zajednicki;

namespace KorisnickiInterfejs.UIKontroler
{
    internal class Kontroler
    {
        private static Kontroler instance;
        private Socket socket;
        private static readonly object lockObj = new object();
        private JsonNetworkSerializer serializer;

        private Kontroler() { }

        public static Kontroler Instance
        {
            get
            {
                if (instance == null)
                    instance = new Kontroler();
                return instance;
            }
        }

        public void Connect()
        {
            try
            {
                if (socket != null && socket.Connected) return;
                if (socket != null) { socket.Close(); socket = null; }
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect("127.0.0.1", 9999);
                serializer = new JsonNetworkSerializer(socket);
            }
            catch (Exception ex)
            {
                throw new Exception("Greska prilikom povezivanja na server! (" + ex.Message + ")");
            }
        }

        // ── AUTENTIFIKACIJA ────────────────────────────────
        public Administrator Login(Administrator admin)
        {
            Zahtev zahtev = new Zahtev
            {
                Operacija = Operacija.PrijaviAdministratora,
                Podaci = admin
            };
            serializer.Send(zahtev);
            Odgovor odgovor = serializer.Recieve<Odgovor>();
            if (odgovor.Signal == true)
                return JsonSerializer.Deserialize<Administrator>((JsonElement)odgovor.Podaci);
            else
                throw new Exception(odgovor.Poruka);
        }

        // ── CLANOVI ────────────────────────────────────────
        public void KreirajClana(Clan clan)
        {
            Zahtev zahtev = new Zahtev { Operacija = Operacija.KreirajClana, Podaci = clan };
            serializer.Send(zahtev);
            Odgovor odgovor = serializer.Recieve<Odgovor>();
            if (odgovor.Signal == false) throw new Exception(odgovor.Poruka);
        }

        public List<Clan> PretraziClana(string ime, string prezime, string email, int? tipId)
        {
            ClanSearchParametri parametriObj = new ClanSearchParametri()
            { Ime = ime, Prezime = prezime, Email = email, IdTipClanarine = tipId };
            Zahtev zahtev = new Zahtev { Operacija = Operacija.PretraziClana, Podaci = parametriObj };
            serializer.Send(zahtev);
            Odgovor odgovor = serializer.Recieve<Odgovor>();
            if (odgovor.Signal == true)
                return JsonSerializer.Deserialize<List<Clan>>((JsonElement)odgovor.Podaci);
            else
                throw new Exception(odgovor.Poruka);
        }

        public void PromeniClana(Clan clan)
        {
            Zahtev zahtev = new Zahtev { Operacija = Operacija.PromeniClana, Podaci = clan };
            serializer.Send(zahtev);
            Odgovor odgovor = serializer.Recieve<Odgovor>();
            if (odgovor.Signal == false) throw new Exception(odgovor.Poruka);
        }

        public void ObrisiClana(int idClan)
        {
            Zahtev zahtev = new Zahtev { Operacija = Operacija.ObrisiClana, Podaci = idClan };
            serializer.Send(zahtev);
            Odgovor odgovor = serializer.Recieve<Odgovor>();
            if (odgovor.Signal == false) throw new Exception(odgovor.Poruka);
        }

        // ── RACUNI ─────────────────────────────────────────
        public void KreirajRacun(Racun racun)
        {
            Zahtev zahtev = new Zahtev { Operacija = Operacija.KreirajRacun, Podaci = racun };
            serializer.Send(zahtev);
            Odgovor odgovor = serializer.Recieve<Odgovor>();
            if (odgovor.Signal == false) throw new Exception(odgovor.Poruka);
        }

        public List<Racun> PretraziRacun(DateTime? datumOd, DateTime? datumDo, int? idClan, int? idAdmin)
        {
            RacunSearchParametri parametri = new RacunSearchParametri()
            { DatumOd = datumOd, DatumDo = datumDo, IdClan = idClan, IdAdministrator = idAdmin };
            Zahtev zahtev = new Zahtev() { Operacija = Operacija.PretraziRacun, Podaci = parametri };
            serializer.Send(zahtev);
            Odgovor odgovor = serializer.Recieve<Odgovor>();
            if (odgovor.Signal == true)
                return JsonSerializer.Deserialize<List<Racun>>((JsonElement)odgovor.Podaci);
            else
                throw new Exception(odgovor.Poruka);
        }

        public void PromeniRacun(Racun racun)
        {
            Zahtev zahtev = new Zahtev { Operacija = Operacija.PromeniRacun, Podaci = racun };
            serializer.Send(zahtev);
            Odgovor odgovor = serializer.Recieve<Odgovor>();
            if (odgovor.Signal == false) throw new Exception(odgovor.Poruka);
        }

        // ── TERMIN TRENINGA ────────────────────────────────
        public void DodajTerminTreninga(TerminTreninga termin, int idAdmin, string statusOpis)
        {
            DodajTerminPodaci podaci = new DodajTerminPodaci
            { Termin = termin, IdAdministrator = idAdmin, StatusOpis = statusOpis };
            Zahtev zahtev = new Zahtev { Operacija = Operacija.DodajTerminTreninga, Podaci = podaci };
            serializer.Send(zahtev);
            Odgovor odgovor = serializer.Recieve<Odgovor>();
            if (odgovor.Signal == false) throw new Exception(odgovor.Poruka);
        }

        // ── POMOCNE METODE ─────────────────────────────────
        public Racun VratiRacunSaStavkama(int idRacun)
        {
            Zahtev zahtev = new Zahtev { Operacija = Operacija.VratiRacunSaStavkama, Podaci = idRacun };
            serializer.Send(zahtev);
            Odgovor odgovor = serializer.Recieve<Odgovor>();
            if (odgovor.Signal == true)
                return JsonSerializer.Deserialize<Racun>((JsonElement)odgovor.Podaci);
            else
                throw new Exception(odgovor.Poruka);
        }

        public List<Racun> VratiSveRacune()
        {
            Zahtev zahtev = new Zahtev { Operacija = Operacija.VratiSveRacune, Podaci = null };
            serializer.Send(zahtev);
            Odgovor odgovor = serializer.Recieve<Odgovor>();
            if (odgovor.Signal == true)
                return JsonSerializer.Deserialize<List<Racun>>((JsonElement)odgovor.Podaci);
            else
                throw new Exception(odgovor.Poruka);
        }

        public List<Clan> VratiSveClanove()
        {
            Zahtev zahtev = new Zahtev { Operacija = Operacija.VratiSveClanove, Podaci = null };
            serializer.Send(zahtev);
            Odgovor odgovor = serializer.Recieve<Odgovor>();
            if (odgovor.Signal == true)
                return JsonSerializer.Deserialize<List<Clan>>((JsonElement)odgovor.Podaci);
            else
                throw new Exception(odgovor.Poruka);
        }

        public List<TipClanarine> VratiSveTipoveClanarine()
        {
            Zahtev zahtev = new Zahtev { Operacija = Operacija.VratiSveTipoveClanarine, Podaci = null };
            serializer.Send(zahtev);
            Odgovor odgovor = serializer.Recieve<Odgovor>();
            if (odgovor.Signal == true)
                return JsonSerializer.Deserialize<List<TipClanarine>>((JsonElement)odgovor.Podaci);
            else
                throw new Exception(odgovor.Poruka);
        }

        public List<FitnesUsluga> VratiSveUsluge()
        {
            Zahtev zahtev = new Zahtev { Operacija = Operacija.VratiSveUsluge, Podaci = null };
            serializer.Send(zahtev);
            Odgovor odgovor = serializer.Recieve<Odgovor>();
            if (odgovor.Signal == true)
                return JsonSerializer.Deserialize<List<FitnesUsluga>>((JsonElement)odgovor.Podaci);
            else
                throw new Exception(odgovor.Poruka);
        }

        public void Disconnect()
        {
            if (socket != null && socket.Connected)
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Greska pri prekidu veze! (" + ex.Message + ")");
                }
                finally
                {
                    socket = null;
                    instance = null;
                }
            }
            else
            {
                throw new Exception("Niste povezani na server!");
            }
        }
    }
}