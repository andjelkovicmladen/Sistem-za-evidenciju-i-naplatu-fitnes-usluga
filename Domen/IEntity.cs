using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Domen
{
    public interface IEntity
    {
        string TableName { get; }
        string InsertColumns { get; }
        string InsertValues { get; }
        string UpdateSetClause { get; }
        string WhereClause { get; }
        Dictionary<string, object> GetParameters();
        Dictionary<string, object> GetWhereParameters();
        List<IEntity> GetReaderList(SqlDataReader reader);
    }
}
