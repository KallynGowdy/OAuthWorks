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
