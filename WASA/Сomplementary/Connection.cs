using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Windows;

namespace WASA.Сomplementary
{
    internal class Connection
    {
        private static string[] _mac = new[] { "B42E9966DBE8", "3E219C83D01F" };
        private const string _ip = "90.189.146.84";
        public static string GetConnectionString()
        {
            try
            {
                var macAddr = (from nic in NetworkInterface.GetAllNetworkInterfaces() where nic.OperationalStatus == OperationalStatus.Up select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
                foreach (var mac in _mac)
                {
                    if (mac == macAddr)
                    {
                        return $"Host=192.168.0.6;Port=5432;DataBase=wasa;Username=postgres;Password=1234";
                        
                    }
                   
                }
                return $"Host='{_ip}';Port=5432;DataBase=wasa;Username=postgres;Password=1234";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
