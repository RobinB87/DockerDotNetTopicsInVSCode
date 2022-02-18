using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;

namespace CarvedRock.InvoiceGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var _config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName)
                .AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .Build();

            ConfigureLogging();

            try
            {
                var connectionString = _config.GetConnectionString("Db");

                Log.ForContext("ConnectionString", connectionString)
                    .Information("Loaded config!", connectionString);

                Log.ForContext("Args", args)
                   .Information("Starting program...");

                Console.WriteLine("Hello World!"); // do some invoice generation

                Log.Information("Finished execution!");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Some kind of exception occurred.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureLogging()
        {
            var name = typeof(Program).Assembly.GetName().Name;

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Assembly", name)
                .WriteTo.Console()
                .WriteTo.Seq("http://host.docker.internal:5341")
                .CreateLogger();
        }
    }
}
