using System;
using GsCore.Database.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GsCore.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //#### If we are NOT seeding data     
            //CreateHostBuilder(args).Build().Run();


            //#### If we are trying to seed data             

            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<GsDbContext>();
                    DataSeeder.Seed(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occured while seeding the database with data");

                }
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
