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
            //Console.WriteLine(GetMacAddress());
            Process proc = new Process();
            proc.StartInfo.CreateNoWindow = true;
            proc.StartInfo.FileName = "cmd";

            proc.StartInfo.Arguments = @"/C ""arp 192.168.0.1 -a""";

            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.UseShellExecute = false;
            proc.Start();
            string output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();

            Console.WriteLine(output);
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