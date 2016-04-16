using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PseudoJsonToJson
{
    public class JsonAmbulance
    {
        public string FixSimpleJson(string json)
        {
            if (IsValidJson(json))
                return json;
            GetNameValuePair(json.Trim());
            throw new NotImplementedException();

            // Inny pomysł - czytanie wiadomości wyjątku z JObject.Parse() i rekursywne poprawienie całego json'a...
        }

        public static bool IsValidJson(string s)
        {
            try
            {
                JObject.Parse(s);
            }
            catch (JsonReaderException)
            {
                return false;
            }
            return true;
        }

        private void GetNameValuePair(string s)
        {
            var pattern = "\"[^\"]+?\"[^\"]*?:";
            var regex = new Regex(pattern);
            var matches = regex.Matches(s);
            var indexesToFix = new Collection<int>();

            foreach (Match match in matches)
            {
                var startIndex = match.Index + match.Length;
                int valueLength;
                var nextMatch = match.NextMatch();
                if (nextMatch.Success)
                    valueLength = nextMatch.Index - startIndex;
                else
                    valueLength = s.Length - startIndex - 1; // -1, as we want to ignore the last '}'
                var value = s.Substring(startIndex, valueLength);
                if (IsValidJson("{" + match.Value + value + "}"))
                    continue;
                var valueBorders = GetValueBorders(value);
                indexesToFix.Add(startIndex + valueBorders[0]);
                indexesToFix.Add(startIndex + valueBorders[1]);
            }

        }

        private static int[] GetValueBorders(string value)
        {
            // Case of: |"value"|
            if (value.StartsWith("\"") && value.EndsWith("\""))
                return new[] {0, value.Length - 1};

            // Case of: | "value", |
            var firstQuotationMarkIndex = value.IndexOf("\"", StringComparison.InvariantCulture);
            if (firstQuotationMarkIndex >= 0)
            {
                var lastQuotationMarkIndex = value.LastIndexOf("\"", StringComparison.InvariantCulture);
                return new[] {firstQuotationMarkIndex, lastQuotationMarkIndex};
            }

            // Case of: | value, |
            // Case of: | ,  |
            var lastColonIndex = value.LastIndexOf(",", StringComparison.InvariantCulture);
            var firstNonWhiteSpaceIndex = value.IndexOf(c => !char.IsWhiteSpace(c));
            var lastNonWhiteSpaceIndex = value.LastIndexOf(c => !char.IsWhiteSpace(c));
            if (lastColonIndex >= 0)
            {
                // Case of: | value1, value2  |
                if (lastColonIndex == lastNonWhiteSpaceIndex)
                {
                    return new[] {firstNonWhiteSpaceIndex, lastColonIndex - 1};
                }
            }

            // Case of: | value  |
            if (lastNonWhiteSpaceIndex >= 0)
            {
                return new[] {firstNonWhiteSpaceIndex, lastNonWhiteSpaceIndex};
            }

            // Case of: |        |
            return new[] {0, 0};
        }

        private static string ValueCleanUp(string value)
        {
            // Case of: |"value"|
            if (value.StartsWith("\"") && value.EndsWith("\""))
                return value;
            int length;
            // Case of: | "value", |
            var firstQuotationMarkIndex = value.IndexOf("\"", StringComparison.InvariantCulture);
            if (firstQuotationMarkIndex >= 0)
            {
                var lastQuotationMarkIndex = value.LastIndexOf("\"", StringComparison.InvariantCulture);
                length = lastQuotationMarkIndex + 1 - firstQuotationMarkIndex;
                return value.Substring(firstQuotationMarkIndex, length);
            }

            // Case of: | value, |
            // Case of: | ,  |
            var lastColonIndex = value.LastIndexOf(",", StringComparison.InvariantCulture);
            var firstNonWhiteSpaceIndex = value.IndexOf(c => !char.IsWhiteSpace(c));
            var lastNonWhiteSpaceIndex = value.LastIndexOf(c => !char.IsWhiteSpace(c));
            if (lastColonIndex >= 0)
            {
                // Case of: | value1, value2  |
                length = lastColonIndex - firstNonWhiteSpaceIndex;
                if (lastColonIndex == lastNonWhiteSpaceIndex)
                {
                    return value.Substring(firstNonWhiteSpaceIndex, length);
                }
            }

            // Case of: | value  |
            if (lastNonWhiteSpaceIndex >= 0)
            {
                length = lastNonWhiteSpaceIndex + 1 - firstNonWhiteSpaceIndex;
                return value.Substring(firstNonWhiteSpaceIndex, length);
            }

            // Case of: |        |
            return "";
        }

        private struct NameValuePair
        {
            public readonly string Name;
            public readonly string Value;

            public NameValuePair(string name, string value)
            {
                Name = name;
                Value = value;
            }
        }
    }

    public static class StringExtention
    {
        public static int IndexOf<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            var i = 0;

            foreach (var element in source)
            {
                if (predicate(element))
                    return i;

                i++;
            }

            return -1;
        }

        public static int LastIndexOf<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate)
        {
            var i = source.Count() - 1;

            foreach (var element in source.Reverse())
            {
                if (predicate(element))
                    return i;

                i--;
            }

            return -1;
        }
    }
}