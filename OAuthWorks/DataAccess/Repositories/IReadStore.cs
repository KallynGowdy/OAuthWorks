using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.DataAccess.Repositories
{
    /// <summary>
    /// Defines an interface for an object that can read other objects from it's store but does not need to provide Write operations.
    /// </summary>
    /// <typeparam name="TValue">The type of the values that can be read from this store.</typeparam>
    /// <typeparam name="TKey">The type of the keys/ids that are used to get values from this store.</typeparam>
    public interface IReadStore<in TKey, out TValue> : IDisposable
    {
        /// <summary>
        /// Gets an entity by it's identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to retrieve.</param>
        /// <returns>Returns the requested entity if it is contained in the repository, otherwise null.</returns>
        TValue GetById(TKey id);
    }
}
