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

namespace OAuthWorks
{
    /// <summary>
    /// Defines an abstract class for an IAuthorizationCodeResponseError.
    /// </summary>
    [Serializable]
    public abstract class AuthorizationCodeResponseExceptionBase : Exception, IAuthorizationCodeResponseError
    {
        /// <summary>
        /// The key used to serialize the error code contained in this object.
        /// </summary>
        public const string ErrorCodeKey = "error_code";

        /// <summary>
        /// The key used to serialize the error description contained in this object.
        /// </summary>
        public const string ErrorDescriptionKey = "error_description";

        /// <summary>
        /// The key used to serialize the error uri contained in this object.
        /// </summary>
        public const string ErrorUriKey = "error_uri";

        /// <summary>
        /// The key used to serialize the state contained in this object.
        /// </summary>
        public const string StateKey = "state";

        /// <summary>
        /// Creates a new AuthorizationCodeResponseException object using the given message and the given inner exception.
        /// </summary>
        /// <param name="message">A string describing what caused the exception to occur.</param>
        /// <param name="innerException">An exception that occurred that caused this exception to occur. Can be null.</param>
        protected AuthorizationCodeResponseExceptionBase(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCodeResponseExceptionBase"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        protected AuthorizationCodeResponseExceptionBase(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCodeResponseExceptionBase"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected AuthorizationCodeResponseExceptionBase(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            ErrorCode = (AuthorizationRequestCodeErrorType)info.GetValue(ErrorCodeKey, typeof(AuthorizationRequestCodeErrorType));
            ErrorDescription = info.GetString(ErrorDescriptionKey);
            ErrorUri = new Uri(info.GetString(ErrorUriKey));
            State = info.GetString(StateKey);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationCodeResponseExceptionBase"/> class.
        /// </summary>
        protected AuthorizationCodeResponseExceptionBase()
        {
        }

        /// <summary>
        /// Gets the type of the error that provides information on the basic problem that occured.
        /// </summary>
        public abstract AuthorizationRequestCodeErrorType ErrorCode
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the human-readable ASCII text that provides additional information that is used to assis the client developer
        /// in understanding the error that occurred.
        /// </summary>
        public abstract string ErrorDescription
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets a URI that identifies a human-readable web page with information about the error, used to provide the client developer with additional information about
        /// the error.
        /// </summary>
        public abstract Uri ErrorUri
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the state that was provided by the client in the incomming Authorization Request. REQUIRED ONLY IF the state was provided in the request.
        /// </summary>
        public abstract string State
        {
            get;
            protected set;
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter" />
        /// </PermissionSet>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(ErrorCodeKey, ErrorCode);
            info.AddValue(ErrorDescriptionKey, ErrorDescription);
            info.AddValue(ErrorUriKey, ErrorUri);
            info.AddValue(StateKey, State);
            base.GetObjectData(info, context);
        }
    }
}
