using CLAP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AddressFinderConsole
{
    internal class AddressValidationConsoleRunner
    {
        public static void RED([Required]string filename)
        {
            new REDAddressImporter(filename).Import();
        }
        public static void PAF([Required]string filename)
        {
            new PAFAddressImporter(filename).Import();
        }
    }
}
