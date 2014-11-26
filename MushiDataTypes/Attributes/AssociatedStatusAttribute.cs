using System;
using System.Linq;
using MushiDataTypes.Enums;
using MushiDataTypes.Helpers;

namespace MushiDataTypes.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class AssociatedStatusAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformAttribute"/> class.
        /// </summary>
        /// <param name="platform">The platform.</param>
        public AssociatedStatusAttribute(PlayerStatus status)
        {
            Status = status;
        }

        /// <summary>
        /// Gets or sets the platform.
        /// </summary>
        /// <value>
        /// The platform.
        /// </value>
        public PlayerStatus Status { get; set; }

        /// <summary>
        /// Gets the platform.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public static PlayerStatus? GetAssociatedStatus(object field)
        {
            var attr = AttributeHelper.GetAttributesOfField<AssociatedStatusAttribute>(field).SingleOrDefault();
            if (attr != null)
            {
                return attr.Status;
            }
            return null;
        }
    }
}