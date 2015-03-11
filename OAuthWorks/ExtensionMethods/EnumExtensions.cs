using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using PortableOAuthWorks.DataAnnotations;

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
	    /// <exception cref="ArgumentNullException">The value of 'value' cannot be null. </exception>
	    public static bool IsSubgroupOf(this Enum value, Enum other)
        {
            if (value == null) throw new ArgumentNullException("value");
			MemberInfo member = GetMember(value.ToString(), value.GetType());
            IEnumerable<EnumSubgroupAttribute> attributes = member.GetCustomAttributes<EnumSubgroupAttribute>(false);
            return attributes.Any(a => a.SubgroupOf.Equals(other));
        }

	    /// <summary>
	    /// Gets the first subgroup that the given enum belongs to and returns it.
	    /// </summary>
	    /// <typeparam name="TEnum">The type of the enum that the value is a subgroup of.</typeparam>
	    /// <param name="value">The enum value to get the subgroup that it belongs to.</param>
	    /// <returns>Returns the enum value of the first subgroup of the given type that this value belongs to.</returns>
	    /// <exception cref="ArgumentNullException">The value of 'value' cannot be null. </exception>
	    public static TEnum GetSubgroup<TEnum>(this Enum value)
        {
            if (value == null) throw new ArgumentNullException("value");
			MemberInfo member = GetMember(value.ToString(), value.GetType());
            IEnumerable<EnumSubgroupAttribute> attributes = member.GetCustomAttributes<EnumSubgroupAttribute>(false);
            return (TEnum)attributes.First(e => e.SubgroupOf is TEnum).SubgroupOf;
        }

	    /// <summary>
	    /// Gets the description of this enum value, returns null if no description attribute was applied.
	    /// </summary>
	    /// <param name="value">The value to get the description of.</param>
	    /// <returns>Returns a string representing the description of the value.</returns>
	    /// <exception cref="ArgumentNullException">The value of 'value' cannot be null. </exception>
	    public static string GetDescription(this Enum value)
        {
            if (value == null) throw new ArgumentNullException("value");
            MemberInfo member = GetMember(value.ToString(), value.GetType());
            DescriptionAttribute a = member.GetCustomAttribute<DescriptionAttribute>();
            return a?.Description;
        }

	    /// <summary>
	    /// Gets the <see cref="EnumMemberAttribute.Value"/> of this enum value.
	    /// </summary>
	    /// <param name="value"></param>
	    /// <returns></returns>
	    /// <exception cref="ArgumentNullException">The value of 'value' cannot be null. </exception>
	    public static string GetValue(this Enum value)
        {
            if (value == null) throw new ArgumentNullException("value");
			MemberInfo member = GetMember(value.ToString(), value.GetType());
            EnumMemberAttribute a = member.GetCustomAttribute<EnumMemberAttribute>();
            return a?.Value;
        }

	    /// <summary>
	    /// Gets the <see cref="MemberInfo"/> for the member with the given name from the given type.
	    /// </summary>
	    /// <param name="memberName">The name of the member that should be retrieved.</param>
	    /// <param name="type">The type that the member should be retrieved from.</param>
	    /// <returns>Returns the <see cref="MemberInfo"/> that represents the reflected member, otherwise null if it could not be found.</returns>
	    private static MemberInfo GetMember(string memberName, Type type)
	    {
			TypeInfo t = type.GetTypeInfo();
		    return t.DeclaredMembers.First(m => m.Name == memberName);
	    }
    }
}
