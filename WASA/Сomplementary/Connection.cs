namespace WASA.Сomplementary
{
    internal class Connection
    {

        public static string GetConnectionString()
        {
            return "Host=5.137.18.167;Port=5432;DataBase=wasa;Username=postgres;Password=1234";
        }

        public static string GetConnectionString(string Ip)
        {
            return $"Host='{Ip}';Port=5432;DataBase=wasa;Username=postgres;Password=1234";
        }

        public static string GetConnectionString(string username, string password)
        {
            return $"host=5.137.18.167;port=5432;database=wasa;username='{username}';password='{password}'";
        }

        public static string GetConnectionString(string Ip, string Port, string DataBaseName, string Username, string Password)
        {
            return $"Host='{Ip}';Port='{Port}';DataBase='{DataBaseName}';Username='{Username}';Password='{Password}'";
        }
    }
}
