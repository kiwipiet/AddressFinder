using System.Diagnostics;

namespace AddressFinder
{
    [DebuggerDisplay("Address = {Format}, Score = {Score}")]
    public class PostalAddressSearchResultExplanations : PostalAddressSearchResult
    {
        public PostalAddressSearchResultExplanations()
        {
        }
        public PostalAddressSearchResultExplanations(float score)
        {
            Score = score;
        }

        public Lucene.Net.Search.Explanation[] Explanations { get; set; }
    }
}
