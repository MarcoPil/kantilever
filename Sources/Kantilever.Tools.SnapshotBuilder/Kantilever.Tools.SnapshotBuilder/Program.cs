using InfoSupport.WSA.Infrastructure;
using Kantilever.Catalogusbeheer.DatabaseApp;
using Kantilever.Magazijnbeheer.SnapshotBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Kantilever.Tools.SnapshotBuilder
{
    public class Program
    {
        private static BusOptions busoptions;
        private static EventWaitHandle toStop = new AutoResetEvent(false);

        public static void Main(string[] args)
        {
            Configure();
            StartUp();

            Console.WriteLine("Opening event publisher...");
            using (var publisher = new EventPublisher(busoptions))
            {
                Random rnd = new Random(42);

                foreach (var evt in CatalogusSnapshot.Events)
                {
                    publisher.Publish(evt);
                    Console.WriteLine($"published {evt.GetType().Name} event ({evt.Artikelnummer}, {evt.Naam}, ...)");
                    Thread.Sleep(rnd.Next(50, 100));
                }

                foreach (var evt in VoorraadSnapshot.Events)
                {
                    publisher.Publish(evt);
                    Console.WriteLine($"published {evt.GetType().Name} event ({evt.ArtikelID}, {evt.Voorraad})");
                    Thread.Sleep(rnd.Next(50, 100));
                }
            }
        }

        private static void Configure()
        {
            busoptions = BusOptions.CreateFromEnvironment();
            Console.WriteLine("Configure Busoptions:");
            Console.WriteLine(busoptions);
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
        }
    }

}
