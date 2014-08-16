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

using OAuthWorks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace ExampleMvcWebApplication.Models
{
    public class Client : IClient
    {
        /// <summary>
        /// Gets the name of the client.
        /// </summary>
        [Key]
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the list of allowed redirect URIs for this client.
        /// </summary>
        public virtual ICollection<RedirectUri> RedirectUris
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the list of allowed redirect URIs for this client.
        /// </summary>
        IEnumerable<Uri> IClient.RedirectUris
        {
            get
            {
                return this.RedirectUris.Select(uri => new Uri(uri.Uri));
            }
        }

        /// <summary>
        /// Gets or sets the secret.
        /// </summary>
        /// <value>
        /// The secret.
        /// </value>
        public string Secret
        {
            get;
            set;
        }

        /// <summary>
        /// Determines if the given redirect uri is valid.
        /// </summary>
        /// <param name="redirectUri">The redirect uri to determine validity/qualification for.</param>
        /// <returns>
        /// Returns true if the redirect uri is registered, otherwise false.
        /// </returns>
        public bool IsValidRedirectUri(Uri redirectUri)
        {
            return RedirectUris.Any(r => r.Uri.Equals(redirectUri.ToString()));
        }

        /// <summary>
        /// Determines if the given secret matches the one stored internally.
        /// </summary>
        /// <param name="secret">The secret to match to the internal one.</param>
        /// <returns>
        /// Returns true if the secrets match, otherwise false.
        /// </returns>
        public bool MatchesSecret(string secret)
        {
            return this.Secret.Equals(secret, StringComparison.Ordinal);
        }

        /// <summary>
        /// Determines if this <see cref="Client"/> object equals the given <see cref="IClient"/> object.
        /// </summary>
        /// <param name="other">The <see cref="IClient"/> object to compare with this object.</param>
        /// <returns>
        /// Returns true if this object object is equal to the other object, otherwise false.
        /// </returns>
        public bool Equals(IClient other)
        {
            return other != null &&
                this.Name.Equals(other.Name);
        }
    }
}
