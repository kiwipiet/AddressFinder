using NUnit.Framework;
using System;
using System.Linq;

namespace AddressFinder.Tests
{
    [TestFixture]
    public class BagPostalAddressFormatterTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException), ExpectedMessage = @"Invalid AddressType. Expected 'bag' but was 'rural'
Parameter name: postalAddress")]
        public void Invalid_AddressType_rural()
        {
            BagPostalAddressFormatter formatter = new BagPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress() { AddressType = "RURAL" };

            formatter.Format(postalAddress);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = @"postalAddress is null.
Parameter name: postalAddress")]
        public void NullPostalAddress_Expect_ArgumentNullException()
        {
            BagPostalAddressFormatter formatter = new BagPostalAddressFormatter();

            formatter.Format(null);
        }
        [Test]
        public void Format_Box_Address()
        {
            BagPostalAddressFormatter formatter = new BagPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress() { AddressType = "BAG" };

            formatter.Format(postalAddress);
        }
        [Test]
        public void Format_Box_Address_Set_AddressType()
        {
            BagPostalAddressFormatter formatter = new BagPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress() { AddressType = "BAG" };

            var format = formatter.Format(postalAddress);
            Assert.AreEqual("BAG", format.AddressType);
        }
        [Test]
        public void Box_Address()
        {
            BagPostalAddressFormatter formatter = new BagPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress()
            {
                AddressType = "BAG",
                BoxBagLobbyName = "Onehunga",
                BoxBagNumber = "999028",
                DeliveryServiceType = "Private Bag",
                PostCode = "1643",
                TownCityMailTown = "Auckland"
            };

            var format = formatter.Format(postalAddress);
            Assert.AreEqual("Private Bag 999028", format.AddressLine1);
            Assert.AreEqual("Onehunga", format.AddressLine2);
            Assert.AreEqual(string.Empty, format.AddressLine3);
            Assert.AreEqual(string.Empty, format.Suburb);
            Assert.AreEqual("Auckland", format.City);
            Assert.AreEqual("1643", format.PostCode);
        }
    }
}
