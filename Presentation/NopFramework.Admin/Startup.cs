using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NopFramework.Admin.Startup))]
namespace NopFramework.Admin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
