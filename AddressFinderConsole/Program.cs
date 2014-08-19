using CLAP;
using System;
using System.Diagnostics;

namespace AddressFinderConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var sw = Stopwatch.StartNew();

            Parser.Run<AddressValidationConsoleRunner>(args);

            Console.WriteLine("Runtime: {0}", sw.Elapsed);
            Console.WriteLine("Done. Press any key to continue.");
            Console.ReadKey();
        }
    }
}
