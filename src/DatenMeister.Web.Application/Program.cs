using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using Owin;

namespace DatenMeister.Web.Application
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<WebserverStartup>("http://localhost:8080"))
            {
                Console.WriteLine("Running on http://localhost:8080");
                Console.WriteLine("Press key to stop the server");
                Console.ReadLine();
            }
        }
    }
}
