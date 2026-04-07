using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domen
{
    public class TipClanarine : IEntity
    {

        public int IdTipClanarine { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }

        public override string ToString() => Naziv;

        public string TableName => "TipClanarine";
        public string InsertColumns => "Naziv, Opis";
        public string InsertValues => "@Naziv, @Opis";
        public string UpdateSetClause => "Naziv=@Naziv, Opis=@Opis";
        public string WhereClause => "idTipClanarine=@IdTipClanarine";

        public Dictionary<string, object> GetParameters() => new()
        {
            { "@IdTipClanarine", IdTipClanarine },
            { "@Naziv", Naziv },
            { "@Opis", Opis ?? "" }
        };

        public Dictionary<string, object> GetWhereParameters() => new()
        {
            { "@IdTipClanarine", IdTipClanarine }
        };

        public List<IEntity> GetReaderList(SqlDataReader reader)
        {
            var lista = new List<IEntity>();
            while (reader.Read())
                lista.Add(new TipClanarine
                {
                    IdTipClanarine = (int)reader["idTipClanarine"],
                    Naziv = (string)reader["Naziv"],
                    Opis = reader["Opis"] == DBNull.Value ? "" : (string)reader["Opis"]
                });
            return lista;
        }
    }
}
