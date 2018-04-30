using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Kinnect01Service.Startup))]

namespace Kinnect01Service
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}