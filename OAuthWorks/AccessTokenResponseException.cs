using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an abstract exception that provides unification between the System.Exception class and the OAuthWorks.IAccessTokenResponseException interface.
    /// </summary>
    [Serializable]
    public abstract class AccessTokenResponseException : Exception, IAccessTokenResponseError
    {
        protected AccessTokenResponseException(string message, Exception innerException) : base(message, innerException) { }

        public abstract AccessTokenRequestError ErrorCode
        {
            get;
        }

        public abstract string ErrorDescription
        {
            get;
        }

        public abstract Uri ErrorUri
        {
            get;
        }
    }
}
