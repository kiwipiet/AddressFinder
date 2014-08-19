
namespace AddressFinder
{
    public class AddressSearchQuery : ISearchQuery
    {
        public AddressSearchQuery()
        {
        }
        public AddressSearchQuery(string queryString)
        {
            QueryString = queryString;
            SearchTerms = new SearchTerms(queryString); 
        }
        public string QueryString { get; set; }
        public SearchTerms SearchTerms { get; set; }
    }
}
