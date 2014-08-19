using FileHelpers;
using AddressFinder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace AddressFinderConsole
{
    class PAFAddressImporter
    {
        readonly string _filename = ConfigurationManager.AppSettings["PAFFile"];
        public PAFAddressImporter(string filename)
        {
            _filename = filename;
        }
        private static PostalAddress Convert(PAFPostalAddressImport postalAddress)
        {
            var result = new PostalAddress();
            result.AddressType = postalAddress.AddressType;
            result.BoxBagLobbyName = ToTitleCase(postalAddress.BoxBagLobbyName);
            result.BoxBagNumber = postalAddress.BoxBagNumber;
            result.BuildingName = ToTitleCase(postalAddress.BuildingName);
            result.DeliveryServiceType = postalAddress.DeliveryServiceType;
            result.Floor = ToTitleCase(postalAddress.Floor);
            result.Id = postalAddress.Id;
            result.PostCode = postalAddress.PostCode;
            result.RDNumber = postalAddress.RDNumber;
            result.StreetAlpha = postalAddress.StreetAlpha;
            result.StreetDirection = ToTitleCase(postalAddress.StreetDirection);
            result.StreetName = string.Join(" ", string.Join("|", new string[] { ToTitleCase(postalAddress.StreetName), ToTitleCase(postalAddress.StreetType), ToTitleCase(postalAddress.StreetDirection) }).Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            result.StreetNumber = postalAddress.StreetNumber;
            result.StreetType = postalAddress.StreetType;
            result.SuburbName = ToTitleCase(postalAddress.SuburbName);
            result.TownCityMailTown = ToTitleCase(postalAddress.TownCityMailTown);
            result.UnitId = postalAddress.UnitId;
            result.UnitType = postalAddress.UnitType;
            result.AddressLine = new PostalAddressFormatter().Format(result).AddressOneLine;
            return result;
        }

        private static string ToTitleCase(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;
            if (string.Compare(value, "PO BOX", true) == 0)
            {
                return "PO Box";
            }
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLowerInvariant());
        }
        internal void Import()
        {
            var repo = new AddressRepository();
            using (var engine = new FileHelperAsyncEngine<PAFPostalAddressImport>())
            {
                engine.BeginReadFile(_filename);

                var swWrite = Stopwatch.StartNew();
                var processed = 0L;

                var postalAddresses = new List<PostalAddress>();
                foreach (PAFPostalAddressImport postalAddress in engine)
                {
                    postalAddresses.Add(Convert(postalAddress));
                    if (postalAddresses.Count == 100000)
                    {
                        processed += postalAddresses.Count;

                        repo.Write(postalAddresses);
                        postalAddresses.Clear();

                        Console.WriteLine("Processed {0} in {1}", processed, swWrite.Elapsed);
                        swWrite.Restart();
                    }
                }
                if (postalAddresses.Count > 0)
                {
                    repo.Write(postalAddresses);
                    processed += postalAddresses.Count;
                    Console.WriteLine("Processed {0} in {1}", processed, swWrite.Elapsed);
                }
            }
        }
    }
}
