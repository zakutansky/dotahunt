using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MushiServices.Context;
using MushiServices.Context.Data;
using Notifications;

namespace MushiServicesHost
{
    public class ProgamerUpdater
    {
        /// <summary>
        /// The email subject
        /// </summary>
        private const string EmailSubject = "Mushi - Dotabuff service";

        /// <summary>
        /// The _email body
        /// </summary>
        private StringBuilder _emailBody = new StringBuilder();

        /// <summary>
        /// The dota buff context
        /// </summary>
        private readonly DotabuffContext _dotaBuffContext = new DotabuffContext();

        /// <summary>
        /// The player context
        /// </summary>
        private readonly PlayerContext _playerContext = new PlayerContext();

        /// <summary>
        /// The players
        /// </summary>
        private List<DotabuffPlayer> _dotaBuffPlayers;

        /// <summary>
        /// The local players
        /// </summary>
        private List<ProgamerWithKey> _localPlayers;

        /// <summary>
        /// The _dotabuff player identifier
        /// </summary>
        private readonly string _dotabuffPlayerId = "46ec4a2f-aca4-4a42-83ff-34dc3ac0c19e";


        /// <summary>
        /// Runs this instance.
        /// </summary>
        public void Run()
        {
            try
            {
                Console.WriteLine("START: GetVerifiedPlayers");
                _dotaBuffPlayers = _dotaBuffContext.GetVerifiedPlayers().ToList();
                Console.WriteLine("SUCCESS: GetVerifiedPlayers");
                Console.WriteLine("START: GetDotabuffPlayer");
                var dotabuffPlayer = _dotaBuffContext.GetDotabuffPlayer(_dotabuffPlayerId);
                if (dotabuffPlayer != null)
                    _dotaBuffPlayers.Add(dotabuffPlayer);
                Console.WriteLine("SUCCESS: GetDotabuffPlayer");
                _localPlayers = _playerContext.GetProgamers().ToList();
            }
            catch (Exception)
            {
                Console.WriteLine("Dotabuff parsing error");
                Email.SendEmail(
                    EmailSubject,
                    "Error occured during parsing of dotabuff website",
                    "adam.zakutansky@seznam.cz",
                    "csepcsar@gmail.com"
                    );
                throw;
            }

            if (!_localPlayers.Any())
            {
                try
                {
                    //TODO opakovani steam dotazu
                    _playerContext.CreateProgamers(_dotaBuffPlayers);
                    return;
                }
                catch (Exception)
                {
                    Console.WriteLine("Progamers insert to DB error");
                    throw;
                }
            }

            var result = _playerContext.GetNewOldPlayers(_dotaBuffPlayers, _localPlayers);
            if (result.OldPlayers.Any())
            {
                _emailBody.Append("Following players are no longer dotabuff verified players:");
                _emailBody.Append(Environment.NewLine);
                foreach (var old in result.OldPlayers)
                {
                    _emailBody.Append(string.Format("{0} [{1}]{2}", old.NickName, old.ProviderKey, Environment.NewLine));
                }
                _emailBody.Append(Environment.NewLine);
            }
            if (result.NewPlayers.Any())
            {
                _playerContext.CreateProgamers(result.NewPlayers);
                _emailBody.Append("NEW VERIFIED PROS:");
                _emailBody.Append(Environment.NewLine);
                foreach (var n in result.NewPlayers)
                {
                    _emailBody.Append(string.Format(" - {0} [{1}]: {2} {3}", n.NickName, n.SteamId, n.ProfileUrl, Environment.NewLine));
                }
                _emailBody.Append(Environment.NewLine);
            }

            var modifiedPlayers = _playerContext.CompareForAnyChanges(_dotaBuffPlayers, _localPlayers);
            if (modifiedPlayers.Any())
            {
                _playerContext.UpdatePlayers(modifiedPlayers);
                _emailBody.Append("Dotabuff progamers changes:");
                _emailBody.Append(Environment.NewLine);
                foreach (var mod in modifiedPlayers)
                {
                    _emailBody.Append(mod);
                    _emailBody.Append(Environment.NewLine);
                }
            }
            if (_emailBody.Length == 0)
                Console.WriteLine("No changes found");
            else
            {
                Console.WriteLine(_emailBody);
                Email.SendEmail(
                    EmailSubject,
                    _emailBody.ToString(),
                    "adam.zakutansky@seznam.cz",
                    "csepcsar@gmail.com"
                    );
            }
            Console.WriteLine("Service end SUCCESFULLY");
        }
    }
}
