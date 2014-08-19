using System.Collections.Generic;

namespace AddressFinder
{
    public class SearchPostalAddress
    {
        public SearchPostalAddress(IEnumerable<PostalAddressSearchResult> results, string status, string errorMessage)
        {
            Results = results;
            Status = status;
            ErrorMessage = errorMessage;
        }
        public SearchPostalAddress()
        {
            Status = "OK";
        }
        public IEnumerable<PostalAddressSearchResult> Results { get; set; }
        public string Status { get; set; }
        public string ErrorMessage { get; set; }
    }
}
