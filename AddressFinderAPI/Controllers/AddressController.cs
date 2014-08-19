using AddressFinder;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;

namespace AddressFinderAPI.Controllers
{
    public class AddressController : ApiController
    {
        readonly AddressRepository _addressRepo;
        public AddressController()
        {
            _addressRepo = new AddressRepository() { GetExplanations = bool.Parse(ConfigurationManager.AppSettings["GetExplanations"] ?? "false") };
        }

        [HttpGet]
        public SearchPostalAddress SearchTop(int top, string terms)
        {
            try
            {
                int hits = 0;
                return new SearchPostalAddress() { Results = _addressRepo.Read(new AddressSearchQuery(terms), top, out hits) };
            }
            catch (Exception ex)
            {
                return new SearchPostalAddress() { Results = new List<PostalAddressSearchResult>(), Status = "Error", ErrorMessage = ex.Message };
            }
        }
        [HttpGet]
        public SearchPostalAddress Search(string terms)
        {
            try
            {
                int hits = 0;
                return new SearchPostalAddress() { Results = _addressRepo.Read(new AddressSearchQuery(terms), out hits) };
            }
            catch (Exception ex)
            {
                return new SearchPostalAddress() { Results = new List<PostalAddressSearchResult>(), Status = "Error", ErrorMessage = ex.Message };
            }
        }
    }
}