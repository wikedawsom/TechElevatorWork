using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Capstone;

namespace NPDBCommandLineInterface
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            string connectionString = configuration.GetConnectionString("Project");
            
            ParkMenu parkMenu = new ParkMenu();
            parkMenu.MainMenu(connectionString);

            Console.WriteLine("Program ended, press a key to exit...");
            Console.ReadKey();
        }
    }
}
