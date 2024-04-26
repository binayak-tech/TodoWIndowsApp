using System;
using System.Data.SqlClient;
using System.Data;

using System.Windows.Forms;

namespace TodoApp
{
    public class DbManager
    {
        private string connectionString;
        private SqlConnection connection;
        public DbManager()
        {
            connectionString = "Data Source=DESKTOP-4FMU0HC\\SQLSERVERBK;Initial Catalog=TodoDB;Persist Security Info=True;User ID=CertifyTester;Password=12345;";
            connection = new SqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connecting to the database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // using pattern helps to destroy the connection
        //public bool Save()
        //{
        //    using (connection = new SqlConnection(connectionString)) { 
                
        //    }
        //}
        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Close();
            }
        }
        // methods to add and a method to update into database

        public bool ExecuteNonQuery(string query, SqlParameter[] parameters)
        {
            bool result = false;

            try
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                if (cmd.ExecuteNonQuery() != 0)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error executing query: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }

        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            DataTable dt = new DataTable();

            try
            {
                SqlCommand cmd = new SqlCommand(query, connection);
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error executing query: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return dt;
        }
    }
}
