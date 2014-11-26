using Mushi.Helper;
using MushiDataTypes;
using MushiDataTypes.Enums;

namespace Mushi.Extensions
{
    public static class PlayerExtensions
    {
        /// <summary>
        /// Updates the avatar.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="requestType">Type of the request.</param>
        public static void UpdateAvatar(this Player player, SteamRequestsEnum requestType)
        {
            PlayerHelper.UpdateAvatar(player, requestType);
        }
    }
}