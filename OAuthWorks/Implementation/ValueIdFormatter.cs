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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OAuthWorks.Implementation
{
    /// <summary>
    /// Defines a static class that acts as a container class for implementations of <see cref="IValueIdFormatter{TId}"/>.
    /// </summary>
    public static class ValueIdFormatter
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        /// <summary>
        /// Defines a class that provides a basic implementation of <see cref="OAuthWorks.IValueIdFormatter{string}"/>.
        /// </summary>
        public class Int : IValueIdFormatter<int>
        {
            private static readonly Lazy<IValueIdFormatter<int>> defaultFormatter = new Lazy<IValueIdFormatter<int>>(() => new Int());

            /// <summary>
            /// Gets the singleton <see cref="IValueIdFormatter{int}"/> object used to format IDs and values into one string.
            /// </summary>
            /// <returns></returns>
            public static IValueIdFormatter<int> DefaultFormatter
            {
                get
                {
                    return defaultFormatter.Value;
                }
            }

            /// <summary>
            /// Gets the divider that this formatter uses to join the ID and Value strings.
            /// </summary>
            /// <returns></returns>
            public char Divider
            {
                get;
                private set;
            } = '-';

            /// <summary>
            /// Formats the given Id and refreshToken into one value that is returned.
            /// </summary>
            /// <param name="id">The Id that should be integrated into the given refreshToken.</param>
            /// <param name="refreshToken">The refreshToken that the Id should be integrated into.</param>
            /// <returns>
            /// Returns a new string that, contains both the given refreshToken and Id in a way that they're both easily retrievable.
            /// </returns>
            public string FormatValue(int id, string token)
            {

                return Escape(token, Divider, Divider) + Divider + Escape(id.ToString(), Divider, Divider);
            }

            /// <summary>
            /// Gets the Id value that is stored in the given formatted value.
            /// </summary>
            /// <param name="formattedToken">A value that was generated using <see cref="OAuthWorks.IValueIdFormatter.FormatValue(System.String, System.String)" />.</param>
            /// <returns>
            /// Returns the Id value that was stored in the given formatted value.
            /// </returns>
            public int GetId(string formattedToken)
            {
                if (string.IsNullOrEmpty(formattedToken))
                {
                    throw new ArgumentException("The formatted token can't be null or empty.", "formattedToken");
                }
                Regex r = new Regex(string.Format(@"(?<!{0}){0}(?!{0})", Divider));
                return int.Parse(Unescape(r.Split(formattedToken).Last(), Divider, Divider));
            }

            /// <summary>
            /// Gets the refreshToken value that is stored in the given formatted value.
            /// </summary>
            /// <param name="formattedToken">A value that was generated using <see cref="OAuthWorks.IValueIdFormatter.FormatValue(System.String, System.String)" />.</param>
            /// <returns>
            /// Returns the token value that was stored in the given formatted value.
            /// </returns>
            public string GetToken(string formattedToken)
            {
                if (string.IsNullOrEmpty(formattedToken))
                {
                    throw new ArgumentException("The formatted token can't be null or empty.", "formattedToken");
                }
                Regex r = new Regex(string.Format(@"(?<!{0}){0}(?!{0})", Divider));
                return Unescape(r.Split(formattedToken).First(), Divider, Divider);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "String")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        /// <summary>
        /// Defines a class that provides a basic implementation of <see cref="OAuthWorks.IValueIdFormatter{string}"/>.
        /// </summary>
        public class String : IValueIdFormatter<string>
        {
            private static readonly Lazy<IValueIdFormatter<string>> defaultFormatter = new Lazy<IValueIdFormatter<string>>(() => new String());

            /// <summary>
            /// Gets the singleton <see cref="IValueIdFormatter{string}"/> object used to format IDs and values into one string.
            /// </summary>
            /// <returns></returns>
            public static IValueIdFormatter<string> DefaultFormatter
            {
                get
                {
                    return defaultFormatter.Value;
                }
            }

            /// <summary>
            /// Gets the divider that this formatter uses to join the ID and Value strings.
            /// </summary>
            /// <returns></returns>
            public char Divider
            {
                get;
                private set;
            }
            = '-';

            /// <summary>
            /// Formats the given Id and token into one value that is returned.
            /// </summary>
            /// <param name="id">The Id that should be integrated into the given refreshToken.</param>
            /// <param name="refreshToken">The token that the Id should be integrated into.</param>
            /// <returns>
            /// Returns a new string that, contains both the given token and Id in a way that they're both easily retrievable.
            /// </returns>
            public string FormatValue(string id, string token)
            {
                return Escape(token, Divider, Divider) + Divider + Escape(id, Divider, Divider);
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
                if (string.IsNullOrEmpty(formattedToken))
                {
                    throw new ArgumentException("The formatted token can't be null or empty.", "formattedToken");
                }
                Regex r = new Regex(string.Format(@"(?<!{0}){0}(?!{0})", Divider));
                return Unescape(r.Split(formattedToken).Last(), Divider, Divider);
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
                if (string.IsNullOrEmpty(formattedToken))
                {
                    throw new ArgumentException("The formatted token can't be null or empty.", "formattedToken");
                }
                Regex r = new Regex(string.Format(@"(?<!{0}){0}(?!{0})", Divider));
                return Unescape(r.Split(formattedToken).First(), Divider, Divider);
            }
        }

        /// <summary>
        /// Escapes the specified characters in the given string by preceding them with the specified escape character.
        /// </summary>
        /// <param name="str">The string to escape.</param>
        /// <param name="chars">The characters to escape.</param>
        /// <param name="escapeChar">The character to use as the identifier of an escape sequence.</param>
        /// <returns>Returns the original string with the specified characters escaped.</returns>
        private static string Escape(string str, char escapeChar, params char[] chars)
        {
            chars = chars.Concat(new char[] { escapeChar }).ToArray();
            StringBuilder newString = new StringBuilder(str);

            for (int i = 0; i < newString.Length; i++)
            {
                char c = newString[i];
                if (chars.Contains(c))
                {
                    newString.Insert(i, c);
                    i++;
                }
            }
            return newString.ToString();
        }

        /// <summary>
        /// Unescapes the specified characters in the given string by removing proceeding the proceeding given escape character.
        /// </summary>
        /// <param name="str">The string to unescape</param>
        /// <param name="escapeChar">The character that was used to escape everything.</param>
        /// <param name="chars">The characters to unescape.</param>
        /// <returns>Returns the given string with the specified characters unescaped.</returns>
        private static string Unescape(string str, char escapeChar, params char[] chars)
        {
            chars = chars.Concat(new char[] { escapeChar }).ToArray();
            StringBuilder newString = new StringBuilder(str);
            for (int i = 0; i < newString.Length - 1; i++)
            {
                char c = newString[i];
                char next = newString[i + 1];
                if (c == escapeChar && chars.Contains(next))
                {
                    newString.Remove(i, 1);
                }
            }
            return newString.ToString();
        }
    }
}
