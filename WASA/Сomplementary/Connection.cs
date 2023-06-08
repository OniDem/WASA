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
        public static string GetConnectionString()
        {
            var macAddr = (from nic in NetworkInterface.GetAllNetworkInterfaces() where nic.OperationalStatus == OperationalStatus.Up select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
            

            if (Properties.Resources.mac == macAddr)
            {
                return $"Host=localhost;Port=5432;DataBase=wasa;Username=postgres;Password=1234";
            }
            else
            {
                return $"Host='5.137.212.65';Port=5432;DataBase=wasa;Username=postgres;Password=1234";
            }
        }
    }
}
