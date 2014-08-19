using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections.ObjectModel;

namespace AddressFinder
{
    public class SearchTerms : Collection<SearchTerm>
    {
        public string OriginalSearchTerms { get; private set; }
        public SearchTerms(string searchTerms)
        {
            OriginalSearchTerms = searchTerms.ToLower().Trim();
            foreach (var term in OriginalSearchTerms.Split(" ,/\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Distinct())
            {
                var termToLower = term;
                if (string.IsNullOrWhiteSpace(termToLower))
                {
                    continue;
                }
                var termNumber = 0;
                var isTermNumeric = int.TryParse(term, out termNumber);
                Add(new SearchTerm() { Term = termToLower, IsNumeric = isTermNumeric, Length = termToLower.Length });
            }
        }
    }
}
