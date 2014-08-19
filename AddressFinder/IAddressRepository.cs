using System.Collections.Generic;
using System;

namespace AddressFinder
{
    public interface IAddressRepository
    {
        IEnumerable<PostalAddressSearchResult> Read(ISearchQuery searchQuery, out int hits);
        IEnumerable<PostalAddressSearchResult> Read(ISearchQuery searchQuery, int top, out int hits);
        void Write(IEnumerable<PostalAddress> postalAddresses);
    }
}
