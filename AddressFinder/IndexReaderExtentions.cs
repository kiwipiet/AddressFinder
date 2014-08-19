using Lucene.Net.Index;
using System.Collections.Generic;

namespace AddressFinder
{
    public static class IndexReaderExtentions
    {
        internal static IEnumerable<string> UniqueTermsFromField(this IndexReader reader, string field)
        {
            var termEnum = reader.Terms(new Term(field));

            do
            {
                var currentTerm = termEnum.Term;

                if (currentTerm.Field != field)
                    yield break;

                yield return currentTerm.Text;
            } while (termEnum.Next());
        }
    }
}
