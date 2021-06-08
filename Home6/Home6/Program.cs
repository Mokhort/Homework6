using Homework3;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Home6
{
    class Program
    {
        private static TcpListener listener;
        private const int serverPort = 2365;
        private static bool flag;
        static Context context = new Context();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Server start");
            listener = new TcpListener(IPAddress.Any, serverPort);
            flag = true;
            context.Database.EnsureCreated();
            await Listen();
        }
        private static async Task Listen()
        {
            listener.Start();
            TcpClient client = await listener.AcceptTcpClientAsync();
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);

            string msg = await reader.ReadLineAsync();
            string[] words = msg.Split(",");
            Console.WriteLine($"Message from client: name = {words[0]} lastname = {words[1]} email = {words[2]} password = {words[3]} passport = {words[4]}");
            IDbContextTransaction transaction = await context.Database.BeginTransactionAsync();
            try
            {

                context.Passanger.Add(new Passanger()
                {
                    Name = words[0],
                    LastName = words[1],
                    Email = words[2],
                    Password = words[3],
                    Passport = words[4]
                });
                await transaction.CommitAsync();
                await context.SaveChangesAsync();
                await writer.WriteLineAsync("Your registration ended successfully");
                await writer.FlushAsync();
            }
            catch
            {
                await writer.WriteLineAsync("FAILED REGISTRATION");
            }
            listener.Stop();
            client.Close();
        }
    }
}
