using System;
using Xunit;
using Utils.Extensions;

namespace Utils.Tests
{
    public class StringExtensionsTests
    {
        [Fact]
        public void Convert_String_ToSnakeCase()
        {
            var input = "CamelCaseString";
            var expected = "camel_case_string";

            var result = input.ToSnakeCase();
            Assert.Equal(expected, result);
        }

        [Fact]
        public void LimitString_Length()
        {
            var input = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras vel fringilla arcu";
            var expected = "Lorem ipsum";

            var result = input.Truncate(11);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Remove_Html_Tags_From_String()
        {
            var input = @"<p>Sinds 25 mei zijn de <a href=""testurl.html"">privacyrechtens</a> van mensen versterkt en kan iedereen een privacyklacht indienen bij de Autoriteit Persoonsgegevens <span><a href=""#"">De AP</a></span>.</p>";
            var expected = "Sinds 25 mei zijn de privacyrechtens van mensen versterkt en kan iedereen een privacyklacht indienen bij de Autoriteit Persoonsgegevens De AP";

            var result = input.HtmlToPlainText();
            Assert.Equal(expected, result);
        }
    }
}
