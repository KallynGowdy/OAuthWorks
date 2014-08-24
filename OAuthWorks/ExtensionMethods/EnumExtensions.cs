using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.ExtensionMethods
{
    /// <summary>
    /// Defines a class that contains extension methods for <see cref="Enum"/> objects.
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// Determines if this enum value is a child of the given other enum value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="other"></param>
        /// <returns>Returns true if this enum value contains an attribute that defines it as a subgroup of the given other.</returns>
        public static bool IsSubgroupOf(this Enum value, Enum other)
        {
            if (value == null) throw new ArgumentNullException("value");
            Type type = value.GetType();
            MemberInfo member = type.GetMember(value.ToString()).First();
            IEnumerable<EnumSubgroupAttribute> attributes = member.GetCustomAttributes<EnumSubgroupAttribute>(false);
            return attributes.Any(a => a.SubgroupOf.Equals(other));
        }

        /// <summary>
        /// Gets the first subgroup that the given enum belongs to and returns it.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum that the value is a subgroup of.</typeparam>
        /// <param name="value">The enum value to get the subgroup that it belongs to.</param>
        /// <returns>Returns the enum value of the first subgroup of the given type that this value belongs to.</returns>
        public static TEnum GetSubgroup<TEnum>(this Enum value)
        {
            if (value == null) throw new ArgumentNullException("value");
            Type type = value.GetType();
            MemberInfo member = type.GetMember(value.ToString()).First();
            IEnumerable<EnumSubgroupAttribute> attributes = member.GetCustomAttributes<EnumSubgroupAttribute>(false);
            return (TEnum)attributes.Where(e => e.SubgroupOf is TEnum).First().SubgroupOf;
        }

        /// <summary>
        /// Gets the description of this enum value, returns null if no description attribute was applied.
        /// </summary>
        /// <param name="value">The value to get the description of.</param>
        /// <returns>Returns a string representing the description of the value.</returns>
        public static string GetDescription(this Enum value)
        {
            if (value == null) throw new ArgumentNullException("value");
            Type type = value.GetType();
            MemberInfo member = type.GetMember(value.ToString()).First();
            DescriptionAttribute a = member.GetCustomAttribute<DescriptionAttribute>();
            return a != null ? a.Description : null;
        }

        /// <summary>
        /// Gets the <see cref="EnumMemberAttribute.Value"/> of this enum value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetValue(this Enum value)
        {
            if (value == null) throw new ArgumentNullException("value");
            Type type = value.GetType();
            MemberInfo member = type.GetMember(value.ToString()).First();
            EnumMemberAttribute a = member.GetCustomAttribute<EnumMemberAttribute>();
            return a != null ? a.Value : null;
        }
    }
}
