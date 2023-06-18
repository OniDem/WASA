using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.IO.Ports;
using System.Threading.Tasks;

namespace ConsoleApp5
{
    internal class Program
    {


        static void Main(string[] args)
        {
            Console.WriteLine(GetMacAddress());
            Console.WriteLine();
            GetSerialPorts();
        }
        static string GetMacAddress()
        {
            string macAddresses = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces, thereby ignoring any
                // loopback devices etc.
                if (nic.NetworkInterfaceType != NetworkInterfaceType.Ethernet) continue;
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }
            return macAddresses;
        }

        static void GetSerialPorts()
        {
            string[] ports = SerialPort.GetPortNames();

            Console.WriteLine("The following serial ports were found:");

            // Display each port name to the console.
            foreach (string port in ports)
            {
                Console.WriteLine(port);
            }
        }
    }
}