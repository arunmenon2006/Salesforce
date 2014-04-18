using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SalesforceLogin.Startup))]
namespace SalesforceLogin
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
