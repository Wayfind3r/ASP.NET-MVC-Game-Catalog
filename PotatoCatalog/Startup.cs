using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PotatoCatalog.Startup))]
namespace PotatoCatalog
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
