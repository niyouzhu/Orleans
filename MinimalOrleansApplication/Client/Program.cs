using GrainInterfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using System;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static int Main(string[] args)
        {
            return RunMainAsync().Result;
        }

        private async static Task<int> RunMainAsync()
        {
            try
            {
                using (var client = await ConnectClient())
                {
                    await DoClientWork(client);
                    Console.ReadLine();
                }
                return 0;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }
        }

        private static Task<IClusterClient> CreateClient()
        {
            var builder = new ClientBuilder();
            var client = builder.UseLocalhostClustering()
                   .Configure<ClusterOptions>(options => { options.ClusterId = "dev"; options.ServiceId = "Server"; })
                   .ConfigureLogging(logging => logging.AddConsole())
                   .Build();
            return Task.FromResult(client);
        }

        private async static Task<IClusterClient> ConnectClient()
        {
            var client = await CreateClient();
            await client.Connect();
            Console.WriteLine("Client successfully connected to silo host");
            return client;
        }

        private async static Task DoClientWork(IClusterClient client)
        {
            var friend = client.GetGrain<IHello>(0);
            var response = await friend.SayHello("Client is comming");
            Console.WriteLine($"Response from Server: {response}");
        }
    }
}
