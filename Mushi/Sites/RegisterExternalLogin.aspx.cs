using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Mushi.Context;
using Mushi.Models;
using MushiDataTypes.Enums;
using Steam;
using System;
using System.Linq;
using System.Web;

namespace Mushi.Sites
{
    public partial class RegisterExternalLogin : System.Web.UI.Page
    {
        /// <summary>
        /// Gets the name of the provider.
        /// </summary>
        /// <value>
        /// The name of the provider.
        /// </value>
        protected string ProviderName { get; set; }


        /// <summary>
        /// Gets the player context factory.
        /// </summary>
        /// <value>
        /// The player context factory.
        /// </value>
        private PlayerContext PlayerContextFactory
        {
            get { return new PlayerContext(); }
        }

        /// <summary>
        /// Gets the steam context factory.
        /// </summary>
        /// <value>
        /// The steam context factory.
        /// </value>
        private SteamContext SteamContextFactory
        {
            get { return new SteamContext(); }
        }

        private readonly string[] _allowedSteamIds =
        {
            "76561197981671190",
            "76561197987622054",
            "76561198140188479",
            "76561198073203279",
            "76561198149684423",
            "76561197983813655",
            "76561198067371116"
        };

        protected void Page_Load()
        {
            if (IsPostBack) return;
            ProviderName = IdentityHelper.GetProviderNameFromRequest(Request);
            if (String.IsNullOrEmpty(ProviderName) || ProviderName != "Steam")
            {
                Response.Redirect("~/Login.aspx?providerName=null");
            }
            var loginInfo = Context.GetOwinContext().Authentication.GetExternalLoginInfo();
            if (loginInfo == null)
            {
                Response.Redirect("~/Login.aspx?loginInfo=null");
            }
            if (!TrySetSteamId(loginInfo))
            {
                Response.Redirect("~/Login.aspx?SteamConvert=false");
            }
            //TODO remove later
            if (!_allowedSteamIds.ToList().Contains(loginInfo.Login.ProviderKey))
            {
                Response.Redirect("~/Login.aspx");
            }
            var manager = new ApplicationUserManager(
                    new UserStore<ApplicationUser>(ApplicationDbContext.Create()));
            var user = manager.Find(loginInfo.Login);
            if (user != null)
            {
                var player = SteamContextFactory.GetPlayerSteamInfo(long.Parse(loginInfo.Login.ProviderKey), SteamRequestsEnum.SignIn);
                if (player != null)
                {
                    PlayerContextFactory.UpdatePlayerInfo(user.Id, player.AvatarUrl);
                }
                IdentityHelper.SignIn(manager, user, isPersistent: true);
                Response.Redirect("~/Sites/Mushi.aspx");
            }
            else
            {
                CreateAndLogInUser(manager, loginInfo);
            }
        }

        /// <summary>
        /// Tries the set steam identifier.
        /// </summary>
        /// <param name="loginInfo">The login information.</param>
        /// <returns></returns>
        private bool TrySetSteamId(ExternalLoginInfo loginInfo)
        {
            var steamId = SteamHelpers.GetSteamIdFromOpenId(loginInfo.Login.ProviderKey);
            if (string.IsNullOrEmpty(steamId)) return false;
            loginInfo.Login.ProviderKey = steamId;
            return true;
        }

        /// <summary>
        /// Creates the and log in user.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="loginInfo">The login information.</param>
        private void CreateAndLogInUser(ApplicationUserManager manager, ExternalLoginInfo loginInfo)
        {
            var user = new ApplicationUser {UserName = "default"};
            var result = manager.Create(user);
            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(user.Id))
                {
                    var steamId = loginInfo.Login.ProviderKey;
                    result = manager.AddLogin(user.Id, loginInfo.Login);
                    if (result.Succeeded)
                    {
                        var player = SteamContextFactory.GetPlayerSteamInfo(long.Parse(steamId), SteamRequestsEnum.SignUp );
                        if (player != null)
                        {
                            PlayerContextFactory.UpdatePlayerInfo(user.Id, player.AvatarUrl);
                        }
                        if (result.Succeeded)
                        {
                            IdentityHelper.SignIn(manager, user, isPersistent: true);
                            Response.Redirect("~/Sites/Mushi.aspx");
                        }
                    }
                }
            }
            AddErrors(result);
        }

        /// <summary>
        /// Adds the errors.
        /// </summary>
        /// <param name="result">The result.</param>
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}