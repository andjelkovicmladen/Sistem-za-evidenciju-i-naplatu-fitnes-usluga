-- ============================================================
-- Migracijska skripta: kreiranje šeme i seed podataka
-- Pokrenuti unutar prazne Azure SQL baze (već povezan na bazu).
-- Ne sadrži USE/CREATE DATABASE — Azure SQL je per-konekcija baza.
-- ============================================================

-- 1) Tabele bez stranih ključeva (roditelji)

CREATE TABLE Administrator (
    idAdministrator INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Ime             NVARCHAR(100) NOT NULL,
    Prezime         NVARCHAR(100) NOT NULL,
    Email           NVARCHAR(150) NOT NULL,
    [Password]      NVARCHAR(255) NOT NULL
);
GO

CREATE TABLE TipClanarine (
    idTipClanarine  INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Naziv           NVARCHAR(100) NOT NULL,
    Opis            NVARCHAR(255) NULL
);
GO

CREATE TABLE FitnessUsluga (
    idFitnessUsluga INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Naziv           NVARCHAR(150) NOT NULL,
    TipUsluge       INT NOT NULL,
    CenaPoSatu      DECIMAL(10,2) NOT NULL,
    MaxKapacitet    INT NOT NULL
);
GO

-- 2) Tabele sa stranim ključevima (deca)

CREATE TABLE Clan (
    idClan          INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Ime             NVARCHAR(100) NOT NULL,
    Prezime         NVARCHAR(100) NOT NULL,
    BrojTelefona    NVARCHAR(20)  NOT NULL,
    Email           NVARCHAR(150) NOT NULL,
    [Password]      NVARCHAR(255) NOT NULL,
    idTipClanarine  INT NOT NULL,
    CONSTRAINT FK_Clan_TipClanarine FOREIGN KEY (idTipClanarine)
        REFERENCES TipClanarine(idTipClanarine)
);
GO

CREATE TABLE Racun (
    idRacun         INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    DatumIzdavanja  DATETIME NOT NULL,
    DatumDospeca    DATETIME NOT NULL,
    idAdministrator INT NOT NULL,
    idClan          INT NOT NULL,
    CONSTRAINT FK_Racun_Administrator FOREIGN KEY (idAdministrator)
        REFERENCES Administrator(idAdministrator),
    CONSTRAINT FK_Racun_Clan FOREIGN KEY (idClan)
        REFERENCES Clan(idClan)
);
GO

CREATE TABLE StavkaRacuna (
    idRacun         INT NOT NULL,
    rb              INT NOT NULL,
    idFitnessUsluga INT NOT NULL,
    BrojSati        INT NOT NULL,
    Iznos           DECIMAL(10,2) NOT NULL,
    CONSTRAINT PK_StavkaRacuna PRIMARY KEY (idRacun, rb),
    CONSTRAINT FK_StavkaRacuna_Racun FOREIGN KEY (idRacun)
        REFERENCES Racun(idRacun),
    CONSTRAINT FK_StavkaRacuna_FitnessUsluga FOREIGN KEY (idFitnessUsluga)
        REFERENCES FitnessUsluga(idFitnessUsluga)
);
GO

CREATE TABLE TerminTreninga (
    idTermin        INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    DatumVreme      DATETIME NOT NULL,
    TrajanjeMinuta  INT NOT NULL,
    idFitnessUsluga INT NOT NULL,
    idAdministrator INT NOT NULL,
    StatusOpis      NVARCHAR(100) NULL,
    CONSTRAINT FK_TerminTreninga_FitnessUsluga FOREIGN KEY (idFitnessUsluga)
        REFERENCES FitnessUsluga(idFitnessUsluga),
    CONSTRAINT FK_TerminTreninga_Administrator FOREIGN KEY (idAdministrator)
        REFERENCES Administrator(idAdministrator)
);
GO

-- 3) View za pregled računa (parity sa lokalnom bazom; WebAPI ga ne koristi direktno)

CREATE VIEW vw_RacuniPregled AS
SELECT r.idRacun, r.DatumIzdavanja, r.DatumDospeca,
       a.Ime + ' ' + a.Prezime AS Administrator,
       c.Ime + ' ' + c.Prezime AS Clan,
       ISNULL(SUM(sr.Iznos), 0) AS UkupanIznos
FROM Racun r
JOIN Administrator a ON r.idAdministrator = a.idAdministrator
JOIN Clan c ON r.idClan = c.idClan
LEFT JOIN StavkaRacuna sr ON r.idRacun = sr.idRacun
GROUP BY r.idRacun, r.DatumIzdavanja, r.DatumDospeca,
         a.Ime, a.Prezime, c.Ime, c.Prezime;
GO

-- 4) Seed podaci (sa eksplicitnim ID-jevima radi reproducibilnosti)

SET IDENTITY_INSERT Administrator ON;
INSERT INTO Administrator (idAdministrator, Ime, Prezime, Email, [Password])
VALUES (1, 'Marko', 'Markovic', 'marko@fitness.rs', 'admin123');
SET IDENTITY_INSERT Administrator OFF;
GO

SET IDENTITY_INSERT TipClanarine ON;
INSERT INTO TipClanarine (idTipClanarine, Naziv, Opis) VALUES
    (1, 'Basic',   'Pristup teretani i grupnim treninzima'),
    (2, 'Premium', 'Basic + personalni treninzi'),
    (3, 'VIP',     'Svi sadrzaji + spa i sauna');
SET IDENTITY_INSERT TipClanarine OFF;
GO

SET IDENTITY_INSERT FitnessUsluga ON;
INSERT INTO FitnessUsluga (idFitnessUsluga, Naziv, TipUsluge, CenaPoSatu, MaxKapacitet) VALUES
    (1, 'Grupni Yoga',        0,  800.00, 20),
    (2, 'Personalni Trening', 1, 2500.00,  1),
    (3, 'Spa & Relax',        2, 1200.00,  4),
    (4, 'Grupna Aerobika',    0,  700.00, 25);
SET IDENTITY_INSERT FitnessUsluga OFF;
GO
