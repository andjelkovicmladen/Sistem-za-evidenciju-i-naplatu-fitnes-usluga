using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domen
{
    public class Administrator : IEntity
    {
        public int IdAdministrator { get; set; }
        public string  Ime { get; set; }
        public string  Prezime { get; set; }
        public string  Email { get; set; }
        public string  Password { get; set; }

        public string  ImePrezime => $"{Ime} {Prezime}";

        public string  TableName => "Administrator";
        public string InsertColumns => "Ime, Prezime, Email, Password";
        public string  InsertValues => "@Ime, @Prezime, @Email, @Password";
        public string  UpdateSetClause => "Ime=@Ime, Prezime=@Prezime";
        public string  WhereClause => "idAdministrator=@IdAdministrator";

        public Dictionary<string, object> GetParameters() => new()
        {
            { "@IdAdministrator", IdAdministrator },
            { "@Ime", Ime },
            { "@Prezime", Prezime },
            { "@Email", Email },
            { "@Password", Password }
        };

        public Dictionary<string, object> GetWhereParameters() => new()
        {
            { "@IdAdministrator", IdAdministrator }
        };

        public List<IEntity> GetReaderList(SqlDataReader reader)
        {
            var lista = new List<IEntity>();
            while (reader.Read())
                lista.Add(new Administrator
                {
                    IdAdministrator = (int)reader["idAdministrator"],
                    Ime = (string)reader["Ime"],
                    Prezime = (string)reader["Prezime"],
                    Email = (string)reader["Email"],
                    Password = (string)reader["Password"]
                });
            return lista;
        }
    }
}
