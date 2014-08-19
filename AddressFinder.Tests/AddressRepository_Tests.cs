using NUnit.Framework;
using System.Linq;

namespace AddressFinder.Tests
{
    [TestFixture]
    public class AddressRepository_Tests : AddressValidationTestBase
    {
        [Test]
        public void One_Term()
        {
            AddressRepository repo = new AddressRepository();
            repo.GetExplanations = true;
            int hits = 0;
            var results = repo.Read(new AddressSearchQuery("Poto"), out hits);
            ExplainResult(results);
            Assert.AreEqual(25, results.Count());
        }
        [Test]
        public void Full_StreetName()
        {
            AddressRepository repo = new AddressRepository();
            repo.GetExplanations = true;
            int hits = 0;
            var results = repo.Read(new AddressSearchQuery("8 St Elizabeth Heights"), out hits);
            ExplainResult(results);
            Assert.AreEqual(25, results.Count());
            Assert.AreEqual(1000, hits);
            Assert.AreEqual("St Elizabeth Heights", results.First().StreetName);
            Assert.AreEqual("8", results.First().StreetNumber);
        }
        [Test]
        public void Full_StreetName_PostCode()
        {
            AddressRepository repo = new AddressRepository();
            repo.GetExplanations = true; 
            int hits = 0;
            var results = repo.Read(new AddressSearchQuery("8 St Elizabeth Heights 5010"), out hits);
            ExplainResult(results);
            Assert.AreEqual(25, results.Count());
            Assert.AreEqual(1000, hits);
            Assert.AreEqual("St Elizabeth Heights", results.First().StreetName);
            Assert.AreEqual("8", results.First().StreetNumber);
        }
        //Suite 3 Floor 6, 70 Hobson Street, Thorndon, Wellington 6011
        [Test]
        public void Suite_Floor_Street_70Hobson()
        {
            AddressRepository repo = new AddressRepository();
            repo.GetExplanations = true;
            int hits = 0;
            var results = repo.Read(new AddressSearchQuery("Suite 3 Floor 6, 70 Hobson Street, Thorndon, Wellington 6011"), out hits);
            ExplainResult(results);
            Assert.AreEqual("Suite 6 Floor 3, 70 Hobson Street, Thorndon, Wellington 6011", results.First().Format.AddressOneLine);
        }
        [Test]
        public void Suite_Floor_Street_36_70Hobson()
        {
            AddressRepository repo = new AddressRepository();
            repo.GetExplanations = true;
            int hits = 0;
            var results = repo.Read(new AddressSearchQuery("3/6 70 Hobson Street"), out hits);
            ExplainResult(results);
            Assert.AreEqual("Suite 6 Floor 3, 70 Hobson Street, Thorndon, Wellington 6011", results.First().Format.AddressOneLine);
        }
        //4/29 Waiapu Road, Kelburn, Wellington 6012
        [Test]
        public void Road_4_29Waiapu_Road_Kelburn()
        {
            AddressRepository repo = new AddressRepository();
            repo.GetExplanations = true;
            int hits = 0;
            var results = repo.Read(new AddressSearchQuery("4/29 Waiapu Road, Kelburn, Wellington 6012"), out hits);
            ExplainResult(results);
            Assert.AreEqual("4/29 Waiapu Road, Kelburn, Wellington 6012", results.First().Format.AddressOneLine);
        }
        [Test]
        public void Road_15_voss()
        {
            AddressRepository repo = new AddressRepository();
            repo.GetExplanations = true;
            int hits = 0;
            var results = repo.Read(new AddressSearchQuery("15 voss"), out hits);
            ExplainResult(results);
            Assert.AreEqual("15 Voss Street, Shirley, Christchurch 8013", results.First().Format.AddressOneLine);
        }
        [Test]
        public void Road_15_vol()
        {
            AddressRepository repo = new AddressRepository();
            repo.GetExplanations = true;
            int hits = 0;
            var results = repo.Read(new AddressSearchQuery("15 vol"), out hits);
            ExplainResult(results);
            Assert.AreEqual("15 Voltaire Court, Botany Downs, Auckland 2010", results.First().Format.AddressOneLine);
        }
        [Test]
        public void Road_15_volg()
        {
            AddressRepository repo = new AddressRepository();
            repo.GetExplanations = true;
            int hits = 0;
            var results = repo.Read(new AddressSearchQuery("15 volg"), out hits);
            ExplainResult(results);
            Assert.AreEqual("15 Volga Street, Island Bay, Wellington 6023", results.First().Format.AddressOneLine);
        }
        //22 Stafford Street, Mount Victoria, Wellington 6011
        [Test]
        public void Road_22_Stafford_Street_Moun()
        {
            AddressRepository repo = new AddressRepository();
            repo.GetExplanations = true;
            int hits = 0;
            var results = repo.Read(new AddressSearchQuery("22 Stafford Street Moun"), out hits);
            ExplainResult(results);
            Assert.AreEqual("22 Stafford Street, Mount Victoria, Wellington 6011", results.First().Format.AddressOneLine);
        }
        [Test]
        public void Road_8_st_eliz()
        {
            AddressRepository repo = new AddressRepository();
            repo.GetExplanations = true;
            int hits = 0;
            var results = repo.Read(new AddressSearchQuery("8 st Elizabeth heigh"), out hits);
            ExplainResult(results);
            Assert.AreEqual("8 St Elizabeth Heights, Normandale, Lower Hutt 5010", results.First().Format.AddressOneLine);
        }
        [Test]
        public void PO_Box_24001()
        {
            AddressRepository repo = new AddressRepository();
            repo.GetExplanations = true;
            int hits = 0;
            var results = repo.Read(new AddressSearchQuery("PO Box 24001"), out hits);
            ExplainResult(results);
            Assert.AreEqual("PO BOX 24001, Wellington, 6142", results.First().Format.AddressOneLine);
        }
        [Test]
        public void PO_Box()
        {
            AddressRepository repo = new AddressRepository();
            repo.GetExplanations = true;
            int hits = 0;
            var results = repo.Read(new AddressSearchQuery("PO Box"), out hits);
            ExplainResult(results);
            Assert.AreEqual("PO BOX 24001, Wellington, 6142", results.First().Format.AddressOneLine);
        }
        [Test]
        public void PO_Box_2()
        {
            AddressRepository repo = new AddressRepository();
            repo.GetExplanations = true;
            int hits = 0;
            var results = repo.Read(new AddressSearchQuery("PO Box 2"), out hits);
            ExplainResult(results);
            Assert.AreEqual("PO BOX 2, Mapua, 7048", results.First().Format.AddressOneLine);
        }
        [Test]
        public void box_hill()
        {
            AddressRepository repo = new AddressRepository();
            repo.GetExplanations = true;
            int hits = 0;
            var results = repo.Read(new AddressSearchQuery("box hill"), out hits);
            ExplainResult(results);
            Assert.AreEqual("5/33 Box Hill, Khandallah, Wellington 6035", results.First().Format.AddressOneLine);
        }
        [Test]
        public void Blakey_Ave_Abbreviation()
        {
            AddressRepository repo = new AddressRepository();
            repo.GetExplanations = true;
            int hits = 0;
            var results = repo.Read(new AddressSearchQuery("25 Blakey Ave Karori Wellington 6012"), out hits);
            ExplainResult(results);
            Assert.AreEqual("25 Blakey Avenue, Karori, Wellington 6012", results.First().Format.AddressOneLine);
        }
        [Test]
        public void Fourteenth_Avenue_Abbreviation()
        {
            AddressRepository repo = new AddressRepository();
            repo.GetExplanations = true;
            int hits = 0;
            var results = repo.Read(new AddressSearchQuery("44 14th Avenue"), out hits);
            ExplainResult(results);
            Assert.AreEqual("44 Fourteenth Avenue, Tauranga South, Tauranga 3112", results.First().Format.AddressOneLine);
        }
    }
}