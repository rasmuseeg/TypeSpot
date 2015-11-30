using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Typespot.Startup))]
namespace Typespot
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
