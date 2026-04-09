# 🏋️ Sistem za Evidenciju i Naplatu Fitnes Usluga

Studentski projekat — desktop aplikacija za upravljanje fitnes centrom, razvijena u **C# (.NET 8)** koristeći **client-server arhitekturu** i **Windows Forms** UI.

---

## 📋 Opis projekta

Aplikacija omogućava administratorima fitnes centra da upravljaju članovima, fitnes uslugama, računima i terminima treninga. Komunikacija između klijentske aplikacije i servera odvija se putem **TCP soketa** uz **JSON serijalizaciju** podataka.

---

## 🛠️ Tehnologije

| Tehnologija | Verzija |
|---|---|
| C# / .NET | 8.0 |
| Windows Forms | net8.0-windows |
| SQL Server LocalDB | 15.x |
| Microsoft.Data.SqlClient | NuGet |
| System.Text.Json | NuGet |

---

## 🏗️ Arhitektura projekta

```
Sistem-za-evidenciju-i-naplatu-fitnes-usluga/
│
├── Domen/                  # Domenski entiteti (Clan, Racun, Administrator...)
├── Zajednicki/             # Zajednički sloj (Zahtev, Odgovor, Operacija, Serializer)
├── BrokerBazePodataka/     # Pristup bazi podataka (Broker, BrokerBP)
├── Server/                 # Server aplikacija (TCP listener, operacije)
│   └── Operacije/          # Konkretne operacije nad bazom
├── KorisnickiInterfejs/    # Klijentska WinForms aplikacija
│   └── UIKontroler/        # Kontroler za komunikaciju sa serverom
└── Database/               # SQL skripta za kreiranje baze
```

---

## ⚙️ Funkcionalnosti

### 👤 Upravljanje članovima
- Kreiranje novog člana
- Pretraga članova (po imenu, prezimenu, emailu, tipu članarine)
- Izmena podataka člana
- Brisanje člana

### 🧾 Upravljanje računima
- Kreiranje računa sa stavkama
- Pretraga računa (po datumu, članu, administratoru)
- Izmena računa
- Prikaz računa sa svim stavkama

### 🏃 Termini treninga
- Dodavanje termina treninga
- Vezivanje termina za fitnes uslugu i administratora

### 🔐 Autentifikacija
- Prijava administratora sa email/lozinkom
- Zaštita od višestrukih prijava istog korisnika

---

## 🗄️ Baza podataka

Aplikacija koristi **SQL Server LocalDB** sa bazom pod nazivom `DB`.

### Tabele
- `Administrator` — podaci o adminsitratorima
- `TipClanarine` — tipovi članarine (mesečna, godišnja...)
- `Clan` — evidencija članova fitnes centra
- `FitnesUsluga` — dostupne fitnes usluge sa cenom po satu
- `Racun` — računi za usluge
- `StavkaRacuna` — stavke pojedinog računa
- `TerminTreninga` — zakazani termini treninga

---

## 🚀 Pokretanje projekta

### Preduslovi
- Visual Studio 2026
- .NET 8 SDK
- SQL Server LocalDB (dolazi uz Visual Studio)
- SQL Server Management Studio (SSMS) — opciono

### Koraci

**1. Klonirati repozitorijum**
```bash
git clone https://github.com/tvoj-username/naziv-repoa.git
```

**2. Kreirati bazu podataka**

Otvoriti SSMS, konektovati se na `(localdb)\MSSQLLocalDB`, kreirati bazu `DB` i pokrenuti skriptu:
```
Database/script.sql
```

**3. Dodati test administratora**
```sql
USE DB;
INSERT INTO Administrator (Ime, Prezime, Email, Password)
VALUES ('Admin', 'Test', 'admin@fitnes.com', 'admin123');

INSERT INTO TipClanarine (Naziv, Opis)
VALUES ('Mesecna', 'Mesecna clanarina'),
       ('Godisnja', 'Godisnja clanarina');
```

**4. Pokrenuti projekte**

U Visual Studio postaviti **Multiple Startup Projects**:
- Desni klik na Solution → Properties → Startup Project → Multiple startup projects
- `Server` → Start
- `KorisnickiInterfejs` → Start

Ili pokrenuti ručno — prvo **Server**, pa **KorisnickiInterfejs**.

**5. Login**
- Email: `admin@fitnes.com`
- Password: `admin123`

---

## 🔌 Konfiguracija konekcije

### Baza podataka
U fajlu `BrokerBazePodataka/Broker.cs`:
```csharp
connection = new SqlConnection(
    @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DB;Integrated Security=True");
```

### Server adresa i port
U fajlu `KorisnickiInterfejs/UIKontroler/Kontroler.cs`:
```csharp
socket.Connect("127.0.0.1", 9999);
```

---

## 👥 Autori

- **Ime Prezime** — *student*
- Fakultet organizacionih nauka, Beograd
- Predmet: Programiranje — PS Final

---

## 📄 Licenca

Ovaj projekat je izrađen u obrazovne svrhe.
