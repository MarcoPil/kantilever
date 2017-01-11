using InfoSupport.WSA.Infrastructure;
using InfoSupport.WSA.Logging.Model;
using Kantilever.Voorbeeld.EventBusListener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kantilever.VoorbeeldAuditlogIntegratie
{
    public class Program
    {
        private static BusOptions busoptions;
        private static EventWaitHandle toStop = new AutoResetEvent(false);

        public static void Main(string[] args)
        {
            Configure();
            StartUp();

            KeepApplicationAlive();
        }

        private static void Configure()
        {
            busoptions = BusOptions.CreateFromEnvironment();
            Console.WriteLine("Configure Busoptions:");
            Console.WriteLine(busoptions);

            //Console.WriteLine("Configure Connection:");
            //Console.WriteLine(connectionString);

            //dbOptions = new DbContextOptionsBuilder<SomeContext>()
            //        .UseSqlServer(connectionString)
            //        .Options;
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

            //Console.WriteLine("Ensure Database is created...");
            //using (var context = new SomeContext(dbOptions))
            //{
            //    context.Database.EnsureCreated();
            //}

            var replayEndpoint = Environment.GetEnvironmentVariable("startup-replay-endpoint");
            if (replayEndpoint != null)
            {
                var replayBusOptions = new BusOptions
                {
                    ExchangeName = "Kantilever.Voorbeeld.ReplayExchange",
                    HostName = "localhost",
                    Port = 20000,
                    UserName = "Kantilever",
                    Password = "Kant1lever",
                };

                using (var listener = new EventListener(replayBusOptions))
                using (var auditlogproxy = new MicroserviceProxy(replayEndpoint, busoptions))
                {
                    listener.Open();

                    var replayCommand = new ReplayEventsCommand
                    {
                        ExchangeName = replayBusOptions.ExchangeName,
                        //RoutingKeyExpression = "Kantilever.#",
                    };

                    Console.WriteLine($"Start replaying Events on Exchange={replayCommand.ExchangeName}...");
                    auditlogproxy.Execute(replayCommand);
                    Console.WriteLine("Done replaying events.");
                }
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
