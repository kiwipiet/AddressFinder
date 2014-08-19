using FileHelpers;
using System;
using System.Collections.Generic;

namespace AddressFinderConsole
{
    [DelimitedRecord(","),
    IgnoreFirst(1)]
    public class REDPostalAddressImport
    {
        /// <summary>
        /// Unique reference number
        /// </summary>
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string Id;

        /// <summary>
        /// Indiciates the type of delivery point
        /// Urban, Rural, Bag, Box, Counter, CMB Urban, CMB Rural
        /// </summary>
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string AddressType;

        /// <summary>
        /// Holds the base street number
        /// 1 - 100000
        /// </summary>
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string StreetNumber;

        /// <summary>
        /// Holds the Alpha component of a Street Number
        /// A - Z
        /// </summary>
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string StreetAlpha;

        /// <summary>
        /// Describes the category of a sub-dwelling - used in conjunction with the Unit_ID
        /// Apartment, Flat, Unit, Shop, Suite
        /// </summary>
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string UnitType;

        /// <summary>
        /// Sub-Dwellling Identifier
        /// 32, BB2, Top, Penthouse, W506A
        /// </summary>
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string UnitId;

        /// <summary>
        /// Floor Type and Identifier within a Building or Complex
        /// Level 1, Floor 3, Basement
        /// </summary>
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string Floor;

        /// <summary>
        /// Building / Property name 
        /// </summary>
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string BuildingName;

        /// <summary>
        /// Holds the Name of the Street
        /// </summary>
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string StreetName;

        /// <summary>
        /// The Street Type that follows the Street Name
        /// Road, Crescent, Lane Character
        /// </summary>
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string StreetType;

        /// <summary>
        /// Indicates where a road may be split or extended
        /// North, West, Extension Character
        /// </summary>
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string StreetDirection;

        /// <summary>
        /// The Type of Delivery Service
        /// PO Box, Private Bag, Counter, CMB
        /// </summary>
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string DeliveryServiceType;

        /// <summary>
        /// PO Box / Private Bag Number 
        /// </summary>
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string BoxBagNumber;

        /// <summary>
        /// The Name of the NZPost outlet or Agency 
        /// </summary>
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string BoxBagLobbyName;

        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string SuburbName;

        /// <summary>
        /// The Name of the Town / Mailtown associated with the Delivery point
        /// </summary>
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string TownCityMailTown;

        /// <summary>
        /// The 4 digit code defined by NZPost for the sorting of mail
        /// </summary>
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string PostCode;

        /// <summary>
        /// Rural Delivery route number
        /// </summary>
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.NotAllow)]
        public string RDNumber;
    }
}
