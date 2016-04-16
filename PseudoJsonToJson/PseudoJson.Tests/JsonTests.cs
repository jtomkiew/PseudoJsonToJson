using NUnit.Framework;
using JsonFixer;

namespace PseudoJson.Tests
{
    [TestFixture]
    public class JsonTests
    {
        public const string ValidJson1 = "{ \"_id\": \"570fe32cebf5a9aca7e8ee54\","
                                         + "\"index\": 0,"
                                         + "\"guid\": \"a5558bd4-cad7-40cd-92d0-785d1f35bb15\","
                                         + "\"isActive\": false,"
                                         + "\"balance\": \"$1,324.92\","
                                         + "\"picture\": \"http://placehold.it/32x32\","
                                         + "\"age\": 30,"
                                         + "\"eyeColor\": \"green\","
                                         + "\"name\": \"Bridgette Miranda\" }";

        public const string InvalidJson1 = "{ \"_id\": \"570fe32cebf5a9aca7e8ee54\","
                                           + "\"index\": 0,"
                                           + "\"guid\": \"a5558bd4-cad7-40cd-92d0-785d1f35bb15\","
                                           + "\"isActive\": false,"
                                           + "\"balance\": $1,324.92,"
                                           + "\"picture\": \"http://placehold.it/32x32\","
                                           + "\"age\": 30,"
                                           + "\"eyeColor\": green,"
                                           + "\"name\": \"Bridgette Miranda\" }";

        public const string ValidJson2 = "{"
                                         + "	\"name1\":\"value1\","
                                         + "	\"name2\":\"value2\","
                                         + "	\"name3\":\"value3\","
                                         + "	\"name4\":\"value4\","
                                         + "	\"name5\":\"value5\","
                                         + "	\"name6\":\"value6\","
                                         + "	\"name7\":\"value7\","
                                         + "	\"name8\":\"value8\","
                                         + "	\"name9\":\"value9\""
                                         + "}";

        public const string ValidJson3 = "{"
                                         + "	\"name1\":\"value1\","
                                         + "	\"name2\" :  \"value2\", "
                                         + "	\"name3\":  \"value3\" ,"
                                         + "	\"name4\" :  \"value4\","
                                         + "	\"name5\":  ,"
                                         + "	\"name6\" :  \"value6, value7\","
                                         + "	\"name7\":  \"value7\","
                                         + "	\"name8\" :  \"valu,,,e8\","
                                         + "	\"name9\":  \"value9, value10\""
                                         + "}";

        public const string InvalidJson3 = "{"
                                           + "	\"name1\":\"value1\","
                                           + "	\"name2\" :  \"value2\", "
                                           + "	\"name3\":  \"value3\" ,"
                                           + "	\"name4\" :  value4,"
                                           + "	\"name5\":  ,"
                                           + "	\"name6\" :  value6, value7,"
                                           + "	\"name7\":  \"value7\","
                                           + "	\"name8\" :  valu,,,e8,"
                                           + "	\"name9\":  value9, value10"
                                           + "}";

        public const string ValidJson4 = "{"
                                   + "	\"name\":\"value\""
                                   + "}";

        public const string InvalidJson4 = "{"
                                           + "	\"name\":value"
                                           + "}";

        [Test]
        public void CheckValidJson1()
        {
            var fixer = new JsonFixer.JsonFixer();
            var fixedJson = fixer.FixJsonString(ValidJson1);
            Assert.AreEqual(ValidJson1, fixedJson);
        }

        [Test]
        public void CheckInvalidJson1()
        {
            var fixer = new JsonFixer.JsonFixer();
            var fixedJson = fixer.FixJsonString(InvalidJson1);
            Assert.AreNotEqual(InvalidJson1, fixedJson);
            Assert.AreEqual(ValidJson1, fixedJson);
        }

        [Test]
        public void CheckValidJson2()
        {
            var fixer = new JsonFixer.JsonFixer();
            var fixedJson = fixer.FixJsonString(ValidJson2);
            Assert.AreEqual(ValidJson2, fixedJson);
        }


        [Test]
        public void CheckValidJson3()
        {
            var fixer = new JsonFixer.JsonFixer();
            var fixedJson = fixer.FixJsonString(ValidJson3);
            Assert.AreEqual(ValidJson3, fixedJson);
        }


        [Test]
        public void CheckInvalidJson3()
        {
            var fixer = new JsonFixer.JsonFixer();
            var fixedJson = fixer.FixJsonString(InvalidJson3);
            Assert.AreNotEqual(InvalidJson3, fixedJson);
            Assert.AreEqual(ValidJson3, fixedJson);
        }


        [Test]
        public void CheckValidJson4()
        {
            var fixer = new JsonFixer.JsonFixer();
            var fixedJson = fixer.FixJsonString(ValidJson4);
            Assert.AreEqual(ValidJson4, fixedJson);
        }


        [Test]
        public void CheckInvalidJson4()
        {
            var fixer = new JsonFixer.JsonFixer();
            var fixedJson = fixer.FixJsonString(InvalidJson4);
            Assert.AreNotEqual(InvalidJson4, fixedJson);
            Assert.AreEqual(ValidJson4, fixedJson);
        }

        [Test]
        public void IndexOfExtentionTests()
        {
            var whiteSpaces = " \r\n  ";
            var testString = "asd fg hjkl";
            var s = whiteSpaces + testString + whiteSpaces;
            var index = whiteSpaces.Length;
            Assert.AreEqual(index, s.IndexOf(c => !char.IsWhiteSpace(c)));
            Assert.AreEqual(testString[0], s[index]);

            Assert.AreEqual(0, testString.IndexOf(c => !char.IsWhiteSpace(c)));
        }

        [Test]
        public void LastIndexOfExtentionTests()
        {
            var whiteSpaces = " \r\n  ";
            var testString = "asd fg hjkl";
            var s = whiteSpaces + testString + whiteSpaces;
            var index = whiteSpaces.Length + testString.Length - 1;
            Assert.AreEqual(index, s.LastIndexOf(c => !char.IsWhiteSpace(c)));
            Assert.AreEqual(testString[testString.Length - 1], s[index]);

            Assert.AreEqual(testString.Length - 1, testString.LastIndexOf(c => !char.IsWhiteSpace(c)));
        }

        [Test]
        public void CheckJsons()
        {
            var okJson = "{\"name\":\"value\"}";
            Assert.IsTrue(JsonFixer.JsonFixer.IsValidJson(okJson));

            var maybeJson = "{\"name\":\"va\"lue\"}";
            Assert.IsFalse(JsonFixer.JsonFixer.IsValidJson(maybeJson));

            var colonJson = "{\"name\":\"value\",}";
            Assert.IsTrue(JsonFixer.JsonFixer.IsValidJson(colonJson));

            var doubleColonJson = "{\"name\":\"value\",,}";
            Assert.IsFalse(JsonFixer.JsonFixer.IsValidJson(doubleColonJson));

            var newLineJson = "{\"name\":\"value\"} \r\n";
            Assert.IsTrue(JsonFixer.JsonFixer.IsValidJson(newLineJson));
        }
    }
}