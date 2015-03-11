using System;
using System.Runtime.Serialization;

namespace OAuthWorks.Implementation.DataTransferObjects
{
    /// <summary>
    /// Defines a DTO that represents a access token request using the Resource Owner Password Credentials Flow (https://tools.ietf.org/html/rfc6749#section-1.3.3).
    /// </summary>
    public class PasswordCredentialsTokenRequestDto : TokenRequestDto
    {
        private IValidatedUser validatedUser;

        /// <summary>
        /// Gets or sets the username that should be used to retrieve the token.
        /// </summary>
        [DataMember(Name = "username")]
        public string Username
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the password that should be used to retrieve the token.
        /// </summary>
        [DataMember(Name = "password")]
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scope that should be requested.
        /// </summary>
        [DataMember(Name = "scope")]
        public string Scope
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the validated user to the given user.
        /// </summary>
        /// <param name="validatedUser">The user that has been validated.</param>
        public void SetValidatedUser(IValidatedUser validatedUser)
        {
            if (validatedUser == null) throw new ArgumentNullException("validatedUser");
            this.validatedUser = validatedUser;
        }

        /// <summary>
        /// Creates a new <see cref="IAccessTokenRequest"/> object using the given object for client authorization.
        /// </summary>
        /// <param name="clientAuthorization">The object that represents the secret and id for the client.</param>
        /// <returns>Returns a new <see cref="IAccessTokenRequest"/> object that represents the request for a new access token from the Authorization Server.</returns>
        public override IAccessTokenRequest CreateRequest(ClientAuthorizationDto clientAuthorization)
        {
            if (clientAuthorization == null) throw new ArgumentNullException("clientAuthorization");
            if (validatedUser == null) throw new InvalidOperationException("The user must be validated before trying to create a 'IAccessTokenRequest'. 'SetValidatedUser(IValidatedUser)' must be called before trying to create a 'IAccessTokenRequest'.");

            return new PasswordCredentialsAccessTokenRequest(validatedUser, clientAuthorization.ClientId, clientAuthorization.ClientSecret, GrantType, Scope);
        }
    }
}