using NUnit.Framework;
using System;
using System.Linq;

namespace AddressFinder.Tests
{
    [TestFixture]
    public class UrbanPostalAddressFormatterTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException), ExpectedMessage = @"Invalid AddressType. Expected 'urban' but was 'rural'
Parameter name: postalAddress")]
        public void Invalid_AddressType_rural()
        {
            UrbanPostalAddressFormatter formatter = new UrbanPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress() { AddressType = "RURAL" };

            formatter.Format(postalAddress);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = @"postalAddress is null.
Parameter name: postalAddress")]
        public void NullPostalAddress_Expect_ArgumentNullException()
        {
            UrbanPostalAddressFormatter formatter = new UrbanPostalAddressFormatter();

            formatter.Format(null);
        }
        [Test]
        public void Format_Urban_Address()
        {
            UrbanPostalAddressFormatter formatter = new UrbanPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress() { AddressType = "URBAN" };

            formatter.Format(postalAddress);
        }
        [Test]
        public void Format_Urban_Address_Set_AddressType()
        {
            UrbanPostalAddressFormatter formatter = new UrbanPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress() { AddressType = "URBAN" };

            var format = formatter.Format(postalAddress);
            Assert.AreEqual("URBAN", format.AddressType);
        }
        [Test]
        public void Urban_Street()
        {
            UrbanPostalAddressFormatter formatter = new UrbanPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress() 
            {
                AddressType = "URBAN",
                PostCode = "6011",
                StreetName = "Manners Street", 
                StreetNumber = "2",
                StreetType = "Street",
                SuburbName = "Te Aro",
                TownCityMailTown = "Wellington"
            };

            var format = formatter.Format(postalAddress);
            Assert.AreEqual("2 Manners Street", format.AddressLine1);
            Assert.AreEqual("Te Aro", format.AddressLine2);
            Assert.AreEqual(string.Empty, format.AddressLine3);
            Assert.AreEqual("Te Aro", format.Suburb);
            Assert.AreEqual("Wellington", format.City);
            Assert.AreEqual("6011", format.PostCode);
        }
        [Test]
        public void Urban_Street_Flat()
        {
            UrbanPostalAddressFormatter formatter = new UrbanPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress()
            {
                AddressType = "URBAN",
                PostCode = "6011",
                StreetName = "Manners Street",
                StreetNumber = "15",
                StreetType = "Street",
                SuburbName = "Te Aro",
                TownCityMailTown = "Wellington",
                UnitId = "1",
                UnitType = "FLAT"
            };

            var format = formatter.Format(postalAddress);
            Assert.AreEqual("1/15 Manners Street", format.AddressLine1);
            Assert.AreEqual("Te Aro", format.AddressLine2);
            Assert.AreEqual(string.Empty, format.AddressLine3);
            Assert.AreEqual("Te Aro", format.Suburb);
            Assert.AreEqual("Wellington", format.City);
            Assert.AreEqual("6011", format.PostCode);
        }
        [Test]
        public void Urban_Street_Suite()
        {
            UrbanPostalAddressFormatter formatter = new UrbanPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress()
            {
                AddressType = "URBAN",
                BuildingName = "Mid City Complex",
                PostCode = "6011",
                StreetName = "Manners Street",
                StreetNumber = "18",
                StreetType = "Street",
                SuburbName = "Te Aro",
                TownCityMailTown = "Wellington",
                UnitId = "3",
                UnitType = "SUITE"
            };

            var format = formatter.Format(postalAddress);
            Assert.AreEqual("3/18 Manners Street", format.AddressLine1);
            Assert.AreEqual("Te Aro", format.AddressLine2);
            Assert.AreEqual(string.Empty, format.AddressLine3);
            Assert.AreEqual("Te Aro", format.Suburb);
            Assert.AreEqual("Wellington", format.City);
            Assert.AreEqual("6011", format.PostCode);
        }
        [Test]
        public void Urban_Street_Suite_UnitID_AlphaNumeric()
        {
            UrbanPostalAddressFormatter formatter = new UrbanPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress()
            {
                AddressType = "URBAN",
                PostCode = "6011",
                StreetName = "Manners Street",
                StreetNumber = "125",
                StreetType = "Street",
                SuburbName = "Te Aro",
                TownCityMailTown = "Wellington",
                UnitId = "3A",
                UnitType = "SUITE"
            };

            var format = formatter.Format(postalAddress);
            Assert.AreEqual("3A/125 Manners Street", format.AddressLine1);
            Assert.AreEqual("Te Aro", format.AddressLine2);
            Assert.AreEqual(string.Empty, format.AddressLine3);
            Assert.AreEqual("Te Aro", format.Suburb);
            Assert.AreEqual("Wellington", format.City);
            Assert.AreEqual("6011", format.PostCode);
        }
        [Test]
        public void Urban_Street_Numeric_Unit_UnitID_Alpha()
        {
            UrbanPostalAddressFormatter formatter = new UrbanPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress()
            {
                AddressType = "URBAN",
                PostCode = "1050",
                StreetName = "Buttle Street",
                StreetNumber = "15",
                StreetType = "Street",
                SuburbName = "Remuera",
                TownCityMailTown = "Auckland",
                UnitId = "A",
                UnitType = "UNIT"
            };

            var format = formatter.Format(postalAddress);
            Assert.AreEqual("15A Buttle Street", format.AddressLine1);
            Assert.AreEqual("Remuera", format.AddressLine2);
            Assert.AreEqual(string.Empty, format.AddressLine3);
            Assert.AreEqual("Remuera", format.Suburb);
            Assert.AreEqual("Auckland", format.City);
            Assert.AreEqual("1050", format.PostCode);
        }
    }
}