using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;

namespace WASA.Сomplementary
{
    internal class Connection
    {
        static FileIO fileIO = new();
        public static string GetConnectionString()
        {
                return $"Host='{fileIO.GetAddressData("ip").ToString().Replace('"', ' ').Trim()}';Port={fileIO.GetAddressData("port").ToString().Replace('"', ' ').Trim()};DataBase={fileIO.GetAddressData("name").ToString().Replace('"', ' ').Trim()};Username={fileIO.GetAddressData("username").ToString().Replace('"', ' ').Trim()};Password={fileIO.GetAddressData("password").ToString().Replace('"', ' ').Trim()}";
        }
    }
}
