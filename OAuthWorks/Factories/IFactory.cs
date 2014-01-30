using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Factories
{
    /// <summary>
    /// Defines an interface for an object factory that produces objects of the given type.
    /// </summary>
    /// <typeparam name="T">The type of the objects that the factory produces.</typeparam>
    public interface IFactory<in T>
    {
        /// <summary>
        /// Gets a new object of the type T from this factory.
        /// </summary>
        /// <returns>Returns a new object of the type T.</returns>
        T Get();

    }
}
