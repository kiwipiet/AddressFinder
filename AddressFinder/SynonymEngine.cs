using Lucene.Net.Analysis;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Humanizer;

namespace AddressFinder
{
    public class SynonymEngine : ISynonymEngine
    {
        public class Fields
        {
            public static readonly string Recipient = "Recipient";
            public static readonly string UnitType = "UnitType";
            public static readonly string FloorType = "FloorType";
            public static readonly string BuildingName = "BuildingName";
            public static readonly string Prefix = "Prefix";
            public static readonly string Organisation = "Organisation";
            public static readonly string StreetDirection = "StreetDirection";
            public static readonly string StreetType = "StreetType";
            public static readonly string Ordinal = "StreetType";
        }
        private readonly static IEnumerable<CharArraySet> _synonymGroups = GetSynonymGroups();

        private static IEnumerable<CharArraySet> GetSynonymGroups()
        {
            var abbreviations = new Collection<CharArraySet>();
            GetRecipientAbbreviations(abbreviations);
            GetUnitTypeAbbreviations(abbreviations);
            GetFloorTypeAbbreviations(abbreviations);
            GetBuildingNameAbbreviations(abbreviations);
            GetPrefixAbbreviations(abbreviations);
            GetOrganisationAbbreviations(abbreviations);
            GetStreetDirectionAbbreviations(abbreviations);
            GetStreetTypeAbbreviations(abbreviations);
            GetOrdinalAbbreviations(abbreviations);
            return abbreviations;
        }
        private static void GetOrdinalAbbreviations(Collection<CharArraySet> abbreviations)
        {
            for (int i = 0; i < 100; i++)
            {
                abbreviations.Add(new CharArraySet(new Collection<string>() { i.Ordinal(), i.ToOrdinalWords() }, true));
            }
        }

        private static void GetUnitTypeAbbreviations(ICollection<CharArraySet> abbreviations)
        {
            abbreviations.Add(new CharArraySet(new Collection<string>() {"Apartment", "Apt"}, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Kiosk", "Ksk" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Room", "Rm" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Shop", "Shp" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Suite", "Ste" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Villa", "Vlla" }, true));
        }

        private static void GetFloorTypeAbbreviations(ICollection<CharArraySet> abbreviations)
        {
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Floor", "Fl" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Level", "L" }, true));
        }

        private static void GetRecipientAbbreviations(ICollection<CharArraySet> abbreviations)
        {
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Attention", "Attn" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Care of", "C/-", "C/O" }, true));
        }

        private static void GetBuildingNameAbbreviations(ICollection<CharArraySet> abbreviations)
        {
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Building", "Bldg" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "House", "Hse" }, true));
        }

        private static void GetPrefixAbbreviations(ICollection<CharArraySet> abbreviations)
        {
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Mount", "Mt" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Saint", "St" }, true));
        }

        private static void GetOrganisationAbbreviations(ICollection<CharArraySet> abbreviations)
        {
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Administration", "Admn" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Agency", "Agcy" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Branch", "Brnch,", "Br" }, true));
            //abbreviations.Add(new CharArraySet(new Collection<string>() { "Care of", "C/-,", "C/O" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Centre", "Ctr" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Company", "Co" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Corporation", "Corp" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Department", "Dept" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Division", "Div" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Enterprise", "Entprs" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Government", "Govt" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Group", "Gp" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Headquarters", "HQ" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Incorporated", "Inc" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Laboratory", "Lab" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Limited", "Ltd" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Management", "Mgmt" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Manufacturer", "Mfr" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Manufacturing", "Mfg" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "National", "Natl" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Office", "Ofc" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Partnership", "Prtnrshp" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Proprietary", "Pty" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "System", "Sys" }, true));
        }
        private static void GetStreetDirectionAbbreviations(ICollection<CharArraySet> abbreviations)
        {
            abbreviations.Add(new CharArraySet(new Collection<string>() { "North", "N" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "East", "E" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "South", "S" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "West", "W" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Northeast", "NE" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Southeast", "SE" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Northwest", "NW" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Southwest", "SW" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Upper", "Upr" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Lower", "Lwr" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Central", "Ctrl" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Extension", "Ext" }, true));
        }

        private static void GetStreetTypeAbbreviations(ICollection<CharArraySet> abbreviations)
        {
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Access", "Accs" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Avenue", "Ave" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Bank", "Bank" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Bay", "Bay" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Beach", "Bch" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Belt", "Belt" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Bend", "Bnd" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Boulevard", "Blvd" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Brae", "Brae" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Centre", "Ctr" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Circle", "Cir" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Circus", "Crcs" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Close", "Cl" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Common", "Cmn" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Leader", "Ledr" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Leigh", "Lgh" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Mount", "Mt" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Paku", "Paku" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Parade", "Pde" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Park", "Pk" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Place", "Pl" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Cove", "Cv" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Promenade", "Prom" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Crescent", "Cres" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Quay", "Qy" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Crest", "Crst" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Ridge", "Rdge" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Dale", "Dle" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Dell", "Del" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Road", "Rd" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Drive Dr" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Square", "Sq" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Esplanade", "Esp" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Strand", "Strd" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Fairway", "Fawy" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Street", "St" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Gardens", "Gdns" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Terrace", "Tce" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Gate", "Gte" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Track", "Trk" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Glade", "Gld" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Glen", "Gln" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Valley", "Vly" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Green", "Grn" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "View", "Vw" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Grove", "Grv" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Views", "Vws" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Village Vlg" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Heights Hts" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Villas", "Vlls" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Highway", "Hwy" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Vista", "Vis" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Hill", "Hl" }, true));
            abbreviations.Add(new CharArraySet(new Collection<string>() { "Walk", "Wlk" }, true));
        }

        public CharArraySet GetSynonyms(char[] word, int len)
        {
            if (word == null || word .Length == 0)
                return null;

            foreach (var synonymGroup in _synonymGroups)
            {
                //if the word is a part of the group return 
                //the group as the results.
                if (synonymGroup.Contains(word, 0, len))
                {
                    return synonymGroup;
                }
            }

            return null;
        }
    }
    internal static class OrdinalExtensions
    {
        public static string Ordinal(this int number)
        {
            const string TH = "th";
            string s = number.ToString();

            // Negative and zero have no ordinal representation
            if (number < 1)
            {
                return s;
            }

            number %= 100;
            if ((number >= 11) && (number <= 13))
            {
                return s + TH;
            }

            switch (number % 10)
            {
                case 1: return s + "st";
                case 2: return s + "nd";
                case 3: return s + "rd";
                default: return s + TH;
            }
        }
    }
}
