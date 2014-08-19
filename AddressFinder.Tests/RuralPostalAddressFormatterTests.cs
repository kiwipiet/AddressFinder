using NUnit.Framework;
using System;
using System.Linq;

namespace AddressFinder.Tests
{
    [TestFixture]
    public class RuralPostalAddressFormatterTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException), ExpectedMessage = @"Invalid AddressType. Expected 'rural' but was 'urban'
Parameter name: postalAddress")]
        public void Invalid_AddressType_rural()
        {
            RuralPostalAddressFormatter formatter = new RuralPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress() { AddressType = "URBAN" };

            formatter.Format(postalAddress);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = @"postalAddress is null.
Parameter name: postalAddress")]
        public void NullPostalAddress_Expect_ArgumentNullException()
        {
            RuralPostalAddressFormatter formatter = new RuralPostalAddressFormatter();

            formatter.Format(null);
        }
        [Test]
        public void Format_Urban_Address()
        {
            RuralPostalAddressFormatter formatter = new RuralPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress() { AddressType = "RURAL" };

            formatter.Format(postalAddress);
        }
        [Test]
        public void Format_Urban_Address_Set_AddressType()
        {
            RuralPostalAddressFormatter formatter = new RuralPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress() { AddressType = "RURAL" };

            var format = formatter.Format(postalAddress);
            Assert.AreEqual("RURAL", format.AddressType);
        }
        [Test]
        public void Rural_Street()
        {
            RuralPostalAddressFormatter formatter = new RuralPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress()
            {
                AddressType = "RURAL",
                PostCode = "9382",
                RDNumber = "2",
                StreetName = "Charles Court",
                StreetNumber = "6",
                StreetType = "Coart",
                TownCityMailTown = "Wanaka"
            };

            var format = formatter.Format(postalAddress);
            Assert.AreEqual("6 Charles Court", format.AddressLine1);
            Assert.AreEqual("RD 2", format.AddressLine2);
            Assert.AreEqual(string.Empty, format.AddressLine3);
            Assert.AreEqual("RD 2", format.Suburb);
            Assert.AreEqual("Wanaka", format.City);
            Assert.AreEqual("9382", format.PostCode);
        }
        [Test]
        public void Rural_Flat()
        {
            RuralPostalAddressFormatter formatter = new RuralPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress()
            {
                AddressType = "RURAL",
                PostCode = "0174",
                RDNumber = "4",
                StreetName = "Whangarei Heads Road",
                StreetNumber = "803",
                StreetType = "Road",
                TownCityMailTown = "Whangarei",
                UnitId = "19",
                UnitType = "UNIT"
            };

            var format = formatter.Format(postalAddress);
            Assert.AreEqual("19/803 Whangarei Heads Road", format.AddressLine1);
            Assert.AreEqual("RD 4", format.AddressLine2);
            Assert.AreEqual(string.Empty, format.AddressLine3);
            Assert.AreEqual("RD 4", format.Suburb);
            Assert.AreEqual("Whangarei", format.City);
            Assert.AreEqual("0174", format.PostCode);
        }
        [Test]
        public void Urban_Street_Suite_UnitID_AlphaNumeric()
        {
            RuralPostalAddressFormatter formatter = new RuralPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress()
            {
                AddressType = "RURAL",
                PostCode = "7691",
                RDNumber = "1",
                StreetName = "Williams Street",
                StreetNumber = "548",
                StreetType = "Street",
                TownCityMailTown = "Kaiapoi",
                UnitId = "18A",
                UnitType = "UNIT"
            };

            var format = formatter.Format(postalAddress);
            Assert.AreEqual("18A/548 Williams Street", format.AddressLine1);
            Assert.AreEqual("RD 1", format.AddressLine2);
            Assert.AreEqual(string.Empty, format.AddressLine3);
            Assert.AreEqual("RD 1", format.Suburb);
            Assert.AreEqual("Kaiapoi", format.City);
            Assert.AreEqual("7691", format.PostCode);
        }
    }
}