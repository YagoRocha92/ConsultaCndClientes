using System.Data.SQLite;
using System.Data;


namespace ConsultaCertidaoCliente.Utilitarios
{
    public static class DbHelper
    {
        private static string connectionString = "Data Source=C:\\Users\\yago_\\OneDrive\\repositorios\\ConsultaCertidaoCliente\\consultaCertidaoCliente.db";

        public static SQLiteConnection GetConnection()
        {
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}