using FileHelpers;
using AddressFinder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AddressFinderConsole
{
    internal class REDAddressImporter
    {
        string _fileName = @"O:\Technology\Suppliers\NZPost\RED_BASE_September_2013.txt";
        public REDAddressImporter(string fileName)
        {
            _fileName = fileName;
        }
        private static PostalAddress Convert(REDPostalAddressImport postalAddress)
        {
            var result = new PostalAddress();
            result.AddressType = postalAddress.AddressType;
            result.BoxBagLobbyName = postalAddress.BoxBagLobbyName;
            result.BoxBagNumber = postalAddress.BoxBagNumber;
            result.BuildingName = postalAddress.BuildingName;
            result.DeliveryServiceType = postalAddress.DeliveryServiceType;
            result.Floor = GetFloor(postalAddress.Floor);
            result.Id = postalAddress.Id;
            result.PostCode = postalAddress.PostCode;
            result.RDNumber = postalAddress.RDNumber;
            result.StreetAlpha = postalAddress.StreetAlpha;
            result.StreetDirection = postalAddress.StreetDirection;
            result.StreetName = postalAddress.StreetName;
            result.StreetNumber = postalAddress.StreetNumber;
            result.StreetType = postalAddress.StreetType;
            result.SuburbName = postalAddress.SuburbName;
            result.TownCityMailTown = postalAddress.TownCityMailTown;
            result.UnitId = postalAddress.UnitId;
            result.UnitType = postalAddress.UnitType;
            result.AddressLine = new PostalAddressFormatter().Format(result).AddressOneLine;
            return result;
        }

        private static string GetFloor(string floor)
        {
            if (string.IsNullOrWhiteSpace(floor))
                return string.Empty;
            return string.Format("Floor {0}", floor);
        }

        internal void Import()
        {
            var repo = new AddressRepository();
            using (var engine = new FileHelperAsyncEngine<REDPostalAddressImport>())
            {
                engine.BeginReadFile(_fileName);

                var swWrite = Stopwatch.StartNew();
                var processed = 0L;

                var postalAddresses = new List<PostalAddress>();
                foreach (REDPostalAddressImport postalAddress in engine)
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
