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
using System.Text;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines a class that provides a basic implementation of <see cref="OAuthWorks.IValueIdFormatter"/>.
    /// </summary>
    public class ValueIdFormatter : IValueIdFormatter
    {
        /// <summary>
        /// Formats the given Id and token into one value that is returned.
        /// </summary>
        /// <param name="id">The Id that should be integrated into the given token.</param>
        /// <param name="token">The token that the Id should be integrated into.</param>
        /// <returns>
        /// Returns a new string that, contains both the given token and Id in a way that they're both easily retrievable.
        /// </returns>
        public string FormatValue(string id, string token)
        {
            return token + '-' + id;
        }

        /// <summary>
        /// Gets the Id value that is stored in the given formatted value.
        /// </summary>
        /// <param name="formattedToken">A value that was generated using <see cref="OAuthWorks.IValueIdFormatter.FormatValue(System.String, System.String)" />.</param>
        /// <returns>
        /// Returns the Id value that was stored in the given formatted value.
        /// </returns>
        public string GetId(string formattedToken)
        {
            return formattedToken.Split('-').Last();
        }

        /// <summary>
        /// Gets the token value that is stored in the given formatted value.
        /// </summary>
        /// <param name="formattedToken">A value that was generated using <see cref="OAuthWorks.IValueIdFormatter.FormatValue(System.String, System.String)" />.</param>
        /// <returns>
        /// Returns the token value that was stored in the given formatted value.
        /// </returns>
        public string GetToken(string formattedToken)
        {
            return formattedToken.Split('-').First();
        }
    }
}
