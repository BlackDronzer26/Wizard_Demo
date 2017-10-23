using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Product_Wizard.Startup))]
namespace Product_Wizard
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
