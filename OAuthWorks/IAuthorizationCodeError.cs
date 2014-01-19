using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface that contains values that describe the problem that happened while trying to authorize the client. (RFC 6749, Section 4.1.2.1, http://tools.ietf.org/html/rfc6749#section-4.1.2.1)
    /// </summary>
    public interface IAuthorizationCodeError
    {
        // The specification defines values that are required to be present, including:
        //
        // -- error,                Enum,       An error code that describes the basic problem that occured.
        //                                      Must be one of the following,
        //                                       -- unauthorized_client
        //                                       -- access_denied
        //                                       -- unsupported_response_type
        //                                       -- invalid_scope
        //                                       -- server_error
        //                                       -- temporarily_unavailable
        //
        // -- state,                string,     The state that was provided by the client in the request. REQUIRED ONLY IF the state
        //                                      was provided in the request.
        //
        // Other values are optional:
        //
        // -- error_description,    string,     The human-readable ASCII text that describes what went wrong,
        //                                      used to help the client developer understand the exact problem and how to fix it.
        //
        // -- error_uri,            string,     A URI identifying a human-readable web page with information about the error, used
        //                                      to provide the client developer with additional information about the error.

        /// <summary>
        /// Gets the type of the error that provides information on the basic problem that occured.
        /// </summary>
        AuthorizationRequestCodeErrorType Error
        {
            get;
        }

        /// <summary>
        /// Gets the human-readable ASCII text that provides additional information that is used to assis the client developer
        /// in understanding the error that occurred.
        /// </summary>
        string ErrorDescription
        {
            get;
        }

        /// <summary>
        /// Gets a URI that identifies a human-readable web page with information about the error, used to provide the client developer with additional information about
        /// the error.
        /// </summary>
        string ErrorUri
        {
            get;
        }

        /// <summary>
        /// Gets the state that was provided by the client in the incomming Authorization Request. REQUIRED ONLY IF the state was provided in the request.
        /// </summary>
        string State
        {
            get;
        }

    }
}
