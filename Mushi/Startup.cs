using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mushi.Startup))]
namespace Mushi
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(30);
            var idProvider = new CustomUserIdProvider();

            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);
            app.MapSignalR();
        }
    }

    public class CustomUserIdProvider : IUserIdProvider
    {
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public string GetUserId(IRequest request)
        {
            return request.GetHttpContext().User.Identity.GetUserId();
        }
    }
}
