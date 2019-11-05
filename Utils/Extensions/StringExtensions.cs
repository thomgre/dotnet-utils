using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Utils.Extensions
{
    public static class StringExtensions
    {
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static string RemoveWhitespace(this string value)
        {
            if (value == null)
            {
                return value;
            }
            return Regex.Replace(value, @"\s+", "");
        }

        public static string Concat(this string[] value, string separator = ",")
        {
            if (value == null)
            {
                return "";
            }
            return string.Join(separator, value);
        }

        public static string TitleCase(this string value, string cultureName = "nl-NL")
        {
            if (value == null)
            {
                return value;
            }
            TextInfo textInfo = new CultureInfo(cultureName, false).TextInfo;
            // Extra ToLower() needed because all caps is considered an acronym.
            return textInfo.ToTitleCase(value.ToLower());
        }

        public static string StripHTML(this string value)
        {
            return string.IsNullOrEmpty(value) ? value : Regex.Replace(value, "<.*?>", string.Empty).Trim();
        }

        public static string Left(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            maxLength = Math.Abs(maxLength);

            return value.Length <= maxLength
                   ? value
                   : value.Substring(0, maxLength);
        }

        public static string Right(this string value, int length)
        {
            return value.Substring(value.Length - length);
        }

        public static string PlainTextTruncate(this string input, int length)
        {
            string text = HtmlToPlainText(input);
            if (text.Length < length)
            {
                return text;
            }

            char[] terminators = { '.', ',', ';', ':', '?', '!' };
            int end = text.LastIndexOfAny(terminators, length);
            if (end == -1)
            {
                end = text.LastIndexOf(" ", length);
                return text.Substring(0, end) + "...";
            }
            return text.Substring(0, end + 1);
        }

        //From https://stackoverflow.com/a/16407272/5
        //TODO: Use a proper sanitizer, perhaps https://github.com/atifaziz/High5
        public static string HtmlToPlainText(this string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);

            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);

            return text;
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string GetTenantId(this string str)
        {
            return string.IsNullOrEmpty(str) ? str : str.Substring(0, 3);
        }
    }
}
