using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using MushiServices.Context.Data;
using MushiServices.DbServices;
using MushiServices.Helpers;
using Steam;

namespace MushiServices.Context
{
    public class DotabuffContext
    {
        /// <summary>
        /// The _verifiedPlayersHtml
        /// </summary>
        private readonly HtmlDocument _verifiedPlayersHtml;

        /// <summary>
        /// The dotabuff URL
        /// </summary>
        private const string VerifiedPlayersUrl = @"http://www.dotabuff.com/players/verified";

        /// <summary>
        /// Initializes a new instance of the <see cref="DotabuffContext"/> class.
        /// </summary>
        public DotabuffContext()
        {
            using (var client = new WebClient())
            {
                _verifiedPlayersHtml = HtmlHelper.DownloaDocument(VerifiedPlayersUrl, client);
            }
        }

        /// <summary>
        /// Gets the verified players.
        /// </summary>
        /// <returns></returns>
        /// Exceptions:
        /// System.ArgumentNullException
        public IEnumerable<DotabuffPlayer> GetVerifiedPlayers()
        {
            var players = _verifiedPlayersHtml.DocumentNode.SelectSingleNode("//table")
                .Descendants("tr")
                .Skip(1)
                .Where(tr => tr.Elements("td").Count() > 1)
                .Select(o =>
                    new DotabuffPlayer
                    {
                        ProfileUrl =
                            "http://www.dotabuff.com/" + o.FirstChild.FirstChild.FirstChild.Attributes["href"].Value,
                        NickName = o.ChildNodes[1].FirstChild.InnerText
                    })
                    .ToList();
            SetSteamId(players);
            AdjustNickNames(players);
            return players;
        }

        /// <summary>
        /// Sets the steam identifier.
        /// </summary>
        /// <param name="players">The player.</param>
        private void SetSteamId(IEnumerable<DotabuffPlayer> players)
        {
            if (players != null)
            {
                using (var client = new WebClient())
                {
                    foreach (var player in players)
                    {
                        var profileHtml = HtmlHelper.DownloaDocument(player.ProfileUrl, client);
                        var claimedId = profileHtml.DocumentNode.SelectNodes("//a[@href]")
                            .First(o =>
                                o.Attributes["href"].Value.Contains("steamcommunity")
                            )
                            .Attributes["href"]
                            .Value;
                        var steamId = SteamHelpers.GetSteamIdFromDotabuff(claimedId);
                        player.SteamId = steamId;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the dotabuff player.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public DotabuffPlayer GetDotabuffPlayer(string userId)
        {
            using (var service = new MushiDbService())
            {
                var player = service.GetProPlayer(userId);
                if (player != null)
                {
                    return new DotabuffPlayer
                    {
                        NickName = player.UserName,
                        SteamId = player.ProviderKey,
                        ProfileUrl = player.ProfileUrl
                    };
                }
                return null;
            }
        }

        /// <summary>
        /// Adjusts the nick names.
        /// </summary>
        /// <param name="players">The players.</param>
        private void AdjustNickNames(List<DotabuffPlayer> players)
        {
            players.ForEach(o =>
            {
                o.NickName = o.NickName.Replace("&gt;", ">");
                o.NickName = o.NickName.Replace("&lt;", "<");
            });
        }
    }
}
