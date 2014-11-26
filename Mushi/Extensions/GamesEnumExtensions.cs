using MushiDataTypes.Attributes;
using MushiDataTypes.Enums;

namespace Mushi.Extensions
{
    public static class GamesEnumExtension
    {
        /// <summary>
        /// Gets the game platform.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns></returns>
        public static PlatformsEnum? GetGamePlatform(this GamesEnum game)
        {
            return PlatformAttribute.GetPlatform(game);
        }

        /// <summary>
        /// Gets the game platform identifier.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns></returns>
        public static int? GetGamePlatformId(this GamesEnum game)
        {
            var result = PlatformAttribute.GetPlatform(game);
            return (result != null) ? (int)result : (int?)null;
        }
    }
}