using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PromiseData.Startup))]
namespace PromiseData
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
