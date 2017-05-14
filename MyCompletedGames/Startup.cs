using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MyCompletedGames.Startup))]
namespace MyCompletedGames
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
