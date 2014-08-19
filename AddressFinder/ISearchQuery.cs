
namespace AddressFinder
{
    public interface ISearchQuery
    {
        string QueryString { get; set; }
        SearchTerms SearchTerms { get; set; }
    }
}
