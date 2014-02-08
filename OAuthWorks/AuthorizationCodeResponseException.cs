using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an abstract class for an IAuthorizationCodeResponseError.
    /// </summary>
    [Serializable]
    public abstract class AuthorizationCodeResponseException : Exception, IAuthorizationCodeResponseError
    {
        /// <summary>
        /// Creates a new AuthorizationCodeResponseException object using the given message and the given inner exception.
        /// </summary>
        /// <param name="message">A string describing what caused the exception to occur.</param>
        /// <param name="innerException">An exception that occurred that caused this exception to occur. Can be null.</param>
        protected AuthorizationCodeResponseException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AuthorizationCodeResponseException(string message) : base(message) { }

        public abstract AuthorizationRequestCodeErrorType ErrorCode
        {
            get;
            protected set;
        }

        public abstract string ErrorDescription
        {
            get;
            protected set;
        }

        public abstract Uri ErrorUri
        {
            get;
            protected set;
        }

        public abstract string State
        {
            get;
            protected set;
        }
    }
}
