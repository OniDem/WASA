using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace WASA.Сomplementary
{
    internal class Connection
    {
        private const string _mac = "B42E9966DBE8";
        private const string _ip = "90.189.146.84";
        public static string GetConnectionString()
        {
            var macAddr = (from nic in NetworkInterface.GetAllNetworkInterfaces() where nic.OperationalStatus == OperationalStatus.Up select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
            

            if (_mac == macAddr)
            {
                return $"Host=localhost;Port=5432;DataBase=wasa;Username=postgres;Password=1234";
            }
            else
            {
                return $"Host='{_ip}';Port=5432;DataBase=wasa;Username=postgres;Password=1234";
            }
        }
    }
}
