using System.Collections.Generic;
using System.Diagnostics;

namespace AddressFinder.Tests
{
    public class AddressValidationTestBase
    {
        [Conditional("DEBUG")]
        protected void ExplainResult(PostalAddressSearchResult result)
        {
            Trace.WriteLine(string.Format("{0} - {1}", result.Score, result.Format.AddressOneLine));
            //foreach (var explanation in result.Explanations)
            //{
            //    //_testContext.WriteLine("{0}", explanation);
            //    Trace.WriteLine(string.Format("{0}", explanation));
            //}
            Trace.WriteLine(string.Empty);
        }
        [Conditional("DEBUG")]
        protected void ExplainResult(IEnumerable<PostalAddressSearchResult> results)
        {
            foreach (var result in results)
            {
                ExplainResult(result);
            }
        }
    }
}
