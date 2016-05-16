using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ComicConIT.Startup))]
namespace ComicConIT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }


}
