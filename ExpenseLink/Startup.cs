using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExpenseLink.Startup))]
namespace ExpenseLink
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
