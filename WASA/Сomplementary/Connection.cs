using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;

namespace WASA.Сomplementary
{
    internal class Connection
    {
        static String host = System.Net.Dns.GetHostName();


        public static string GetConnectionString()
        {
            var httpClient = new HttpClient();

            if (Properties.Resources.host == GetLocalIPAddress())
            {
                return $"Host=localhost;Port=5432;DataBase=wasa;Username=postgres;Password=1234";
            }
            else
            {
                return $"Host='5.137.18.167';Port=5432;DataBase=wasa;Username=postgres;Password=1234";
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

        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
