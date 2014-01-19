﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface that contains values and contextual information that is sent in the Authorization Response
    /// of the Authorization Code Grant flow in the OAuth 2.0 spec.
    /// (RFC 6749, Section 4.1.2, http://tools.ietf.org/html/rfc6749#section-4.1.2)
    /// </summary>
    public interface IAuthorizationCodeResponse
    {
        // The Specification states that several values can be included in the response.
        // These are required:
        //
        // -- code,     string,     The authorization code generated by the
        //                          authorization server.  The authorization code MUST expire
        //                          shortly after it is issued to mitigate the risk of leaks.  A
        //                          maximum authorization code lifetime of 10 minutes is
        //                          RECOMMENDED.  The client MUST NOT use the authorization code
        //                          more than once.  If an authorization code is used more than
        //                          once, the authorization server MUST deny the request and SHOULD
        //                          revoke (when possible) all tokens previously issued based on
        //                          that authorization code.  The authorization code is bound to
        //                          the client identifier and redirection URI.
        //
        // -- state,    string,     The state that was sent by the client in the request. REQUIRED ONLY IF
        //                          the state was sent in the request.
        //

        /// <summary>
        /// Gets the code that was generated by the authorization server. REQUIRED.
        /// </summary>
        string Code
        {
            get;
        }

        /// <summary>
        /// Gets the state that was sent by the client in the Authorization Request.REQUIRED ONLY IF the state was sent in the request.
        /// </summary>
        string State
        {
            get;
        }

        /// <summary>
        /// Gets whether an error occured while processing the request.
        /// </summary>
        bool IsError
        {
            get;
        }

        /// <summary>
        /// Gets an object that represents the actual error that occured.
        /// </summary>
        IAuthorizationCodeError Error
        {
            get;
        }
    }
}
