using AddressFinder.Tests.Util;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Analysis.Tokenattributes;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Util;
using NUnit.Framework;
using System;
using System.Linq;

namespace AddressFinder.Tests
{
    [TestFixture]
    public class SynonymFilter_Tests : BaseTokenStreamTestCase
    {
        [Test]
        public void SynonymFilter_Test()
        {
            QueryParser qp = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, "", new MultiAnalyzer(this));
            
            Assert.AreEqual("(apartment apt) foo", qp.Parse("Apartment foo").ToString());
            Assert.AreEqual("(14th fourteenth) foo", qp.Parse("14th foo").ToString());
        }
        /// <summary> Expands "multi" to "multi" and "multi2", both at the same position,
        /// and expands "triplemulti" to "triplemulti", "multi3", and "multi2".  
        /// </summary>
        private class MultiAnalyzer : Analyzer
        {
            private void InitBlock(SynonymFilter_Tests enclosingInstance)
            {
                this.enclosingInstance = enclosingInstance;
            }
            private SynonymFilter_Tests enclosingInstance;
            public SynonymFilter_Tests Enclosing_Instance
            {
                get
                {
                    return enclosingInstance;
                }

            }

            public MultiAnalyzer(SynonymFilter_Tests enclosingInstance)
            {
                InitBlock(enclosingInstance);
            }

            public override TokenStream TokenStream(System.String fieldName, System.IO.TextReader reader)
            {
                TokenStream result = new StandardTokenizer(Lucene.Net.Util.Version.LUCENE_30, reader);
                result = new LowerCaseFilter(result);
                result = new SynonymFilter(result, new SynonymEngine());
                return result;
            }
        }

    }
    /// <summary>Base class for all Lucene unit tests that use TokenStreams.</summary>
    public abstract class BaseTokenStreamTestCase : LuceneTestCase
    {
        public BaseTokenStreamTestCase()
        { }

        public BaseTokenStreamTestCase(System.String name)
            : base(name)
        { }

        // some helpers to test Analyzers and TokenStreams:
        public interface ICheckClearAttributesAttribute : Lucene.Net.Util.IAttribute
        {
            bool GetAndResetClearCalled();
        }

        public class CheckClearAttributesAttribute : Lucene.Net.Util.Attribute, ICheckClearAttributesAttribute
        {
            private bool clearCalled = false;

            public bool GetAndResetClearCalled()
            {
                try
                {
                    return clearCalled;
                }
                finally
                {
                    clearCalled = false;
                }
            }

            public override void Clear()
            {
                clearCalled = true;
            }

            public override bool Equals(Object other)
            {
                return (
                other is CheckClearAttributesAttribute &&
                ((CheckClearAttributesAttribute)other).clearCalled == this.clearCalled
                );
            }

            public override int GetHashCode()
            {
                //Java: return 76137213 ^ Boolean.valueOf(clearCalled).hashCode();
                return 76137213 ^ clearCalled.GetHashCode();
            }

            public override void CopyTo(Lucene.Net.Util.Attribute target)
            {
                target.Clear();
            }
        }

        public static void AssertTokenStreamContents(TokenStream ts, System.String[] output, int[] startOffsets, int[] endOffsets, System.String[] types, int[] posIncrements, int? finalOffset)
        {
            Assert.IsNotNull(output);
            ICheckClearAttributesAttribute checkClearAtt = ts.AddAttribute<ICheckClearAttributesAttribute>();

            Assert.IsTrue(ts.HasAttribute<ITermAttribute>(), "has no TermAttribute");
            ITermAttribute termAtt = ts.GetAttribute<ITermAttribute>();

            IOffsetAttribute offsetAtt = null;
            if (startOffsets != null || endOffsets != null || finalOffset != null)
            {
                Assert.IsTrue(ts.HasAttribute<IOffsetAttribute>(), "has no OffsetAttribute");
                offsetAtt = ts.GetAttribute<IOffsetAttribute>();
            }

            ITypeAttribute typeAtt = null;
            if (types != null)
            {
                Assert.IsTrue(ts.HasAttribute<ITypeAttribute>(), "has no TypeAttribute");
                typeAtt = ts.GetAttribute<ITypeAttribute>();
            }

            IPositionIncrementAttribute posIncrAtt = null;
            if (posIncrements != null)
            {
                Assert.IsTrue(ts.HasAttribute<IPositionIncrementAttribute>(), "has no PositionIncrementAttribute");
                posIncrAtt = ts.GetAttribute<IPositionIncrementAttribute>();
            }

            ts.Reset();
            for (int i = 0; i < output.Length; i++)
            {
                // extra safety to enforce, that the state is not preserved and also assign bogus values
                ts.ClearAttributes();
                termAtt.SetTermBuffer("bogusTerm");
                if (offsetAtt != null) offsetAtt.SetOffset(14584724, 24683243);
                if (typeAtt != null) typeAtt.Type = "bogusType";
                if (posIncrAtt != null) posIncrAtt.PositionIncrement = 45987657;

                checkClearAtt.GetAndResetClearCalled(); // reset it, because we called clearAttribute() before
                Assert.IsTrue(ts.IncrementToken(), "token " + i + " does not exist");
                Assert.IsTrue(checkClearAtt.GetAndResetClearCalled(), "clearAttributes() was not called correctly in TokenStream chain");

                Assert.AreEqual(output[i], termAtt.Term, "term " + i);
                if (startOffsets != null)
                    Assert.AreEqual(startOffsets[i], offsetAtt.StartOffset, "startOffset " + i);
                if (endOffsets != null)
                    Assert.AreEqual(endOffsets[i], offsetAtt.EndOffset, "endOffset " + i);
                if (types != null)
                    Assert.AreEqual(types[i], typeAtt.Type, "type " + i);
                if (posIncrements != null)
                    Assert.AreEqual(posIncrements[i], posIncrAtt.PositionIncrement, "posIncrement " + i);
            }
            Assert.IsFalse(ts.IncrementToken(), "end of stream");
            ts.End();
            if (finalOffset.HasValue)
                Assert.AreEqual(finalOffset, offsetAtt.EndOffset, "finalOffset ");
            ts.Dispose();
        }

        public static void AssertTokenStreamContents(TokenStream ts, String[] output, int[] startOffsets, int[] endOffsets, String[] types, int[] posIncrements)
        {
            AssertTokenStreamContents(ts, output, startOffsets, endOffsets, types, posIncrements, null);
        }

        public static void AssertTokenStreamContents(TokenStream ts, String[] output)
        {
            AssertTokenStreamContents(ts, output, null, null, null, null, null);
        }

        public static void AssertTokenStreamContents(TokenStream ts, String[] output, String[] types)
        {
            AssertTokenStreamContents(ts, output, null, null, types, null, null);
        }

        public static void AssertTokenStreamContents(TokenStream ts, String[] output, int[] posIncrements)
        {
            AssertTokenStreamContents(ts, output, null, null, null, posIncrements, null);
        }

        public static void AssertTokenStreamContents(TokenStream ts, String[] output, int[] startOffsets, int[] endOffsets)
        {
            AssertTokenStreamContents(ts, output, startOffsets, endOffsets, null, null, null);
        }

        public static void AssertTokenStreamContents(TokenStream ts, String[] output, int[] startOffsets, int[] endOffsets, int? finalOffset)
        {
            AssertTokenStreamContents(ts, output, startOffsets, endOffsets, null, null, finalOffset);
        }

        public static void AssertTokenStreamContents(TokenStream ts, String[] output, int[] startOffsets, int[] endOffsets, int[] posIncrements)
        {
            AssertTokenStreamContents(ts, output, startOffsets, endOffsets, null, posIncrements, null);
        }

        public static void AssertTokenStreamContents(TokenStream ts, String[] output, int[] startOffsets, int[] endOffsets, int[] posIncrements, int? finalOffset)
        {
            AssertTokenStreamContents(ts, output, startOffsets, endOffsets, null, posIncrements, finalOffset);
        }

        public static void AssertAnalyzesTo(Analyzer a, String input, String[] output, int[] startOffsets, int[] endOffsets, String[] types, int[] posIncrements)
        {
            AssertTokenStreamContents(a.TokenStream("dummy", new System.IO.StringReader(input)), output, startOffsets, endOffsets, types, posIncrements, input.Length);
        }

        public static void AssertAnalyzesTo(Analyzer a, String input, String[] output)
        {
            AssertAnalyzesTo(a, input, output, null, null, null, null);
        }

        public static void AssertAnalyzesTo(Analyzer a, String input, String[] output, String[] types)
        {
            AssertAnalyzesTo(a, input, output, null, null, types, null);
        }

        public static void AssertAnalyzesTo(Analyzer a, String input, String[] output, int[] posIncrements)
        {
            AssertAnalyzesTo(a, input, output, null, null, null, posIncrements);
        }

        public static void AssertAnalyzesTo(Analyzer a, String input, String[] output, int[] startOffsets, int[] endOffsets)
        {
            AssertAnalyzesTo(a, input, output, startOffsets, endOffsets, null, null);
        }

        public static void AssertAnalyzesTo(Analyzer a, String input, String[] output, int[] startOffsets, int[] endOffsets, int[] posIncrements)
        {
            AssertAnalyzesTo(a, input, output, startOffsets, endOffsets, null, posIncrements);
        }


        public static void AssertAnalyzesToReuse(Analyzer a, String input, String[] output, int[] startOffsets, int[] endOffsets, String[] types, int[] posIncrements)
        {
            AssertTokenStreamContents(a.ReusableTokenStream("dummy", new System.IO.StringReader(input)), output, startOffsets, endOffsets, types, posIncrements, input.Length);
        }

        public static void AssertAnalyzesToReuse(Analyzer a, String input, String[] output)
        {
            AssertAnalyzesToReuse(a, input, output, null, null, null, null);
        }

        public static void AssertAnalyzesToReuse(Analyzer a, String input, String[] output, String[] types)
        {
            AssertAnalyzesToReuse(a, input, output, null, null, types, null);
        }

        public static void AssertAnalyzesToReuse(Analyzer a, String input, String[] output, int[] posIncrements)
        {
            AssertAnalyzesToReuse(a, input, output, null, null, null, posIncrements);
        }

        public static void AssertAnalyzesToReuse(Analyzer a, String input, String[] output, int[] startOffsets, int[] endOffsets)
        {
            AssertAnalyzesToReuse(a, input, output, startOffsets, endOffsets, null, null);
        }

        public static void AssertAnalyzesToReuse(Analyzer a, String input, String[] output, int[] startOffsets, int[] endOffsets, int[] posIncrements)
        {
            AssertAnalyzesToReuse(a, input, output, startOffsets, endOffsets, null, posIncrements);
        }

        // simple utility method for testing stemmers

        public static void CheckOneTerm(Analyzer a, System.String input, System.String expected)
        {
            AssertAnalyzesTo(a, input, new System.String[] { expected });
        }

        public static void CheckOneTermReuse(Analyzer a, System.String input, System.String expected)
        {
            AssertAnalyzesToReuse(a, input, new System.String[] { expected });
        }
    }

    /// <summary> Base class for all Lucene unit tests.  
    /// <p/>
    /// Currently the
    /// only added functionality over JUnit's TestCase is
    /// asserting that no unhandled exceptions occurred in
    /// threads launched by ConcurrentMergeScheduler and asserting sane
    /// FieldCache usage athe moment of tearDown.
    /// <p/>
    /// If you
    /// override either <c>setUp()</c> or
    /// <c>tearDown()</c> in your unit test, make sure you
    /// call <c>super.setUp()</c> and
    /// <c>super.tearDown()</c>
    /// <p/>
    /// </summary>
    /// <seealso cref="assertSaneFieldCaches">
    /// </seealso>
    [Serializable]
    public abstract class LuceneTestCase
    {
        public static System.IO.FileInfo TEMP_DIR;
        static LuceneTestCase()
        {
            String directory = Paths.TempDirectory;

            TEMP_DIR = new System.IO.FileInfo(directory);
        }

        bool allowDocsOutOfOrder = true;

        public LuceneTestCase()
            : base()
        {
        }

        public LuceneTestCase(System.String name)
        {
        }

        [SetUp]
        public virtual void SetUp()
        {
            ConcurrentMergeScheduler.SetTestMode();
        }

        /// <summary> Forcible purges all cache entries from the FieldCache.
        /// <p/>
        /// This method will be called by tearDown to clean up FieldCache.DEFAULT.
        /// If a (poorly written) test has some expectation that the FieldCache
        /// will persist across test methods (ie: a static IndexReader) this 
        /// method can be overridden to do nothing.
        /// <p/>
        /// </summary>
        /// <seealso cref="FieldCache.PurgeAllCaches()">
        /// </seealso>
        protected internal virtual void PurgeFieldCache(FieldCache fc)
        {
            fc.PurgeAllCaches();
        }

        protected internal virtual System.String GetTestLabel()
        {
            return NUnit.Framework.TestContext.CurrentContext.Test.FullName;
        }

        [TearDown]
        public virtual void TearDown()
        {
            try
            {
                // this isn't as useful as calling directly from the scope where the 
                // index readers are used, because they could be gc'ed just before
                // tearDown is called.
                // But it's better then nothing.
                AssertSaneFieldCaches(GetTestLabel());

                if (ConcurrentMergeScheduler.AnyUnhandledExceptions())
                {
                    // Clear the failure so that we don't just keep
                    // failing subsequent test cases
                    ConcurrentMergeScheduler.ClearUnhandledExceptions();
                    Assert.Fail("ConcurrentMergeScheduler hit unhandled exceptions");
                }
            }
            finally
            {
                PurgeFieldCache(Lucene.Net.Search.FieldCache_Fields.DEFAULT);
            }

            //base.TearDown();  // {{Aroush-2.9}}
            this.seed = null;
        }

        /// <summary> Asserts that FieldCacheSanityChecker does not detect any 
        /// problems with FieldCache.DEFAULT.
        /// <p/>
        /// If any problems are found, they are logged to System.err 
        /// (allong with the msg) when the Assertion is thrown.
        /// <p/>
        /// This method is called by tearDown after every test method, 
        /// however IndexReaders scoped inside test methods may be garbage 
        /// collected prior to this method being called, causing errors to 
        /// be overlooked. Tests are encouraged to keep their IndexReaders 
        /// scoped at the class level, or to explicitly call this method 
        /// directly in the same scope as the IndexReader.
        /// <p/>
        /// </summary>
        /// <seealso cref="FieldCacheSanityChecker">
        /// </seealso>
        protected internal virtual void AssertSaneFieldCaches(System.String msg)
        {
            CacheEntry[] entries = Lucene.Net.Search.FieldCache_Fields.DEFAULT.GetCacheEntries();
            Lucene.Net.Util.FieldCacheSanityChecker.Insanity[] insanity = null;
            try
            {
                try
                {
                    insanity = FieldCacheSanityChecker.CheckSanity(entries);
                }
                catch (System.SystemException e)
                {
                    System.IO.StreamWriter temp_writer;
                    temp_writer = new System.IO.StreamWriter(System.Console.OpenStandardError(), System.Console.Error.Encoding);
                    temp_writer.AutoFlush = true;
                    DumpArray(msg + ": FieldCache", entries, temp_writer);
                    throw e;
                }

                Assert.AreEqual(0, insanity.Length, msg + ": Insane FieldCache usage(s) found");
                insanity = null;
            }
            finally
            {

                // report this in the event of any exception/failure
                // if no failure, then insanity will be null anyway
                if (null != insanity)
                {
                    System.IO.StreamWriter temp_writer2;
                    temp_writer2 = new System.IO.StreamWriter(System.Console.OpenStandardError(), System.Console.Error.Encoding);
                    temp_writer2.AutoFlush = true;
                    DumpArray(msg + ": Insane FieldCache usage(s)", insanity, temp_writer2);
                }
            }
        }

        /// <summary> Convinience method for logging an iterator.</summary>
        /// <param name="label">String logged before/after the items in the iterator
        /// </param>
        /// <param name="iter">Each next() is toString()ed and logged on it's own line. If iter is null this is logged differnetly then an empty iterator.
        /// </param>
        /// <param name="stream">Stream to log messages to.
        /// </param>
        public static void DumpIterator(System.String label, System.Collections.IEnumerator iter, System.IO.StreamWriter stream)
        {
            stream.WriteLine("*** BEGIN " + label + " ***");
            if (null == iter)
            {
                stream.WriteLine(" ... NULL ...");
            }
            else
            {
                while (iter.MoveNext())
                {
                    stream.WriteLine(iter.Current.ToString());
                }
            }
            stream.WriteLine("*** END " + label + " ***");
        }

        /// <summary> Convinience method for logging an array.  Wraps the array in an iterator and delegates</summary>
        /// <seealso cref="dumpIterator(String,Iterator,PrintStream)">
        /// </seealso>
        public static void DumpArray(System.String label, System.Object[] objs, System.IO.StreamWriter stream)
        {
            System.Collections.IEnumerator iter = (null == objs) ? null : new System.Collections.ArrayList(objs).GetEnumerator();
            DumpIterator(label, iter, stream);
        }

        /// <summary> Returns a {@link Random} instance for generating random numbers during the test.
        /// The random seed is logged during test execution and printed to System.out on any failure
        /// for reproducing the test using {@link #NewRandom(long)} with the recorded seed
        /// .
        /// </summary>
        public virtual System.Random NewRandom()
        {
            if (this.seed != null)
            {
                throw new System.SystemException("please call LuceneTestCase.newRandom only once per test");
            }
            return NewRandom(seedRnd.Next(System.Int32.MinValue, System.Int32.MaxValue));
        }

        /// <summary> Returns a {@link Random} instance for generating random numbers during the test.
        /// If an error occurs in the test that is not reproducible, you can use this method to
        /// initialize the number generator with the seed that was printed out during the failing test.
        /// </summary>
        public virtual System.Random NewRandom(int seed)
        {
            if (this.seed != null)
            {
                throw new System.SystemException("please call LuceneTestCase.newRandom only once per test");
            }
            this.seed = seed;
            return new System.Random(seed);
        }

        // recorded seed
        [NonSerialized]
        protected internal int? seed = null;
        //protected internal bool seed_init = false;

        // static members
        [NonSerialized]
        private static readonly System.Random seedRnd = new System.Random();

        #region Java porting shortcuts
        protected static void assertEquals(string msg, object obj1, object obj2)
        {
            Assert.AreEqual(obj1, obj2, msg);
        }

        protected static void assertEquals(object obj1, object obj2)
        {
            Assert.AreEqual(obj1, obj2);
        }

        protected static void assertEquals(double d1, double d2, double delta)
        {
            Assert.AreEqual(d1, d2, delta);
        }

        protected static void assertEquals(string msg, double d1, double d2, double delta)
        {
            Assert.AreEqual(d1, d2, delta, msg);
        }

        protected static void assertTrue(bool cnd)
        {
            Assert.IsTrue(cnd);
        }

        protected static void assertTrue(string msg, bool cnd)
        {
            Assert.IsTrue(cnd, msg);
        }

        protected static void assertNotNull(object o)
        {
            Assert.NotNull(o);
        }

        protected static void assertNotNull(string msg, object o)
        {
            Assert.NotNull(o, msg);
        }

        protected static void assertNull(object o)
        {
            Assert.Null(o);
        }

        protected static void assertNull(string msg, object o)
        {
            Assert.Null(o, msg);
        }
        #endregion
    }
}
