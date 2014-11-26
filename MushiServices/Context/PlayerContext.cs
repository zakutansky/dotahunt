using System;
using System.Collections.Generic;
using System.Linq;
using MushiDataTypes.Enums;
using MushiServices.Context.Data;
using MushiServices.DbServices;
using Steam;

namespace MushiServices.Context
{
    public class PlayerContext
    {
        /// <summary>
        /// Gets the progamers.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProgamerWithKey> GetProgamers()
        {
            using (var service = new MushiDbService())
            {
                return service.GetProPlayers().ToList();
            }
        }

        /// <summary>
        /// Gets the new old players.
        /// </summary>
        /// <param name="buffPlayers">The buff players.</param>
        /// <param name="localPlayers">The local players.</param>
        /// <returns></returns>
        public ComparsionResult GetNewOldPlayers(List<DotabuffPlayer> buffPlayers,
            List<ProgamerWithKey> localPlayers)
        {
            var result = new ComparsionResult();
            var buffIds = buffPlayers.Select(o => o.SteamId).ToArray();
            var localIds = localPlayers.Select(o => o.ProviderKey).ToArray();

            var newIds = buffIds.Where(o => !localIds.Contains(o)).ToArray();
            if (newIds.Any())
            {
                result.NewPlayers = buffPlayers.Where(o => newIds.Contains(o.SteamId)).ToList();
            }
            var oldIds = localIds.Where(o => !buffIds.Contains(o)).ToArray();
            if (oldIds.Any())
            {
                result.OldPlayers = localPlayers.Where(o => oldIds.Contains(o.ProviderKey)).ToList();
            }
            return result;
        }

        /// <summary>
        /// Compares for any changes.
        /// </summary>
        /// <param name="buffPlayers">The buff players.</param>
        /// <param name="localPlayers">The local players.</param>
        /// <returns></returns>
        public List<ModifiedPlayer> CompareForAnyChanges(List<DotabuffPlayer> buffPlayers,
            IEnumerable<ProgamerWithKey> localPlayers)
        {
            var result = new List<ModifiedPlayer>();
            foreach (var player in localPlayers)
            {
                var buffPlayer = buffPlayers.SingleOrDefault(o => o.SteamId == player.ProviderKey);
                if (buffPlayer != null)
                {
                    string newNick = null, newUrl = null;
                    if (player.NickName != buffPlayer.NickName)
                    {
                        newNick = buffPlayer.NickName;
                    }
                    if (player.ProfileUrl != buffPlayer.ProfileUrl)
                    {
                        newUrl = buffPlayer.ProfileUrl;
                    }
                    if (newNick != null || newUrl != null)
                    {
                        result.Add(new ModifiedPlayer
                        {
                            UserId = player.UserId,
                            SteamId = player.ProviderKey,
                            OldNickName = player.NickName,
                            NewNickName = newNick,
                            NewProfileUrl = newUrl
                        });
                    }
                }
            }
            return result;
        }

        public void UpdatePlayers(List<ModifiedPlayer> players)
        {
            using (var service = new MushiDbService())
            {
                players.ForEach(o =>
                        service.UpdatePlayerInfo(o.UserId, o.NewNickName)
                    );
            }
        }


        /// <summary>
        /// Creates the progamer.
        /// </summary>
        /// <param name="players">The player.</param>
        public void CreateProgamers(IEnumerable<DotabuffPlayer> players)
        {
            using (var service = new MushiDbService())
            {
                var steamContext = new SteamContext();
                foreach (var player in players)
                {
                    ValidatePlayer(player);
                    var steamUser = steamContext.GetPlayerSteamInfo(long.Parse(player.SteamId), SteamRequestsEnum.SignIn);
                    if(steamUser == null)throw new Exception("Cannnot load steam user: nick=" + player.NickName + " steamId: " + player.SteamId);
                    var userId = CreateAspNetUser(service, player.NickName, steamUser.AvatarUrl);
                    if(string.IsNullOrEmpty(userId)) throw new Exception("Error create new aspnetuser");
                    CreateAspNetUserLogin(service, userId, "Steam", player.SteamId);
                    CreateProgamer(service, userId, player.ProfileUrl);
                }
            }
        }

        /// <summary>
        /// Validates the player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <exception cref="System.Exception">Dotabuff requier fields empty</exception>
        private void ValidatePlayer(DotabuffPlayer player)
        {
            if (string.IsNullOrEmpty(player.NickName)
                || string.IsNullOrEmpty(player.SteamId))
                throw new Exception("Dotabuff requier fields empty");
        }

        /// <summary>
        /// Creates the progamer.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="profileUrl">The profile URL.</param>
        /// <returns></returns>
        private void CreateProgamer(MushiDbService service, string playerId, string profileUrl)
        {
            service.CreateProgamer(playerId, profileUrl);
        }

        /// <summary>
        /// Creates the ASP net user login.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="userId">The user identifier.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="providerKey">The provider key.</param>
        private void CreateAspNetUserLogin(MushiDbService service, string userId, string provider, string providerKey)
        {
            service.CreateAspNetUserLogin(userId, provider, providerKey);
        }

        /// <summary>
        /// Creates the ASP net user.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="avaterUrl">The avater URL.</param>
        /// <returns></returns>
        private string CreateAspNetUser(MushiDbService service, string userName, string avaterUrl)
        {
            return service.CreateAspNetUser(userName, avaterUrl);
        }
    }
}
