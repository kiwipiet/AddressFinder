using NUnit.Framework;
using System.Linq;

namespace AddressFinder.Tests
{
    [TestFixture]
    public class AbbreviationEngine_Tests
    {
        [Test]
        public void UnitType_Apartment()
        {
            SynonymEngine engine = new SynonymEngine();
            char[] term = "Apartment".ToCharArray();
            var result = engine.GetSynonyms(term, term.Length);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }
        [Test]
        public void UnitType_Apt()
        {
            SynonymEngine engine = new SynonymEngine();
            char[] term = "Apt".ToCharArray();
            var result = engine.GetSynonyms(term, term.Length);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }
        [Test]
        public void UnitType_Apartment_lowercase()
        {
            SynonymEngine engine = new SynonymEngine();
            char[] term = "Apartment".ToLowerInvariant().ToCharArray();
            var result = engine.GetSynonyms(term, term.Length);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }
        [Test]
        public void Crap()
        {
            SynonymEngine engine = new SynonymEngine();
            char[] term = "Crap".ToCharArray();
            var result = engine.GetSynonyms(term, term.Length);
            Assert.IsNull(result);
        }
        [Test]
        public void Null()
        {
            SynonymEngine engine = new SynonymEngine();
            var result = engine.GetSynonyms(null, 0);
            Assert.IsNull(result);
        }
    }
}