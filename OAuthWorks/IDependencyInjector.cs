using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for an object that retrieves other objects based on type.
    /// </summary>
    public interface IDependencyInjector
    {
        /// <summary>
        /// Gets a new object of the given type.
        /// </summary>
        /// <typeparam name="T">The type of the object to retrieve.</typeparam>
        /// <returns>
        /// Returns a new object of the given type if the dependency injector can provide that type.
        /// Returns null if the dependency injector does not support the given type.
        /// </returns>
        T GetInstance<T>();
    }
}
