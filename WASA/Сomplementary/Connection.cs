using System;
using System.Net;

namespace WASA.Сomplementary
{
    internal class Connection
    {
        static String host = System.Net.Dns.GetHostName();
        static string ip = new WebClient().DownloadString("https://api.ipify.org");

        public static string GetConnectionString()
        {
            if (Properties.Resources.host == ip)
            {
                return $"Host=localhost;Port=5432;DataBase=wasa;Username=postgres;Password=1234";
            }
            else
            {
                return $"Host='{Convert.ToString(ip)}';Port=5432;DataBase=wasa;Username=postgres;Password=1234";
            }
        }

        public static string GetConnectionString(string username, string password)
        {
            return $"host='{host}';port=5432;database=wasa;username='{username}';password='{password}'";
        }

        public static string GetConnectionString(string Ip, string Port, string DataBaseName, string Username, string Password)
        {
            return $"Host='{host}';Port='{Port}';DataBase='{DataBaseName}';Username='{Username}';Password='{Password}'";
        }
    }
}
