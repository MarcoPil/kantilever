using InfoSupport.WSA.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kantilever.Magazijnbeheer.ConsoleApp
{
    public class Program
    {
        private static BusOptions busOptions;

        public static void Main(string[] args)
        {
            Configure();

            using (var proxy = new MicroserviceProxy("Kantilever.Magazijnbeheer", busOptions))
            {
                int aantal = 1;
                bool doContinue = true;
                do
                {
                    if (aantal > 0)
                    {
                        var command = new ZetArtikelInMagazijnCommand { ArtikelID = 3, Aantal = aantal };
                        Console.WriteLine($"Sending ZetArtikelInMagazijn command: {command.Aantal}");
                        proxy.Execute(command);
                        Console.WriteLine("Command has sent");
                    }
                    else
                    {
                        var command = new HaalArtikelUitMagazijnCommand { ArtikelID = 3, Aantal = -aantal };
                        Console.WriteLine($"Sending HaalArtikelUitMagazijn command: {command.Aantal}");
                        proxy.Execute(command);
                        Console.WriteLine("Command has sent");
                    }

                    Console.Write("Geef aantal:");
                    var line = Console.ReadLine();
                    doContinue = int.TryParse(line, out aantal);
                } while (doContinue);
            }
        }

        private static void Configure()
        {
            busOptions = BusOptions.CreateFromEnvironment();
            Console.WriteLine("Configure Busoptions:");
            Console.WriteLine(busOptions);
        }
    }
}
