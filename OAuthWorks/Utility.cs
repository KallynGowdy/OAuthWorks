using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines a list of utility extension methods for objects.
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Throws a new System.ArgumentNullException if the value of the object is null.
        /// </summary>
        /// <param name="value">The object to check for null-ness.</param>
        /// <param name="parameterName">The name of the parameter that is being checked.</param>
        public static void ThrowIfNull(this object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
