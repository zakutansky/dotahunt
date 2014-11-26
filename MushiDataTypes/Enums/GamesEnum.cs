using MushiDataTypes.Attributes;

namespace MushiDataTypes.Enums
{
    public enum GamesEnum
    {
        NotSelected,

        [Platform(PlatformsEnum.Steam)]
        Dota2,

        [Platform(PlatformsEnum.Lol)]
        Lol
    }
}