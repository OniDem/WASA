using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.IO.Ports;
using System.Threading.Tasks;
using System.Diagnostics;

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
            var macAddr = (from nic in NetworkInterface.GetAllNetworkInterfaces() where nic.OperationalStatus == OperationalStatus.Up select nic.GetPhysicalAddress().ToString()).FirstOrDefault();
            return macAddr!;
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