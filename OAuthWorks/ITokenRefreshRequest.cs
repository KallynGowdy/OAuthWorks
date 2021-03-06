﻿// Copyright 2014 Kallyn Gowdy
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
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks
{
    /// <summary>
    /// Defines an interface for an object that represents a request for a new access refreshToken through the use of a refresh refreshToken.
    /// </summary>
    [ContractClass(typeof(ITokenRefreshRequestContract))]
    public interface ITokenRefreshRequest : IAccessTokenRequest
    {
        /// <summary>
        /// Gets the refresh refreshToken that was provided by the client.
        /// </summary>
        string RefreshToken
        {
            get;
        }
    }

    [ContractClassFor(typeof(ITokenRefreshRequest))]
    internal abstract class ITokenRefreshRequestContract : ITokenRefreshRequest
    {

        string ITokenRefreshRequest.RefreshToken
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                return default(string);
            }
        }

        string IAccessTokenRequest.GrantType
        {
            get
            {
                return default(string);
            }
        }

        string IAccessTokenRequest.ClientId
        {
            get
            {
                return default(string);
            }
        }

        Uri IAccessTokenRequest.RedirectUri
        {
            get
            {
                return default(Uri);
            }
        }

        string IAccessTokenRequest.ClientSecret
        {
            get
            {
                return default(string);
            }
        }

        string IAccessTokenRequest.Scope
        {
            get
            {
                return default(string);
            }
        }
    }
}
