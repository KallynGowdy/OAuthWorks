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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines a class which provides a basic implementation of <see cref="IUnsuccessfulAuthorizationCodeResponse"/>.
    /// </summary>
    [DataContract]
    public class UnsuccessfulAuthorizationCodeResponse : IUnsuccessfulAuthorizationCodeResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulAuthorizationCodeResponse" /> class.
        /// </summary>
        /// <param name="errorCode">The error code that represents the basic problem that occurred.</param>
        /// <param name="state">The state that was sent by the client in the request.</param>
        /// <param name="redirectUri">The redirect URI.</param>
        /// <param name="client">The client that issued the request.</param>
        /// <param name="user">The user that the client issued the request for.</param>
        /// <param name="scopes">The scopes that the client requested.</param>
        public UnsuccessfulAuthorizationCodeResponse(AuthorizationCodeRequestSpecificErrorType errorCode, string state, Uri redirectUri, IClient client, IUser user, IEnumerable<IScope> scopes)
            : this(errorCode, state, redirectUri, null, null, client, user, scopes)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulAuthorizationCodeResponse" /> class.
        /// </summary>
        /// <param name="errorCode">The error code that represents the basic problem that occurred.</param>
        /// <param name="state">The state that was sent by the client in the request.</param>
        /// <param name="redirectUri">The redirect URI.</param>
        public UnsuccessfulAuthorizationCodeResponse(AuthorizationCodeRequestSpecificErrorType errorCode, string state, Uri redirectUri)
            : this(errorCode, state, redirectUri, null, null, null, null, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulAuthorizationCodeResponse"/> class.
        /// </summary>
        /// <param name="errorCode">The error code that represents the basic problem that occurred.</param>
        /// <param name="state">The state that was sent by the client in the request.</param>
        /// <param name="errorDescription">The human-readable description of the error.</param>
        public UnsuccessfulAuthorizationCodeResponse(AuthorizationCodeRequestSpecificErrorType errorCode, string state, Uri redirectUri, string errorDescription)
            : this(errorCode, state, redirectUri, errorDescription, null, null, null, null)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnsuccessfulAuthorizationCodeResponse"/> class.
        /// </summary>
        /// <param name="errorCode">The error code that represents the basic problem that occurred.</param>
        /// <param name="state">The state that was sent by the client in the request.</param>
        /// <param name="errorDescription">The human-readable description of the error.</param>
        /// <param name="errorUri">A URI that points to a human-readable web page that describes the error.</param>
        /// <param name="client">The client that issued the request.</param>
        /// <param name="user">The user that the client issued the request for.</param>
        /// <param name="scopes">The scopes that the client requested.</param>
        public UnsuccessfulAuthorizationCodeResponse(AuthorizationCodeRequestSpecificErrorType errorCode, string state, Uri redirectUri, string errorDescription, Uri errorUri, IClient client, IUser user, IEnumerable<IScope> scopes)
        {
            this.Client = client;
            this.User = user;
            this.Scopes = scopes;
            this.SpecificErrorCode = errorCode;
            this.ErrorCode = SpecificErrorCode.GetSubgroup<AuthorizationCodeRequestErrorType>();
            this.State = state;
            this.ErrorDescription = errorDescription;
            this.ErrorUri = errorUri;
            if (redirectUri != null)
            {
                this.Redirect = new Uri(redirectUri, new
                {
                    error = ErrorCode,
                    state = State,
                    error_description = ErrorDescription,
                    error_uri = ErrorUri
                }.ToQueryString());
            }
            else
            {
                this.Redirect = null;
            }
        }

        /// <summary>
        /// Gets the client that issued the request.
        /// </summary>
        /// <remarks>
        /// When implementing, make sure to mark this property as not serializable to prevent any information from being leaked.
        /// </remarks>
        /// <returns>Returns the <see cref="IClient" /> object that represents the client that issued the Authorization Code Request.</returns>
        public IClient Client
        {
            get;
            private set;
        }


        /// <summary>
        /// Gets the type of the error that provides information on the basic problem that occured.
        /// </summary>
        [DataMember(Name = "error_code")]
        public AuthorizationCodeRequestErrorType ErrorCode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the human-readable ASCII text that provides additional information that is used to assis the client developer
        /// in understanding the error that occurred.
        /// </summary>
        [DataMember(Name = "error_description")]
        public string ErrorDescription
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a URI that identifies a human-readable web page with information about the error, used to provide the client developer with additional information about
        /// the error.
        /// </summary>
        [DataMember(Name = "error_uri")]
        public Uri ErrorUri
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets whether the request for an authorization code was successful.
        /// </summary>
        /// <value>false</value>
        /// <returns>Returns whether the request was successful.</returns>
        public bool IsSuccessful
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the 
        /// <see cref="Uri" /> that specifies where the user should be redirected to. 
        /// This value should contain all of the values needed for a successful OAuth 2.0 authorization code redirect. (Section 4.1.2 [RFC 6749] http://tools.ietf.org/html/rfc6749#section-4.1.2)
        /// </summary>
        /// <returns></returns>
        public Uri Redirect
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the list of parsed scopes that were requested from the user.
        /// </summary>
        /// <remarks>
        /// When implementing, make sure to mark this property as not serializable to prevent any information from being leaked.
        /// </remarks>
        /// <returns>Returns a <see cref="IEnumerable{IScope}" /> object that enumerates the list of scopes that were requested from the user.</returns>
        public IEnumerable<IScope> Scopes
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the sepecific error that occured, providing more information about what happened.
        /// </summary>
        /// <returns>Returns a <see cref="AuthorizationCodeRequestSpecificErrorType" /> object that represents the problem that occured.</returns>
        [DataMember(Name = "specific_error_code")]
        public AuthorizationCodeRequestSpecificErrorType SpecificErrorCode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the state that was provided by the client in the incomming Authorization Request. REQUIRED ONLY IF the state was provided in the request.
        /// </summary>
        [DataMember(Name = "state")]
        public string State
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the user whose account the request was for.
        /// </summary>
        /// <remarks>
        /// When implementing, make sure to mark this property as not serializable to prevent any information from being leaked.
        /// </remarks>
        /// <returns>Returns the <see cref="IUser" /> object that the Authorization Code Request was supposed to grant access to.</returns>
        public IUser User
        {
            get;
            private set;
        }
    }
}
