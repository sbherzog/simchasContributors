using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(simchasContributors.Startup))]
namespace simchasContributors
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
