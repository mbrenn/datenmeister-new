using System;
using Microsoft.Owin.Hosting;

namespace DatenMeister.Web.Application
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<WebserverStartup>("http://localhost:8080"))
            {
                //AreaRegistration.RegisterAllAreas();
                Console.WriteLine("Running on http://localhost:8080");
                Console.WriteLine("Press key to stop the server");
                Console.ReadLine();
            }
        }
    }
}
