using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domen
{
    public class Clan : IEntity
    {

        public int IdClan { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string BrojTelefona { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int IdTipClanarine { get; set; }

        public string ImePrezime => $"{Ime} {Prezime}";

        public string TableName => "Clan";
        public string InsertColumns => "Ime, Prezime, BrojTelefona, Email, Password, idTipClanarine";
        public string InsertValues => "@Ime, @Prezime, @BrojTelefona, @Email, @Password, @IdTipClanarine";
        public string UpdateSetClause => "Ime=@Ime, Prezime=@Prezime, BrojTelefona=@BrojTelefona, Email=@Email, Password=@Password, idTipClanarine=@IdTipClanarine";
        public string WhereClause => "idClan=@IdClan";

        public Dictionary<string, object> GetParameters() => new()
        {
            { "@IdClan", IdClan },
            { "@Ime", Ime },
            { "@Prezime", Prezime },
            { "@BrojTelefona", BrojTelefona },
            { "@Email", Email },
            { "@Password", Password },
            { "@IdTipClanarine", IdTipClanarine }
        };

        public Dictionary<string, object> GetWhereParameters() => new()
        {
            { "@IdClan", IdClan }
        };

        public List<IEntity> GetReaderList(SqlDataReader reader)
        {
            var lista = new List<IEntity>();
            while (reader.Read())
                lista.Add(new Clan
                {
                    IdClan = (int)reader["idClan"],
                    Ime = (string)reader["Ime"],
                    Prezime = (string)reader["Prezime"],
                    BrojTelefona = (string)reader["BrojTelefona"],
                    Email = (string)reader["Email"],
                    Password = (string)reader["Password"],
                    IdTipClanarine = (int)reader["idTipClanarine"]
                });
            return lista;
        }
    }
}
