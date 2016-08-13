using System;
using System.ComponentModel;
using System.Linq;

namespace Swiss
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Method returns the name of this enum
        /// </summary>
        public static string GetName(this Enum value)
        {
            return Enum.GetName(value.GetType(), value);
        }

        /// <summary>
        /// Method returns the description for this enum
        /// </summary>
        public static string GetDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.GetName());
            var descriptionAttribute = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            return descriptionAttribute == null ? value.GetName() : descriptionAttribute.Description;
        }
    }

}
