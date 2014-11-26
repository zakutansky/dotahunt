using MushiDataTypes.Attributes;
using MushiDataTypes.Enums;

namespace Mushi.Extensions
{
    public static class GameOrdersStatusExtensions
    {
        /// <summary>
        /// Gets the associated status.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        public static PlayerStatus? GetAssociatedStatus(this GameOrderStatesEnum status)
        {
            return AssociatedStatusAttribute.GetAssociatedStatus(status);
        }
    }
}