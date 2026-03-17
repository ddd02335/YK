using Npgsql;

namespace TeacherAccounting
{
    public static class DatabaseHelper
    {
        private const string ConnectionString =
            "Host=localhost;Database=bdK;Username=postgres;Password=1234;";

        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        public static bool TestConnection()
        {
            try
            {
                using var conn = GetConnection();
                conn.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
