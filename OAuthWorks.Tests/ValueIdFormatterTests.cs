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
using NUnit.Framework;
using OAuthWorks.Implementation;

namespace OAuthWorks.Tests
{
    [TestFixture]
    public class ValueIdFormatterTests
    {
        [TestCase("valueWithoutEscapedCharacters", "idWithoutEscapedCharacters")]
        [TestCase("valueWith-EscapedCharacters", "idWithoutEscapedCharacters")]
        [TestCase("valueWithoutEscapedCharacters", "idWith-EscapedCharacters")]
        [TestCase("valueWith-EscapedCharacters", "idWith-EscapedCharacters")]
        [TestCase("valueWith--DoubleEscapedCharacters", "idWith-EscapedCharacters")]
        [TestCase("valueWith-EscapedCharacters", "idWith--DoubleEscapedCharacters")]
        public void TestFormatAndGetValuesBack(string value, string id)
        {
            IValueIdFormatter<string> formatter = ValueIdFormatter.String.DefaultFormatter;

            string formatted = formatter.FormatValue(id, value);

            Assert.NotNull(formatted);

            string retrievedId = formatter.GetId(formatted);

            Assert.AreEqual(id, retrievedId);

            string retrievedValue = formatter.GetToken(formatted);

            Assert.AreEqual(value, retrievedValue);
        }

    }
}
