using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines a non-generic interface for an object that is identifiable.
    /// </summary>
    public interface IIDentifiable
    {
        /// <summary>
        /// Gets the Id of this object.
        /// </summary>
        object Id
        {
            get;
        }
    }

    /// <summary>
    /// Defines an interface for an object that is identifiable by a certian type.
    /// </summary>
    /// <typeparam name="T">The type of this object's identifier.</typeparam>
    public interface IIDentifiable<T> : IIDentifiable
    {
        /// <summary>
        /// Gets the Id of this object.
        /// </summary>
        new T Id
        {
            get;
        }

    }
}
