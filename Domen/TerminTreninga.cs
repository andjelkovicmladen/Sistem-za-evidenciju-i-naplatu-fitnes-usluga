using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domen
{
    public class TerminTreninga : IEntity
    {
        public int IdTermin { get; set; }
        public DateTime DatumVreme { get; set; }
        public int TrajanjeMinuta { get; set; }
        public int IdFitnesUsluga { get; set; }
        public int IdAdministrator { get; set; }
        public string StatusOpis { get; set; }

        public string TableName => "TerminTreninga";
        public string InsertColumns => "DatumVreme, TrajanjeMinuta, idFitnesUsluga, idAdministrator, StatusOpis";
        public string InsertValues => "@DatumVreme, @TrajanjeMinuta, @IdFitnesUsluga, @IdAdministrator, @StatusOpis";
        public string UpdateSetClause => "DatumVreme=@DatumVreme, TrajanjeMinuta=@TrajanjeMinuta, StatusOpis=@StatusOpis";
        public string WhereClause => "idTermin=@IdTermin";

        public Dictionary<string, object> GetParameters() => new()
        {
            { "@IdTermin", IdTermin },
            { "@DatumVreme", DatumVreme },
            { "@TrajanjeMinuta", TrajanjeMinuta },
            { "@IdFitnesUsluga", IdFitnesUsluga },
            { "@IdAdministrator", IdAdministrator },
            { "@StatusOpis", StatusOpis ?? "" }
        };

        public Dictionary<string, object> GetWhereParameters() => new()
        {
            { "@IdTermin", IdTermin }
        };

        public List<IEntity> GetReaderList(SqlDataReader reader)
        {
            var lista = new List<IEntity>();
            while (reader.Read())
                lista.Add(new TerminTreninga
                {
                    IdTermin = (int)reader["idTermin"],
                    DatumVreme = (DateTime)reader["DatumVreme"],
                    TrajanjeMinuta = (int)reader["TrajanjeMinuta"],
                    IdFitnesUsluga = (int)reader["idFitnesUsluga"],
                    IdAdministrator = (int)reader["idAdministrator"],
                    StatusOpis = reader["StatusOpis"] == DBNull.Value ? "" : (string)reader["StatusOpis"]
                });
            return lista;
        }
    }
}
