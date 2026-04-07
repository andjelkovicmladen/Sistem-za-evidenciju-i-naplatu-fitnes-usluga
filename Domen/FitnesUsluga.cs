using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domen
{
    public class FitnesUsluga : IEntity
    {
        public int IdFitnesUsluga { get; set; }
        public string Naziv { get; set; }
        public TipUsluge TipUsluge { get; set; }
        public decimal CenaPoSatu { get; set; }
        public int MaxKapacitet { get; set; }

        public override string ToString() => Naziv;

        public string TableName => "FitnesUsluga";
        public string InsertColumns => "Naziv, TipUsluge, CenaPoSatu, MaxKapacitet";
        public string InsertValues => "@Naziv, @TipUsluge, @CenaPoSatu, @MaxKapacitet";
        public string UpdateSetClause => "Naziv=@Naziv, TipUsluge=@TipUsluge, CenaPoSatu=@CenaPoSatu, MaxKapacitet=@MaxKapacitet";
        public string WhereClause => "idFitnesUsluga=@IdFitnesUsluga";

        public Dictionary<string, object> GetParameters() => new()
        {
            { "@IdFitnesUsluga", IdFitnesUsluga },
            { "@Naziv", Naziv },
            { "@TipUsluge", (int)TipUsluge },
            { "@CenaPoSatu", CenaPoSatu },
            { "@MaxKapacitet", MaxKapacitet }
        };

        public Dictionary<string, object> GetWhereParameters() => new()
        {
            { "@IdFitnesUsluga", IdFitnesUsluga }
        };

        public List<IEntity> GetReaderList(SqlDataReader reader)
        {
            var lista = new List<IEntity>();
            while (reader.Read())
                lista.Add(new FitnesUsluga
                {
                    IdFitnesUsluga = (int)reader["idFitnesUsluga"],
                    Naziv = (string)reader["Naziv"],
                    TipUsluge = (TipUsluge)Convert.ToInt32(reader["TipUsluge"]),
                    CenaPoSatu = Convert.ToDecimal(reader["CenaPoSatu"]),
                    MaxKapacitet = (int)reader["MaxKapacitet"]
                });
            return lista;
        }
    }
}
