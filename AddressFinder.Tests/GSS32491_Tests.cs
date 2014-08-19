using NUnit.Framework;
using System;
using System.Linq;

namespace AddressFinder.Tests
{
    [TestFixture]
    public class GSS32491_Tests :  AddressValidationTestBase
    {
        [Test]
        public void _87A_Park_Road_Katikat_Test()
        {
            AddressRepository repo = new AddressRepository();
            repo.GetExplanations = true;
            int hits = 0;
            var results = repo.Read(new AddressSearchQuery("87A park road kati"), out hits);
            ExplainResult(results);
            Assert.AreEqual("87A Park Road, Katikati 3129", results.First().Format.AddressOneLine);

        }
        [Test]
        public void _87B_Park_Road_Katikat_Test()
        {
            AddressRepository repo = new AddressRepository();
            repo.GetExplanations = true;
            int hits = 0;
            var results = repo.Read(new AddressSearchQuery("87B park road kati"), out hits);
            ExplainResult(results);
            Assert.AreEqual("87B Park Road, Katikati 3129", results.First().Format.AddressOneLine);

        }
    }
}
