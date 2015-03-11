using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableOAuthWorks.DataAnnotations
{
	/// <summary>
	/// Defines an attribute that is used to provide an accessible description of a type member (field, property or method) at runtime.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
	public sealed class DescriptionAttribute : Attribute
	{
		/// <summary>
		/// Gets the description for the decorated type member.
		/// </summary>
		public string Description
		{
			get;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DescriptionAttribute"/>.
		/// </summary>
		/// <param name="description">The description for the member.</param>
		/// <exception cref="ArgumentNullException">The value of 'description' cannot be null. </exception>
		public DescriptionAttribute(string description)
		{
			if (description == null) throw new ArgumentNullException("description");
			this.Description = description;
		}

		public override string ToString()
		{
			return Description;
		}
	}
}
