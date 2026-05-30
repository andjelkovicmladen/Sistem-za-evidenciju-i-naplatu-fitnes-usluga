// Domenski tipovi - usklađeni sa JSON odgovorima WebAPI-ja (camelCase serijalizacija)

export interface Administrator {
  idAdministrator: number;
  imePrezime: string;
  email: string;
}

export interface Clan {
  idClan: number;
  ime: string;
  prezime: string;
  brojTelefona: string;
  email: string;
  password: string;
  idTipClanarine: number;
  imePrezime?: string;
}

export interface TipClanarine {
  idTipClanarine: number;
  naziv: string;
  opis: string;
}

export const TipUsluge = {
  GrupniTrening: 0,
  PersonalniTrening: 1,
  Spa: 2,
} as const;
export type TipUsluge = (typeof TipUsluge)[keyof typeof TipUsluge];

export interface FitnesUsluga {
  idFitnesUsluga: number;
  naziv: string;
  tipUsluge: TipUsluge;
  cenaPoSatu: number;
  maxKapacitet: number;
}

export interface StavkaRacuna {
  idRacun: number;
  rb: number;
  idFitnesUsluga: number;
  brojSati: number;
  iznos: number;
  fitnesUsluga?: FitnesUsluga;
}

export interface Racun {
  idRacun: number;
  datumIzdavanja: string;
  datumDospeca: string;
  idAdministrator: number;
  idClan: number;
  stavkeRacuna: StavkaRacuna[];
  administratorImePrezime?: string;
  clanImePrezime?: string;
  ukupanIznos?: number;
  iznos?: number;
}

export interface TerminTreninga {
  idTermin: number;
  datumVreme: string;
  trajanjeMinuta: number;
  idFitnesUsluga: number;
  idAdministrator: number;
  statusOpis: string;
}

export interface ClanSearchParametri {
  ime?: string;
  prezime?: string;
  email?: string;
  idTipClanarine?: number | null;
}

export interface RacunSearchParametri {
  datumOd?: string | null;
  datumDo?: string | null;
  idClan?: number | null;
  idAdministrator?: number | null;
}

export interface LoginResponse {
  token: string;
  admin: Administrator;
}
