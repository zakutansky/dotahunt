
namespace MushiServices.Context.Data
{
    public class ModifiedPlayer
    {
        public string UserId { get; set; }
        public string SteamId { get; set; }

        public string OldNickName { get; set; }

        public string NewNickName { get; set; }

        public string NewProfileUrl { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(NewNickName) && !string.IsNullOrEmpty(NewProfileUrl))
                return string.Format("{0} [{1}] changed nickname to: {2} and has new profile url: {3}",OldNickName, SteamId, NewNickName, NewProfileUrl);
            if(!string.IsNullOrEmpty(NewNickName))
                return string.Format("{0} [{1}] changed nickname to: {2}",OldNickName, SteamId, NewNickName);
            if (!string.IsNullOrEmpty(NewProfileUrl))
                return string.Format("{0} [{1}] has new profile url {2}", OldNickName, SteamId, NewProfileUrl);
            return base.ToString();
        }
    }
}
