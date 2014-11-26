using System;
using System.Linq;
using MushiDataTypes.Helpers;
using MushiDataTypes.Enums;

namespace MushiDataTypes.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class PlatformAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlatformAttribute"/> class.
        /// </summary>
        /// <param name="platform">The platform.</param>
        public PlatformAttribute(PlatformsEnum platform)
        {
            Platform = platform;
        }

        /// <summary>
        /// Gets or sets the platform.
        /// </summary>
        /// <value>
        /// The platform.
        /// </value>
        public PlatformsEnum Platform { get; set; }

        /// <summary>
        /// Gets the platform.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public static PlatformsEnum? GetPlatform(object field)
        {
            var attr = AttributeHelper.GetAttributesOfField<PlatformAttribute>(field).SingleOrDefault();
            if (attr != null)
            {
                return attr.Platform;
            }
            return null;
        }
    }
}