using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface that provides access to processed values that were given in an <see cref="IAuthorizationCodeRequest"/>.
    /// Provides access to resolved <see cref="IClient"/>, <see cref="IUser"/>, and <see cref="IEnumerable{IScope}"/> objects.
    /// </summary>
    public interface IProcessedAuthorizationCodeRequest
    {
        /// <summary>
        /// Gets the <see cref="IClient"/> that made the original <see cref="Request"/>.
        /// </summary>
        /// <returns>Returns a <see cref="IClient"/> object.</returns>
        IClient Client
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="IUser"/> that the request was made for.
        /// </summary>
        /// <returns>Returns a <see cref="IUser"/> object.</returns>
        IUser User
        {
            get;
        }

        /// <summary>
        /// Gets the scopes that the <see cref="Client"/> requested.
        /// </summary>
        /// <returns>Returns an <see cref="IEnumerable{T}"/> of <see cref="IScope"/> objects.</returns>
        IEnumerable<IScope> Scopes
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="IAuthorizationCodeRequest"/> that the <see cref="Client"/> made.
        /// </summary>
        /// <returns>Returns a <see cref="IAuthorizationCodeRequest"/> object.</returns>
        IAuthorizationCodeRequest OriginalRequest
        {
            get;
        }
    }
}
