using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Repositories
{
    /// <summary>
    /// Defines an interface for storing <see cref="OAuthWorks.IAuthorizationCode"/> objects.
    /// </summary>
    public interface IAuthorizationCodeRepository<T> : IRepository<string, T> where T : IAuthorizationCode
    {
        /// <summary>
        /// Gets a list of Authorization codes by the given client.
        /// </summary>
        /// <param name="client">The client </param>
        /// <returns></returns>
        IEnumerable<T> GetByClient(IClient client);
    }
}
