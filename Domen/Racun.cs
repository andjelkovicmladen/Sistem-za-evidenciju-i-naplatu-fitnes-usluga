using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domen
{
    public class Racun : IEntity
    {
        public int IdRacun { get; set; }
        public DateTime DatumIzdavanja { get; set; }
        public DateTime DatumDospeca { get; set; }
        public int IdAdministrator { get; set; }
        public int IdClan { get; set; }
        public List<StavkaRacuna> StavkeRacuna { get; set; } = new();

        // Za prikaz u UI
        public string AdministratorImePrezime { get; set; }
        public string ClanImePrezime { get; set; }
        public decimal UkupanIznos { get; set; }

        public decimal Iznos => (StavkeRacuna?.Count > 0)
            ? StavkeRacuna.Sum(s => s.Iznos)
            : (UkupanIznos > 0 ? UkupanIznos : 0);

        public string TableName => "Racun";
        public string InsertColumns => "DatumIzdavanja, DatumDospeca, idAdministrator, idClan";
        public string InsertValues => "@DatumIzdavanja, @DatumDospeca, @IdAdministrator, @IdClan";
        public string UpdateSetClause => "DatumIzdavanja=@DatumIzdavanja, DatumDospeca=@DatumDospeca";
        public string WhereClause => "idRacun=@IdRacun";

        public Dictionary<string, object> GetParameters() => new()
        {
            { "@IdRacun", IdRacun },
            { "@DatumIzdavanja", DatumIzdavanja },
            { "@DatumDospeca", DatumDospeca },
            { "@IdAdministrator", IdAdministrator },
            { "@IdClan", IdClan }
        };

        public Dictionary<string, object> GetWhereParameters() => new()
        {
            { "@IdRacun", IdRacun }
        };

        public List<IEntity> GetReaderList(SqlDataReader reader)
        {
            var lista = new List<IEntity>();
            while (reader.Read())
                lista.Add(new Racun
                {
                    IdRacun = (int)reader["idRacun"],
                    DatumIzdavanja = (DateTime)reader["DatumIzdavanja"],
                    DatumDospeca = (DateTime)reader["DatumDospeca"],
                    IdAdministrator = (int)reader["idAdministrator"],
                    IdClan = (int)reader["idClan"],
                    AdministratorImePrezime = (string)reader["Administrator"],
                    ClanImePrezime = (string)reader["Clan"],
                    UkupanIznos = Convert.ToDecimal(reader["UkupanIznos"])
                });
            return lista;
        }
    }
    }
