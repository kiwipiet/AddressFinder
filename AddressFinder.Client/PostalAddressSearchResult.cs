using System.Diagnostics;

namespace AddressFinder
{
    [DebuggerDisplay("Address = {Format}, Score = {Score}")]
    public class PostalAddressSearchResult : PostalAddress
    {
        public PostalAddressSearchResult()
        {
        }
        public PostalAddressSearchResult(float score)
        {
            Score = score;
        }

        public float Score { get; set; }

        public PostalAddressFormat Format { get; set; }
    }
}
