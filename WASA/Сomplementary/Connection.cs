namespace WASA.Сomplementary
{
    internal class Connection
    {

        public static string GetConnectionString()
        {
            return "Host=5.137.198.135;Port=5432;DataBase=wasa;Username=postgres;Password=1234";
        }

        public static string GetConnectionString(string Ip)
        {
            return $"Host='{Ip}';Port=5432;DataBase=wasa;Username=postgres;Password=1234";
        }

        public static string GetConnectionString(string Username, string Password)
        {
            return $"Host=5.137.198.135;Port=5432;DataBase=wasa;Username='{Username}';Password='{Password}'";
        }

        public static string GetConnectionString(string Ip, string Port, string DataBaseName, string Username, string Password)
        {
            return $"Host='{Ip}';Port='{Port}';DataBase='{DataBaseName}';Username='{Username}';Password='{Password}'";
        }
    }
}
