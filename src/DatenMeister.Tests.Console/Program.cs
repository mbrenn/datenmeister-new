using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatenMeister.Tests.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var property = new object();
            System.Console.WriteLine("DatenMeister Testing");

            var element = new MOF.InMemory.MofElement();
            element.set(property, "Test");
        }
    }
}
