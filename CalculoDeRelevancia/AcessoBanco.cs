using MySql.Data.MySqlClient;
using System.Data;

namespace CalculoDeRelevancia
{
    public sealed class AcessoBanco
    {
        const string CONNECTION_STRING = "Server=localhost;Database=tcc;Uid=root;Pwd=";

        private static MySqlConnection _connection;

        public static AcessoBanco Instance { get; } = new AcessoBanco(CONNECTION_STRING);

        private AcessoBanco(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
        }

        public int ExecuteNonQuery(string query)
        {
            MySqlCommand command = _connection.CreateCommand();
            int result = 0;

            try
            {
                _connection.Open();
                command.CommandText = query;
                command.ExecuteNonQuery();
                result = (int)command.LastInsertedId;
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return result;
        }

        public DataTable ExecuteReader(string query)
        {
            DataTable dt = new DataTable();
            MySqlCommand command = _connection.CreateCommand();

            try
            {
                _connection.Open();
                command.CommandText = query;
                var result = command.ExecuteReader();

                dt.Load(result);
            }
            finally
            {
                if (_connection.State == ConnectionState.Open)
                    _connection.Close();
            }

            return dt;
        }
    }
}
