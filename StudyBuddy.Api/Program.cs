using System.IO;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace StudyBuddy.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GoogleCredential credentials;
            if (File.Exists("/conf/private_key.json"))
                credentials = GoogleCredential.FromFile("/conf/private_key.json");
            else
                credentials = GoogleCredential.FromFile("./private_key.json");

            FirebaseApp.Create(new AppOptions
            {
                Credential = credentials
            });

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}