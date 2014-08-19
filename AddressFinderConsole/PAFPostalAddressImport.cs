using FileHelpers;
using System;
using System.Collections.Generic;

namespace AddressFinderConsole
{
    [DelimitedRecord("|"),
        IgnoreFirst(2), IgnoreLast(1)]
    public class PAFPostalAddressImport
    {
        public string ActionIndicator;

        /// <summary>
        /// Unique reference number
        /// </summary>
        public string Id;

        /// <summary>
        /// Indiciates the type of delivery point
        /// Urban, Rural, Bag, Box, Counter, CMB Urban, CMB Rural
        /// </summary>
        public string AddressType;

        /// <summary>
        /// Holds the base street number
        /// 1 - 100000
        /// </summary>
        public string StreetNumber;

        /// <summary>
        /// Holds the Alpha component of a Street Number
        /// A - Z
        /// </summary>
        public string StreetAlpha;

        /// <summary>
        /// Describes the category of a sub-dwelling - used in conjunction with the Unit_ID
        /// Apartment, Flat, Unit, Shop, Suite
        /// </summary>
        public string UnitType;

        /// <summary>
        /// Sub-Dwellling Identifier
        /// 32, BB2, Top, Penthouse, W506A
        /// </summary>
        public string UnitId;

        /// <summary>
        /// Floor Type and Identifier within a Building or Complex
        /// Level 1, Floor 3, Basement
        /// </summary>
        public string Floor;

        /// <summary>
        /// Building / Property name 
        /// </summary>
        public string BuildingName;

        public string StreetAliasId;
        /// <summary>
        /// Holds the Name of the Street
        /// </summary>
        public string StreetName;

        /// <summary>
        /// The Street Type that follows the Street Name
        /// Road, Crescent, Lane Character
        /// </summary>
        public string StreetType;

        /// <summary>
        /// Indicates where a road may be split or extended
        /// North, West, Extension Character
        /// </summary>
        public string StreetDirection;

        /// <summary>
        /// The Type of Delivery Service
        /// PO Box, Private Bag, Counter, CMB
        /// </summary>
        public string DeliveryServiceType;

        /// <summary>
        /// PO Box / Private Bag Number 
        /// </summary>
        public string BoxBagNumber;

        /// <summary>
        /// The Name of the NZPost outlet or Agency 
        /// </summary>
        public string BoxBagLobbyName;

        public string SuburbAliasId;
        public string SuburbName;

        public string TownCityMailTownAliasId;

        /// <summary>
        /// The Name of the Town / Mailtown associated with the Delivery point
        /// </summary>
        public string TownCityMailTown;

        /// <summary>
        /// The 4 digit code defined by NZPost for the sorting of mail
        /// </summary>
        public string PostCode;

        /// <summary>
        /// Rural Delivery route number
        /// </summary>
        public string RDNumber;

        public string OldPostCode;
        public string RecordUsageId;
    }
}
