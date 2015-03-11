using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation.DataTransferObjects
{
    /// <summary>
    /// Defines a class that represents a literal request for an access token at the token endpoint using the Authorization Code Grant flow.
    /// </summary>
    [DataContract]
    public class AuthorizationCodeGrantTokenRequestDto : TokenRequestDto
    {
        /// <summary>
        /// Gets or sets the code provided by the client in the request.
        /// </summary>
        /// <returns></returns>
        [DataMember(Name = "code")]
        public string Code
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the <see cref="Uri"/> that was included in the authorization request.
        /// </summary>
        /// <returns></returns>
        [DataMember(Name = "redirect_uri")]
        public Uri RedirectUri
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new <see cref="AuthorizationCodeGrantAccessTokenRequest"/> from this DTO.
        /// </summary>
        /// <returns></returns>
        public override IAccessTokenRequest CreateRequest(ClientAuthorizationDto authorization)
        {
            return new AuthorizationCodeGrantAccessTokenRequest(
                authorizationCode: Code,
                clientId: authorization.ClientId ?? ClientId,
                clientSecret: authorization.ClientSecret ?? ClientSecret,
                redirectUri: RedirectUri);
        }
    }
}
