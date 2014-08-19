using NUnit.Framework;
using System;

namespace AddressFinder.Tests
{
    [TestFixture]
    public class BoxPostalAddressFormatterTests
    {
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException), ExpectedMessage = @"Invalid AddressType. Expected 'box' but was 'rural'
Parameter name: postalAddress")]
        public void Invalid_AddressType_rural()
        {
            BoxPostalAddressFormatter formatter = new BoxPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress() { AddressType = "RURAL" };

            formatter.Format(postalAddress);
        }
        [Test]
        [ExpectedException(typeof(ArgumentNullException), ExpectedMessage = @"postalAddress is null.
Parameter name: postalAddress")]
        public void NullPostalAddress_Expect_ArgumentNullException()
        {
            BoxPostalAddressFormatter formatter = new BoxPostalAddressFormatter();

            formatter.Format(null);
        }
        [Test]
        public void Format_Box_Address()
        {
            BoxPostalAddressFormatter formatter = new BoxPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress() { AddressType = "BOX" };

            formatter.Format(postalAddress);
        }
        [Test]
        public void Format_Box_Address_Set_AddressType()
        {
            BoxPostalAddressFormatter formatter = new BoxPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress() { AddressType = "BOX" };

            var format = formatter.Format(postalAddress);
            Assert.AreEqual("BOX", format.AddressType);
        }
        [Test]
        public void Box_Address()
        {
            BoxPostalAddressFormatter formatter = new BoxPostalAddressFormatter();
            PostalAddress postalAddress = new PostalAddress()
            {
                AddressType = "BOX",
                BoxBagLobbyName = "Manners Street",
                BoxBagNumber = "24001",
                DeliveryServiceType = "PO Box",
                PostCode = "6142",
                TownCityMailTown = "Wellington"
            };

            var format = formatter.Format(postalAddress);
            Assert.AreEqual("PO Box 24001", format.AddressLine1);
            Assert.AreEqual("Manners Street", format.AddressLine2);
            Assert.AreEqual(string.Empty, format.AddressLine3);
            Assert.AreEqual(string.Empty, format.Suburb);
            Assert.AreEqual("Wellington", format.City);
            Assert.AreEqual("6142", format.PostCode);
        }
    }
}