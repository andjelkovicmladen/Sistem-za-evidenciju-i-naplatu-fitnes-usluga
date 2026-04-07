using Domen;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BrokerBazePodataka
{
    public class BrokerBP
    {
        private Broker broker;

        public BrokerBP()
        {
            broker = new Broker();
        }

        private bool ProveraKonekcije()
        {
            if (!broker.IsConnectionOpen())
            {
                broker.OpenConnection();
                return true;
            }
            return false;
        }

        public void Insert(IEntity entity)
        {
            bool openedHere = false;
            try
            {
                openedHere = ProveraKonekcije();
                string upit = $"INSERT INTO {entity.TableName} ({entity.InsertColumns}) VALUES ({entity.InsertValues})";
                broker.ExecuteNonQuery(upit, entity.GetParameters());
            }
            finally
            {
                if (openedHere) broker.CloseConnection();
            }
        }

        public int InsertWithIdentity(IEntity entity, string idColumnName)
        {
            bool openedHere = false;
            try
            {
                openedHere = ProveraKonekcije();
                string upit = $"INSERT INTO {entity.TableName} ({entity.InsertColumns}) OUTPUT inserted.{idColumnName} VALUES ({entity.InsertValues})";
                object result = broker.ExecuteScalar(upit, entity.GetParameters());
                return Convert.ToInt32(result);
            }
            finally
            {
                if (openedHere) broker.CloseConnection();
            }
        }

        public void Update(IEntity entity)
        {
            bool openedHere = false;
            try
            {
                openedHere = ProveraKonekcije();
                string upit = $"UPDATE {entity.TableName} SET {entity.UpdateSetClause} WHERE {entity.WhereClause}";

                var allParams = new Dictionary<string, object>(entity.GetParameters());
                var whereParams = entity.GetWhereParameters();
                if (whereParams != null)
                {
                    foreach (var param in whereParams)
                    {
                        if (!allParams.ContainsKey(param.Key))
                        {
                            allParams.Add(param.Key, param.Value);
                        }
                    }
                }
                broker.ExecuteNonQuery(upit, allParams);
            }
            finally
            {
                if (openedHere) broker.CloseConnection();
            }
        }

        public void Delete(IEntity entity)
        {
            bool openedHere = false;
            try
            {
                openedHere = ProveraKonekcije();
                string upit = $"DELETE FROM {entity.TableName} WHERE {entity.WhereClause}";
                broker.ExecuteNonQuery(upit, entity.GetWhereParameters());
            }
            finally
            {
                if (openedHere) broker.CloseConnection();
            }
        }

        public List<IEntity> GetAll(IEntity entity)
        {
            bool openedHere = false;
            SqlDataReader reader = null;
            try
            {
                openedHere = ProveraKonekcije();
                string upit = $"SELECT * FROM {entity.TableName}";
                reader = broker.ExecuteReader(upit);
                List<IEntity> list = entity.GetReaderList(reader);
                reader.Close();
                return list;
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
                if (openedHere) broker.CloseConnection();
            }
        }

        public List<IEntity> ExecuteQuery(IEntity entity, string query, Dictionary<string, object> parameters = null)
        {
            bool openedHere = false;
            SqlDataReader reader = null;
            try
            {
                openedHere = ProveraKonekcije();
                reader = parameters != null ?
                    broker.ExecuteReader(query, parameters) :
                    broker.ExecuteReader(query);
                List<IEntity> lista = entity.GetReaderList(reader);
                reader.Close();
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception($"Greška pri izvršavanju upita: {ex.Message}", ex);
            }
            finally
            {
                if (reader != null && !reader.IsClosed) reader.Close();
                if (openedHere) broker.CloseConnection();
            }
        }

        public object ExecuteScalar(string query, Dictionary<string, object> parameters = null)
        {
            bool openedHere = false;
            try
            {
                openedHere = ProveraKonekcije();
                return parameters != null ?
                    broker.ExecuteScalar(query, parameters) :
                    broker.ExecuteScalar(query);
            }
            finally
            {
                if (openedHere) broker.CloseConnection();
            }
        }

        public void DeleteByCondition(IEntity entity, string whereClause, Dictionary<string, object> parameters)
        {
            bool openedHere = false;
            try
            {
                openedHere = ProveraKonekcije();
                string upit = $"DELETE FROM {entity.TableName} WHERE {whereClause}";
                broker.ExecuteNonQuery(upit, parameters);
            }
            finally
            {
                if (openedHere) broker.CloseConnection();
            }
        }

        public void BeginTransaction()
        {
            ProveraKonekcije();
            broker.BeginTransaction();
        }

        public void Commit()
        {
            try
            {
                broker.Commit();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Rollback()
        {
            try
            {
                broker.Rollback();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška pri Rollback-u (možda očekivana): {ex.Message}");
            }
        }

        public void CloseConnection()
        {
            broker.CloseConnection();
        }

        public bool IsConnectionOpen()
        {
            return broker.IsConnectionOpen();
        }
    }
}