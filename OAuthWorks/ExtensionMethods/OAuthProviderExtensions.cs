using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.ExtensionMethods
{
    /// <summary>
    /// Defines a static class that contains extension methods for <see cref="IOAuthProvider"/> objects.
    /// </summary>
    public static class OAuthProviderExtensions
    {
        /// <summary>
        /// Requests an access token from the authorization server based on the given request using the Authorization Code Grant flow (Section 4.1 [RFC 6749] http://tools.ietf.org/html/rfc6749#section-4.1).
        /// </summary>
        /// <param name="request">The request that contains the required values.</param>
        /// <returns>Returns a new <see cref="IAccessTokenResponse"/> object that represents the result of the operation.</returns>
        public static IAccessTokenResponse RequestAccessToken(this IOAuthProvider provider, IAccessTokenRequest request)
        {
            if (provider == null) throw new ArgumentNullException("provider");
            IPasswordCredentialsAccessTokenRequest passRequest;
            IAuthorizationCodeGrantAccessTokenRequest authorizationCodeGrant;
            if ((passRequest = request as IPasswordCredentialsAccessTokenRequest) != null)
            {
                return provider.RequestAccessToken(passRequest);
            }
            else if ((authorizationCodeGrant = request as IAuthorizationCodeGrantAccessTokenRequest) != null)
            {
                return provider.RequestAccessToken(authorizationCodeGrant);
            }
            else
            {
                throw new ArgumentException(string.Format("Cannot process the given request. It must implement 'OAuthWorks.IPasswordCredentialsAccessTokenRequest' or 'OAuthWorks.IAuthorizationCodeGrantAccessTokenRequest'"));
            }
        }
    }
}
