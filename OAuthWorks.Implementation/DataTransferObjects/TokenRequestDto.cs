using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation.DataTransferObjects
{
    /// <summary>
    /// Defines an abstract base class that represents a request to the OAuthProvider's token endpoint.
    /// </summary>
    [DataContract]
    public abstract class TokenRequestDto
    {
        /// <summary>
        /// Gets or sets the grant_type that specifies how the token will be obtained.
        /// </summary>
        /// <returns></returns>
        [DataMember(Name = "grant_type")]
        public string GrantType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the ID of the client that is requesting the token.
        /// </summary>
        /// <returns></returns>
        [DataMember(Name = "client_id")]
        public string ClientId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the secret that should be used to authenticate the client.
        /// </summary>
        /// <returns></returns>
        [DataMember(Name = "client_secret")]
        public string ClientSecret
        {
            get;
            set;
        }

        /// <summary>
        /// Creates a new <see cref="IAccessTokenRequest"/> object using the given object for client authorization.
        /// </summary>
        /// <param name="clientAuthorization">The object that represents the secret and id for the client.</param>
        /// <returns>Returns a new <see cref="IAccessTokenRequest"/> object that represents the request for a new access token from the Authorization Server.</returns>
        public abstract IAccessTokenRequest CreateRequest(ClientAuthorizationDto clientAuthorization);
    }
}
