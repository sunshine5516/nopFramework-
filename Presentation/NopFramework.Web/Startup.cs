using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NopFramework.Web.Startup))]
namespace NopFramework.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
