using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonFixer
{
    public class JsonFixer
    {
        public string FixJsonString(string s)
        {
            if (IsValidJson(s))
                return s;
            var json = s.Trim();
            var indexesToFix = GetIndexesToFix(json);
            var fixedJson = InsertQuotationMarks(json, indexesToFix);
            if (!IsValidJson(fixedJson))
            {
                var message = String.Format("Source string:\n{0}.\nAttempted fix:\n{1}", json, fixedJson);
                throw new FailedToFixJsonException(message);
            }
            return fixedJson;
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

        private Collection<int> GetIndexesToFix(string s)
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
                indexesToFix.Add(startIndex + valueBorders[1] + 1);
            }
            return indexesToFix;
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

        private string InsertQuotationMarks(string json, Collection<int> indexes)
        {
            //var count = 0;
            if (!indexes.Any())
                return json;
            if (indexes.Last() > json.Length - 1)
                throw new IndexOutOfRangeException();
            foreach (var toFix in indexes.Reverse())
            {
                json = json.Insert(toFix, "\"");
            }
            return json;
        }
    }
}