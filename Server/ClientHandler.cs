using System;
using System.Net.Sockets;
using BrokerBazePodataka;
using Domen;
using Zajednicki;
using Server.Operacije;

namespace Server
{
    /// <summary>
    /// Rukuje komunikacijom sa jednim klijentom u zasebnoj niti.
    ///
    /// ISPRAVKE:
    /// 1. Prazan catch{} zamenjen logovanim catch-om — greška se više ne guta
    ///    bez traga, što je otežavalo debugging.
    /// 2. Dodata provera null za 'operacija' pre poziva Izvrsi().
    /// 3. Sve konekcije se čiste u finally bloku.
    /// </summary>
    public class ClientHandler
    {
        private readonly Socket klijentskiSocket;
        private readonly ServerFrm? serverFrm;
        private readonly Server server;

        public ClientHandler(Socket klijentskiSocket, ServerFrm? serverFrm, Server server)
        {
            this.klijentskiSocket = klijentskiSocket;
            this.serverFrm = serverFrm;
            this.server = server;
        }

        public void Handle()
        {
            serverFrm?.DodajLog($"Klijent povezan: {klijentskiSocket.RemoteEndPoint}");
            JsonNetworkSerializer serializer = new JsonNetworkSerializer(klijentskiSocket);

            try
            {
                while (true)
                {
                    // 1. Primamo zahtev od klijenta
                    Zahtev? zahtev = serializer.Recieve<Zahtev>();
                    if (zahtev == null)
                    {
                        serverFrm?.DodajLog("Klijent zatvorio konekciju (null zahtev).");
                        break;
                    }

                    serverFrm?.DodajLog($"Stigao zahtev: {zahtev.Operacija}");

                    // 2. Kreiramo odgovarajuću operaciju
                    OperacijaBaze? operacija = KreirajOperaciju(zahtev.Operacija);

                    Odgovor odgovor;
                    if (operacija != null)
                    {
                        // 3. Izvršavamo operaciju — Izvrsi() interno hvata sve greške
                        //    i vraća Odgovor sa Signal=false i porukom greške.
                        //    Klijent UVEK dobija odgovor, nikada ne ostaje "visiti".
                        odgovor = operacija.Izvrsi(zahtev, serverFrm);
                    }
                    else
                    {
                        odgovor = new Odgovor
                        {
                            Signal = false,
                            Poruka = $"Nepoznata operacija: {zahtev.Operacija}",
                            Podaci = null
                        };
                        serverFrm?.DodajLog($"[WARN] Nepoznata operacija: {zahtev.Operacija}");
                    }

                    // 4. Šaljemo odgovor klijentu
                    serializer.Send(odgovor);
                }
            }
            catch (Exception ex)
            {
                // ISPRAVKA: Prazan catch{} je bio ovde — greške su se gutale.
                // Sada logujemo razlog prekida konekcije.
                // Ne šaljemo Odgovor jer konekcija možda više nije upotrebljiva.
                serverFrm?.DodajLog($"[ERROR] Greška u komunikaciji sa klijentom: {ex.Message}");
            }
            finally
            {
                OcistiKonekciju();
            }
        }

        private void OcistiKonekciju()
        {
            try
            {
                server?.UkloniPrijavljenogKorisnika(klijentskiSocket);
                klijentskiSocket?.Shutdown(SocketShutdown.Both);
                klijentskiSocket?.Close();
                server?.UkloniSocket(klijentskiSocket);
                serverFrm?.DodajLog("Klijent diskonektovan i resursi oslobođeni.");
            }
            catch (Exception ex)
            {
                // Greška pri čišćenju — logujemo ali ne bacamo dalje
                serverFrm?.DodajLog($"[WARN] Greška pri čišćenju socket-a: {ex.Message}");
            }
        }

        private OperacijaBaze? KreirajOperaciju(Operacija op)
        {
            switch (op)
            {
                case Operacija.PrijaviAdministratora:
                    return new PrijaviAdministratoraOp(server, klijentskiSocket);
                case Operacija.KreirajClana:
                    return new KreirajClanaOp();
                case Operacija.PretraziClana:
                    return new PretraziClanaOp();
                case Operacija.PromeniClana:
                    return new PromeniClanaOp();
                case Operacija.ObrisiClana:
                    return new ObrisiClanaOp();
                case Operacija.KreirajRacun:
                    return new KreirajRacunOp();
                case Operacija.PretraziRacun:
                    return new PretraziRacunOp();
                case Operacija.PromeniRacun:
                    return new PromeniRacunOp();
                case Operacija.VratiSveRacune:
                    return new VratiSveRacuneOp();
                case Operacija.DodajTerminTreninga:
                    return new DodajTerminTreningaOp();
                case Operacija.VratiSveTipoveClanarine:
                    return new VratiSveTipoveClanarineOp();
                case Operacija.VratiSveClanove:
                    return new VratiSveClanoveOp();
                case Operacija.VratiSveUsluge:
                    return new VratiSveUslugeOp();
                case Operacija.VratiRacunSaStavkama:
                    return new VratiRacunSaStavkamaOp();
                default:
                    return null;
            }
        }
    }
}