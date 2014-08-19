using NUnit.Framework;
using System.Linq;

namespace AddressFinder.Tests
{
    [TestFixture]
    public class SearchTermsTests
    {
        [Test]
        public void TestAddress_4_29Waiapu_Road_Kelburn()
        {
            SearchTerms terms = new SearchTerms("4/29 Waiapu Road, Kelburn, Wellington 6012");
            Assert.AreEqual(7, terms.Count);
        }
        [Test]
        public void TestAddress_Distinct_88()
        {
            SearchTerms terms = new SearchTerms("8/8 Danville");
            Assert.AreEqual(2, terms.Count);
        }
    }
}
