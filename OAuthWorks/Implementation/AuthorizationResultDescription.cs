// Copyright 2014 Kallyn Gowdy
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using OAuthWorks.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines a list of values that specify the different authorization result error codes.
    /// </summary>
    [DataContract]
    
    public enum AuthorizationResultErrorType
    {
        /// <summary>
        /// Defines that the given grant could not be found.
        /// </summary>
        [EnumMember(Value = "missing_grant")]
        [DataMember(Name = "missing_grant")]
        [Description("The given authorization grant (Access Token) does not exist.")]
        MissingGrant,

        /// <summary>
        /// Defines that the given grant has been revoked from the client.
        /// </summary>
        [EnumMember(Value = "revoked_grant")]
        [DataMember(Name = "revoked_grant")]
        [Description("The given authorization grant (Access Token) has been revoked and therefore is no longer valid.")]
        RevokedGrant,

        /// <summary>
        /// Defines that the given grant has expired.
        /// </summary>
        [EnumMember(Value = "expired_grant")]
        [DataMember(Name = "expired_grant")]
        [Description("The given authorization grant (Access Token) has expired and therefore is no longer valid. Use the granted refresh token to gain another grant.")]
        ExpiredGrant,

        /// <summary>
        /// Defines that the given grant does not have permissions to access the resource because it has not been granted all of the required scopes.
        /// </summary>
        [EnumMember(Value = "not_enough_permissions")]
        [DataMember(Name = "not_enough_permissions")]
        [Description("The given authorization grant (Access Token) has not been granted access to one or more of the required scopes.")]
        NotEnoughPermissions,

        /// <summary>
        /// Defines that the grant type is unsupported.
        /// </summary>
        [EnumMember(Value = "unsupported_grant_type")]
        [DataMember(Name = "unsupported_grant_type")]
        [Description("The specified type for the grant is not supported for authentication.")]
        UnsupportedGrantType
    }

    /// <summary>
    /// Defines a class that provides a basic implementation of <see cref="IAuthorizationResultDescription"/>.
    /// </summary>
    [DataContract]
    public class AuthorizationResultDescription : IAuthorizationResultDescription
    {
        /// <summary>
        /// Gets the error code that was returned by the server.
        /// </summary>
        /// <returns></returns>
        [DataMember(Name = "error")]
        string IAuthorizationResultDescription.Error
        {
            get
            {
                return Error.GetValue();
            }
        }

        /// <summary>
        /// Gets the error code that was returned by the server.
        /// </summary>
        /// <returns></returns>
        public AuthorizationResultErrorType Error
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the human-readable description of the error.
        /// </summary>
        /// <returns></returns>
        [DataMember(Name = "error_description")]
        public string ErrorDescription
        {
            get
            {
                return Error.GetDescription();
            }
        }

        /// <summary>
        /// Gets the uri that points to a web page that provides a description of the error.
        /// </summary>
        /// <returns></returns>
        [DataMember(Name = "error_uri")]
        public string ErrorUri
        {
            get;
            set;
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", Error, ErrorDescription);
        }
    }
}
