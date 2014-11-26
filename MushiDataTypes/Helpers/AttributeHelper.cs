using System;
using System.Linq;

namespace MushiDataTypes.Helpers
{
    public class AttributeHelper
    {
        /// <summary>
        /// Gets the attributes of field.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="field">The field.</param>
        /// <returns></returns>
        public static T[] GetAttributesOfField<T>(object field)
            where T : Attribute
        {
            if (field != null)
            {
                var info = field.GetType().GetField(field.ToString());
                if (info != null)
                {
                    var attrs = info.GetCustomAttributes(typeof(T), false);
                    return attrs.OfType<T>().ToArray();
                }
            }
            return new T[0];
        }
    }
}