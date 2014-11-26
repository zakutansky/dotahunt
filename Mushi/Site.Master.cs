using Microsoft.Owin.Security;
using MushiDataTypes;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;

namespace Mushi
{
    public partial class SiteMaster : MasterPage
    {
        /// <summary>
        /// Gets or sets the player.
        /// </summary>
        /// <value>
        /// The player.
        /// </value>
        public Player Player { get; set; }

        /// <summary>
        /// Gets or sets the return URL.
        /// </summary>
        /// <value>
        /// The return URL.
        /// </value>
        private string ReturnUrl { get; set; }

        /// <summary>
        /// Handles the Init event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="System.Web.HttpException">505;HTTP/1.0 not supported; use HTTP/1.1</exception>
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Context.Request.ServerVariables["SERVER_PROTOCOL"] == "HTTP/1.0")
            {
                throw new HttpException(505, "HTTP/1.0 not supported; use HTTP/1.1");
            } 
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            bool visibility = Player != null;
            ibLoginSteam.Visible = !visibility;
            imgAvatar.ImageUrl = visibility ? Player.AvatarUrl : "";
            imgAvatar.Visible = visibility;
            imgLogout.Visible = visibility;
        }

        /// <summary>
        /// Handles the Click event of the imgLogout control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        protected void imgLogout_Click(object sender, ImageClickEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut();
            Response.Redirect("~/Login.aspx");
        }

        /// <summary>
        /// Logs the in now.
        /// </summary>
        private void LogInNow()
        {
            var provider = "Steam";
            // Request a redirect to the external login provider
            string redirectUrl = ResolveUrl(String.Format(CultureInfo.InvariantCulture, "~/Sites/RegisterExternalLogin.aspx?{0}={1}&returnUrl={2}", IdentityHelper.ProviderNameKey, provider, ReturnUrl));
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            Context.GetOwinContext().Authentication.Challenge(properties, provider);
            Response.StatusCode = 401;
            Response.End();
        }

        /// <summary>
        /// Handles the Click event of the ibLoginSteam control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ImageClickEventArgs"/> instance containing the event data.</param>
        protected void ibLoginSteam_Click(object sender, ImageClickEventArgs e)
        {
            //LogInNow();
        }
    }

}