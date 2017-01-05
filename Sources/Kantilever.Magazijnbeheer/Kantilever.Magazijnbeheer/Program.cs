using InfoSupport.WSA.Infrastructure;
using Kantilever.Magazijnbeheer.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kantilever.Magazijnbeheer
{
    public class Program
    {
        private static BusOptions busoptions;
        private static DbContextOptions<MagazijnContext> dbOptions;
        private static EventWaitHandle toStop = new AutoResetEvent(false);

        public static void Main(string[] args)
        {
            Configure();
            StartUp();

            Console.WriteLine("Opening event publisher...");
            using (var publisher = new EventPublisher(busoptions))
            {
                var magazijnService = new MagazijnService(dbOptions, publisher);

                Console.WriteLine("Opening Magazijnbeheer Service...");
                using (var host = new MicroserviceHost<MagazijnService>(magazijnService, busoptions))
                {
                    host.Open();
                    Console.WriteLine("Magazijnbeheer is open for commands...");

                    KeepApplicationAlive();
                }
            }
        }

        private static void Configure()
        {
            busoptions = BusOptions.CreateFromEnvironment();
            Console.WriteLine("Configure Busoptions:");
            Console.WriteLine(busoptions);

            string connectionString = Environment.GetEnvironmentVariable("MagazijnDB");
            Console.WriteLine("Configure Connection:");
            Console.WriteLine(connectionString);

            dbOptions = new DbContextOptionsBuilder<MagazijnContext>()
                    .UseSqlServer(connectionString)
                    .Options;
        }

        private static void StartUp()
        {
            var delay = Environment.GetEnvironmentVariable("startup-delay-in-seconds");
            if (delay != null)
            {
                var startupDelay = 0;
                if (int.TryParse(delay, out startupDelay))
                {
                    Console.WriteLine($"Delay start-up for {startupDelay} seconds ...");
                    Thread.Sleep(startupDelay * 1000);
                }
            }

            Console.WriteLine("Ensure Database is created...");
            using (var context = new MagazijnContext(dbOptions))
            {
                context.Database.EnsureCreated();
            }
        }

        private static void KeepApplicationAlive()
        {
#if DEBUG
            Console.WriteLine("(Press any key to quit)");
            Console.ReadKey();
#else
            toStop.WaitOne();
#endif
        }

        public static void Stop()
        {
            toStop.Set();
        }
    }
}
