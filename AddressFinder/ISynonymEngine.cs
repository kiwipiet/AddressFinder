using System.Collections.Generic;
using System;
using Lucene.Net.Analysis;

namespace AddressFinder
{
    public interface ISynonymEngine
    {
        CharArraySet GetSynonyms(char[] word, int len);
    }
}
