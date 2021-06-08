using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Home6_client
{
    class Program
    {
        private const int serverPort = 2365;
        private static TcpClient client;
        static async Task Main(string[] args)
        {
            string cmd;
            string answer;
            bool flag = true;
            string message;
            client = new TcpClient();
            await client.ConnectAsync(IPAddress.Loopback, serverPort);
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);
            

            while (flag)
            {
                Console.WriteLine("Registration - 1 Exit - 0 ");
                cmd = Console.ReadLine();
                if (cmd == "1") {
                    Console.Write("Enter name: ");
                    string name = Console.ReadLine();
                    Console.Write("Enter last name: ");
                    string lastname = Console.ReadLine();
                    Console.Write("Enter email: ");
                    string email = Console.ReadLine();
                    Console.Write("Enter password: ");
                    string password = Console.ReadLine();
                    Console.Write("Enter passport: ");
                    string passport = Console.ReadLine();
                    message = name + ','+ lastname+ ',' + email + ',' + password + ',' + passport;
                    Console.WriteLine(message);
                    await writer.WriteLineAsync(message);
                    await writer.FlushAsync();
                    answer = await reader.ReadLineAsync();
                    Console.WriteLine(answer);
                }
                else if (cmd == "0")
                {
                    flag = false;
                }
                else Console.WriteLine("Invalid value");

            }
        }
    }
}
