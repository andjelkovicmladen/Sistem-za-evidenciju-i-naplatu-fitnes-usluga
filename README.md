# 🏋️ Sistem za Evidenciju i Naplatu Fitnes Usluga

Studentski projekat — **full-stack** sistem za upravljanje fitnes centrom. Originalno desktop aplikacija u **C# / .NET 8** (Windows Forms + TCP server), proširena **REST API-jem** i modernim **React + TypeScript** frontendom, sa **deploy-em u Azure i Vercel**.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet) ![React](https://img.shields.io/badge/React-19-61DAFB?logo=react) ![TypeScript](https://img.shields.io/badge/TypeScript-5-3178C6?logo=typescript) ![Vite](https://img.shields.io/badge/Vite-8-646CFF?logo=vite) ![Azure](https://img.shields.io/badge/Azure-App%20Service%20%2B%20SQL-0078D4?logo=microsoftazure) ![Vercel](https://img.shields.io/badge/Vercel-deployed-000?logo=vercel) ![SQL Server](https://img.shields.io/badge/SQL%20Server-LocalDB%20%2F%20Azure-CC2927?logo=microsoftsqlserver)

---

## 🌐 Live demo

| Sloj | Link |
|---|---|
| 🎨 **Web aplikacija** (frontend) | https://sistem-za-evidenciju-i-naplatu-fitn.vercel.app |
| 🏗️ **API (Swagger)** | https://fitnes-api-w80565.azurewebsites.net/ |
| 💻 **Repozitorijum** | https://github.com/andjelkovicmladen/Sistem-za-evidenciju-i-naplatu-fitnes-usluga |

**Demo nalog:**
- Email: `marko@fitness.rs`
- Lozinka: `admin123`

> ⚠️ Backend je na Azure F1 *free tier*-u — prvi zahtev nakon mirovanja zna da potraje ~30s (cold start).

---

## 📋 O projektu

Sistem omogućava administratorima fitnes centra da upravljaju **članovima, fitnes uslugama, računima i terminima treninga**. Projekat ima **tri ulazna kanala** ka istoj poslovnoj logici i bazi:

1. 🖥️ **Desktop (WinForms)** — originalna klijent-server aplikacija preko **TCP soketa** sa **JSON serijalizacijom**.
2. 🔌 **REST API (WebAPI)** — ASP.NET Core 8 sa **JWT autentikacijom** i **Swagger** dokumentacijom.
3. 🌐 **Web aplikacija (React)** — Single Page Application koja konzumira REST API.

Sva tri sloja dele iste **`Domen`** klase i **`BrokerBazePodataka`** sloj za pristup bazi, što obezbeđuje konzistentnost poslovne logike.

---

## 🛠️ Tehnologije

### Backend
| Tehnologija | Verzija |
|---|---|
| C# / .NET | 8.0 |
| ASP.NET Core WebAPI | 8.0 |
| Windows Forms | net8.0-windows |
| Microsoft.Data.SqlClient | 6.x |
| JWT Bearer Authentication | 7.4 |
| Swashbuckle (Swagger) | 6.5 |

### Frontend
| Tehnologija | Verzija |
|---|---|
| React | 19 |
| TypeScript | 5.x |
| Vite | 8 |
| React Router | 7 |
| Axios | 1.16 |

### Baza i infrastruktura
| Komponenta | Lokalno | Produkcija |
|---|---|---|
| Baza | SQL Server LocalDB | **Azure SQL Database** (Basic) |
| Backend host | Kestrel | **Azure App Service** (Windows F1) |
| Frontend host | Vite dev server | **Vercel** |
| Region | localhost | France Central |

---

## 🏗️ Arhitektura

```
                       ┌──────────────────────┐
                       │   AZURE SQL DATABASE │   ← jedan magacin podataka
                       │  (fitnesdb / Basic)  │
                       └──────────┬───────────┘
                                  │ ADO.NET
                       ┌──────────▼───────────┐
                       │  BrokerBazePodataka  │   ← jedini sloj koji dira bazu
                       └──────────┬───────────┘
                                  │
                       ┌──────────▼───────────┐
                       │   Domen + Services   │   ← poslovna logika
                       └──┬─────────┬─────────┘
                          │         │
              ┌───────────▼┐       ┌▼──────────────────┐
              │ TCP Server │       │  ASP.NET WebAPI   │
              │ (port 9999)│       │  (REST + JWT)     │
              └─────┬──────┘       └─────┬─────────────┘
                    │                    │ HTTPS / JSON
              ┌─────▼──────┐       ┌─────▼─────────────┐
              │  WinForms  │       │  React (Vite SPA) │
              │  klijent   │       │  na Vercel-u      │
              └────────────┘       └───────────────────┘
```

---

## ⚙️ Funkcionalnosti

### 👤 Članovi
Kreiranje / izmena / brisanje / pretraga (po imenu, prezimenu, email-u, tipu članarine), validacija jedinstvenog email-a i telefona.

### 🧾 Računi
Kreiranje računa sa više stavki (transakciono, sa automatskim rb), pretraga, detalji sa stavkama i sumom, prikaz člana/administratora/iznosa.

### 🗓️ Termini treninga
Zakazivanje termina za fitnes uslugu, sa trajanjem i statusom.

### 🔐 Autentikacija
JWT token (24h), čuva se u `localStorage` na frontendu, axios interceptor automatski dodaje `Authorization: Bearer …` header.

### 📊 Kontrolna tabla (web)
Brzi pregled — broj članova, broj računa, ukupan prihod, lista poslednjih računa.

---

## 📁 Struktura projekta

```
Sistem-za-evidenciju-i-naplatu-fitnes-usluga/
│
├── Domen/                     # Domenski entiteti (Clan, Racun, Administrator…)
├── Zajednicki/                # Zajednički sloj (Zahtev, Odgovor, Operacija, Serializer)
├── BrokerBazePodataka/        # Pristup bazi (Broker + BrokerBP)
├── Server/                    # TCP server (port 9999)
│   └── Operacije/             # Operacije nad bazom
├── KorisnickiInterfejs/       # WinForms klijent
│   └── UIKontroler/           # TCP komunikacija
│
├── WebAPI/                    # ASP.NET Core REST API
│   ├── Controllers/           # AuthController, ClanoviController, RacuniController, TerminiController, LookupController
│   └── Services/              # AuthService, ClanService, RacunService, TerminService, LookupService
│
├── frontend/                  # React + Vite + TypeScript SPA
│   ├── src/
│   │   ├── api/               # axios klijent + endpoint moduli
│   │   ├── components/        # Layout, Modal, Toast, ProtectedRoute
│   │   ├── context/           # AuthContext (JWT)
│   │   ├── pages/             # Login, Dashboard, Clanovi, Racuni, Termini
│   │   └── utils/             # format (RSD, datum)
│   ├── vite.config.ts         # Vite + dev proxy ka backendu
│   └── vercel.json            # SPA rewrites za Vercel
│
├── migrations/
│   └── azure-init.sql         # Šema + view + seed za Azure SQL
│
└── Sistem-za-evidenciju-i-naplatu-fitnes-usluga.sln
```

---

## 🚀 Pokretanje lokalno

### Preduslovi
- **.NET 8 SDK** ([download](https://dotnet.microsoft.com/download))
- **Node.js 20+** (samo za frontend) ([download](https://nodejs.org))
- **SQL Server LocalDB** (dolazi uz Visual Studio ili kao standalone)
- **Visual Studio 2022/2026** (opciono — preporuka za WinForms i debug)

### 1️⃣ Kloniraj repo

```bash
git clone https://github.com/andjelkovicmladen/Sistem-za-evidenciju-i-naplatu-fitnes-usluga.git
cd Sistem-za-evidenciju-i-naplatu-fitnes-usluga
```

### 2️⃣ Postavi bazu

Bazu možeš da kreiraš:

**Opcija A — sa Azure migracijskom skriptom (najbrže):**
Otvori SSMS / Azure Data Studio, poveži se na `(localdb)\MSSQLLocalDB`, kreiraj bazu `DB`, pa pokreni:
```
migrations/azure-init.sql
```

**Opcija B — ručno** preko skripte iz `Database/` foldera (ako koristiš originalnu šemu).

### 3️⃣ Backend (WebAPI)

```bash
dotnet run --project WebAPI --launch-profile http
```

Otvara se na **http://localhost:5000** sa Swagger UI-jem na korenu.

### 4️⃣ Frontend (React)

```bash
cd frontend
npm install
npm run dev
```

Otvara se na **http://localhost:5173**. Vite proxy automatski prosleđuje `/api` na backend (`localhost:5000`).

### 5️⃣ (Opciono) Desktop varijanta

U Visual Studio postavi **Multiple Startup Projects**: `Server` + `KorisnickiInterfejs`. Pokreni — server sluša na portu 9999, otvori se WinForms klijent.

### 🔑 Demo nalog
- Email: `marko@fitness.rs`
- Lozinka: `admin123`

---

## ☁️ Cloud arhitektura (produkcija)

| Resurs | Servis | Tier | Region |
|---|---|---|---|
| Baza | Azure SQL Database | Basic 5 DTU, 2 GB | France Central |
| Backend | Azure App Service | F1 (free), Windows | France Central |
| Frontend | Vercel | Hobby (free) | edge global |
| CI/CD | GitHub → Vercel auto-deploy | — | — |

**Konekcioni string** za backend se prosleđuje kao Azure App Service env var **`FITNES_CONN_STR`** (čita ga `Broker.cs`, sa fallbackom na LocalDB lokalno).

**Frontend** koristi env var **`VITE_API_URL`** (na Vercel-u podešen na URL Azure App Service-a). Lokalno fallback je `/api` preko Vite proxy-ja.

---

## 🔌 REST API — pregled endpointa

| Metod | Putanja | Opis | Auth |
|---|---|---|---|
| `POST` | `/api/auth/login` | Prijava administratora, vraća JWT token | ❌ |
| `GET` | `/api/clanovi` | Lista svih članova | ✅ |
| `GET` | `/api/clanovi/{id}` | Detalji člana | ✅ |
| `POST` | `/api/clanovi` | Kreiranje člana | ✅ |
| `PUT` | `/api/clanovi/{id}` | Izmena člana | ✅ |
| `DELETE` | `/api/clanovi/{id}` | Brisanje člana | ✅ |
| `POST` | `/api/clanovi/search` | Pretraga članova | ✅ |
| `GET` | `/api/racuni` | Svi računi | ✅ |
| `GET` | `/api/racuni/{id}/stavke` | Račun sa stavkama | ✅ |
| `POST` | `/api/racuni` | Kreiranje računa sa stavkama (transakciono) | ✅ |
| `POST` | `/api/termini?statusOpis=Zakazan` | Zakazivanje termina | ✅ |
| `GET` | `/api/usluge` | Lista fitnes usluga | ✅ |
| `GET` | `/api/tipovi-clanarina` | Lista tipova članarine | ✅ |

Detaljno u Swagger UI: https://fitnes-api-w80565.azurewebsites.net/

---

## 👤 Autor

- **Mladen Anđelković** — *student*
- Fakultet organizacionih nauka, Beograd
- Predmet: Projektovanje softvera

---

## 📄 Licenca

Projekat izrađen u obrazovne svrhe.
