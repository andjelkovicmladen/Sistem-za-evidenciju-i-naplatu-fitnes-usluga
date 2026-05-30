using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrokerBazePodataka
{
    public class Broker
    {
        private SqlConnection connection;
        private SqlTransaction transaction;
        public Broker()
        {
            // Ako je postavljen FITNES_CONN_STR env var (npr. u Azure App Service), koristi njega.
            // U suprotnom pada na lokalnu LocalDB konekciju — WinForms, TCP server i lokalni WebAPI rade nepromenjeno.
            string cs = Environment.GetEnvironmentVariable("FITNES_CONN_STR")
                ?? @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DB;Integrated Security=True";
            connection = new SqlConnection(cs);
        }
        public void OpenConnection()
        {
            if (connection.State == ConnectionState.Closed) connection.Open();
        }
        public void CloseConnection()
        {
            if (connection != null && connection.State == ConnectionState.Open) connection.Close();
        }
        public void BeginTransaction()
        {
            if (transaction == null) transaction = connection.BeginTransaction();
        }
        public void Commit()
        {
            if (transaction != null)
            {
                transaction.Commit();
                transaction = null;
            }
        }
        public void Rollback()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction = null;
            }
        }
        public SqlDataReader ExecuteReader(string upit)
        {
            SqlCommand cmd = new SqlCommand(upit, connection, transaction);
            return cmd.ExecuteReader();
        }
        public int ExecuteNonQuery(string upit)
        {
            using (SqlCommand cmd = new SqlCommand(upit, connection, transaction))
                return cmd.ExecuteNonQuery();
        }
        public object ExecuteScalar(string upit)
        {
            using (SqlCommand cmd = new SqlCommand(upit, connection, transaction))
                return cmd.ExecuteScalar();
        }
        //Parametri
        public SqlDataReader ExecuteReader(string upit, Dictionary<string, object> parametri)
        {
            SqlCommand cmd = new SqlCommand(upit, connection, transaction);
            if (parametri != null)
            {
                foreach (var param in parametri)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
            }
            return cmd.ExecuteReader();
        }
        public int ExecuteNonQuery(string upit, Dictionary<string, object> parametri)
        {
            using (SqlCommand cmd = new SqlCommand(upit, connection, transaction))
            {
                if (parametri != null)
                {
                    foreach (var param in parametri)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }
                return cmd.ExecuteNonQuery();
            }
        }
        public object ExecuteScalar(string upit, Dictionary<string, object> parametri)
        {
            using (SqlCommand cmd = new SqlCommand(upit, connection, transaction))
            {
                if (parametri != null)
                {
                    foreach (var param in parametri)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }
                return cmd.ExecuteScalar();
            }
        }

        public bool IsConnectionOpen()
        {
            if (connection == null) return false;
            return connection.State == ConnectionState.Open;
        }

    }
}