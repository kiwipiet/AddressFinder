using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using System.Collections.Generic;
using System.IO;

namespace AddressFinder
{
    public class SynonymAnalyzer : StandardAnalyzer
    {
        public SynonymAnalyzer(Lucene.Net.Util.Version matchVersion)
            : base(matchVersion)
        {
            
        }
        public SynonymAnalyzer(Lucene.Net.Util.Version matchVersion, ISet<string> stopWords)
            : base(matchVersion, stopWords)
        {
            
        }
        public SynonymAnalyzer(Lucene.Net.Util.Version matchVersion, FileInfo stopwords)
            : base(matchVersion, stopwords)
        {
            
        }
        public SynonymAnalyzer(Lucene.Net.Util.Version matchVersion, TextReader stopwords)
            : base(matchVersion, stopwords)
        {
            
        }
        public override TokenStream TokenStream(string fieldName, TextReader reader)
        {
            // TODO : Inject the SynonymEngine
            return new SynonymFilter(base.TokenStream(fieldName, reader), new SynonymEngine());
        }
    }
}
