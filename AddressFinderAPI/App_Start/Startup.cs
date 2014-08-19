using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(AddressFinderAPI.AddressValidationAPIStartup))]

namespace AddressFinderAPI
{
    public class AddressValidationAPIStartup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
