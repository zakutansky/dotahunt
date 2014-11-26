using System;
using System.Collections.Generic;
using System.Linq;
using MushiDataTypes.Enums;
using MushiServices.Context.Data;

namespace MushiServices.DbServices
{
    public class MushiDbService : DisposableService
    {
        /// <summary>
        /// Gets the pro players.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ProgamerWithKey> GetProPlayers()
        {
            var progamers = Entities.ProgamersViews.Select(o => 
                new ProgamerWithKey
                {
                    UserId = o.Id,
                    NickName = o.UserName,
                    ProviderKey = o.ProviderKey,
                    ProfileUrl = o.ProfileUrl
                }
            );
            return progamers;
        }

        /// <summary>
        /// Gets the pro player.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public ProgamersView GetProPlayer(string userId)
        {
            return Entities.ProgamersViews.SingleOrDefault(o => o.Id == userId);
        }


        /// <summary>
        /// Creates the progamer.
        /// </summary>
        /// <param name="playerId">The player identifier.</param>
        /// <param name="profileUrl">The profile URL.</param>
        public void CreateProgamer(string playerId, string profileUrl)
        {
            Entities.Progamers.Add(
                new Progamer
                {
                    UserId = playerId,
                    ProfileUrl = profileUrl,
                    Price = 0,
                    Status = (int)PlayerStatus.Offline
                }
            );
            Entities.SaveChanges();
        }

        /// <summary>
        /// Creates the ASP net user.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="avatarUrl">The avatar URL.</param>
        /// <returns></returns>
        public string CreateAspNetUser(string userName, string avatarUrl)
        {
            var user = new AspNetUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = userName,
                AvatarUrl = avatarUrl,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            Entities.AspNetUsers.Add(user);
            Entities.SaveChanges();
            return user.Id;
        }

        public void CreateAspNetUserLogin(string userId, string provider, string providerKey)
        {
            var login = new AspNetUserLogin
            {
                UserId = userId,
                LoginProvider = provider,
                ProviderKey = providerKey
            };
            Entities.AspNetUserLogins.Add(login);
            Entities.SaveChanges();
        }

        public void UpdatePlayerInfo(string playerId, string playerName)
        {
            var player = Entities.AspNetUsers.SingleOrDefault(o => o.Id == playerId);
            if (player != null)
            {
                player.UserName = playerName;
                Entities.SaveChanges();
            }
        }

        public void UpdatePlayerAvatar(string playerId, string avatarUrl)
        {
            var player = Entities.AspNetUsers.SingleOrDefault(o => o.Id == playerId);
            if (player != null)
            {
                player.AvatarUrl = avatarUrl;
                Entities.SaveChanges();
            }
        }
    }
}
