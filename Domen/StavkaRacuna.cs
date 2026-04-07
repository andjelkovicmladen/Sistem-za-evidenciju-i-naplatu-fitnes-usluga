using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domen
{
    public class StavkaRacuna : IEntity
    {
        public int IdRacun { get; set; }
        public int Rb { get; set; }
        public int IdFitnesUsluga { get; set; }
        public int BrojSati { get; set; }
        public decimal Iznos { get; set; }
        public FitnesUsluga FitnesUsluga { get; set; }

        public string TableName => "StavkaRacuna";
        public string InsertColumns => "idRacun, rb, idFitnesUsluga, BrojSati, Iznos";
        public string InsertValues => "@IdRacun, @Rb, @IdFitnesUsluga, @BrojSati, @Iznos";
        public string UpdateSetClause => "idFitnesUsluga=@IdFitnesUsluga, BrojSati=@BrojSati, Iznos=@Iznos";
        public string WhereClause => "idRacun=@IdRacun AND rb=@Rb";

        public Dictionary<string, object> GetParameters() => new()
        {
            { "@IdRacun", IdRacun },
            { "@Rb", Rb },
            { "@IdFitnesUsluga", IdFitnesUsluga },
            { "@BrojSati", BrojSati },
            { "@Iznos", Iznos }
        };

        public Dictionary<string, object> GetWhereParameters() => new()
        {
            { "@IdRacun", IdRacun },
            { "@Rb", Rb }
        };

        public List<IEntity> GetReaderList(SqlDataReader reader)
        {
            var lista = new List<IEntity>();
            while (reader.Read())
                lista.Add(new StavkaRacuna
                {
                    IdRacun = (int)reader["idRacun"],
                    Rb = (int)reader["rb"],
                    IdFitnesUsluga = (int)reader["idFitnesUsluga"],
                    BrojSati = (int)reader["BrojSati"],
                    Iznos = Convert.ToDecimal(reader["Iznos"]),
                    FitnesUsluga = new FitnesUsluga
                    {
                        IdFitnesUsluga = (int)reader["idFitnesUsluga"],
                        Naziv = (string)reader["Naziv"],
                        CenaPoSatu = Convert.ToDecimal(reader["CenaPoSatu"])
                    }
                });
            return lista;
        }
    }
}
