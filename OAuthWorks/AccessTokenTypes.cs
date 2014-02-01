using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines a basic list of token types that are commonly used for access tokens.
    /// </summary>
    public static class AccessTokenTypes
    {
        /// <summary>
        /// Defines that the token is a "bearer" token. That is, anyone possesing the token has access.
        /// </summary>
        /// <remarks>
        /// Often, OAuth providers will require some other form of authentication than just the access token. Ususally by requiring a client
        /// secret(or password) to be provided. This is the method provided by many different
        /// </remarks>
        public const string Bearer = "bearer";

        /// <summary>
        /// Defines that the token is a Message Authentication Code.
        /// </summary>
        public const string Mac = "mac";
    }
}
