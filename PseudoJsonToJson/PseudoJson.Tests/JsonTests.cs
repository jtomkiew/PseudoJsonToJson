using NUnit.Framework;
using PseudoJsonToJson;

namespace PseudoJson.Tests
{
    [TestFixture]
    public class JsonTests
    {
        public const string SmallGoodJson = "{ \"_id\": \"570fe32cebf5a9aca7e8ee54\","
                                            + "\"index\": 0,"
                                            + "\"guid\": \"a5558bd4-cad7-40cd-92d0-785d1f35bb15\","
                                            + "\"isActive\": false,"
                                            + "\"balance\": \"$1,324.92\","
                                            + "\"picture\": \"http://placehold.it/32x32\","
                                            + "\"age\": 30,"
                                            + "\"eyeColor\": \"green\","
                                            + "\"name\": \"Bridgette Miranda\" }";

        public const string SmallBadJson = "{ \"_id\": \"570fe32cebf5a9aca7e8ee54\","
                                           + "\"index\": 0,"
                                           + "\"guid\": \"a5558bd4-cad7-40cd-92d0-785d1f35bb15\","
                                           + "\"isActive\": false,"
                                           + "\"balance\": $1,324.92,"
                                           + "\"picture\": \"http://placehold.it/32x32\","
                                           + "\"age\": 30,"
                                           + "\"eyeColor\": green,"
                                           + "\"name\": \"Bridgette Miranda\" }";

        [Test]
        public void CheckGoodSmallJsonString()
        {
            var ja = new JsonAmbulance();
            var fixedJson = ja.FixSimpleJson(SmallGoodJson);
            Assert.AreEqual(SmallGoodJson, fixedJson);
        }

        [Test]
        public void FixBadSmallJsonString()
        {
            var ja = new JsonAmbulance();
            var fixedJson = ja.FixSimpleJson(SmallBadJson);
            Assert.AreNotEqual(SmallBadJson, fixedJson);
            Assert.AreEqual(SmallGoodJson, fixedJson);
        }

        [Test]
        public void CheckJsons()
        {
            var okJson = "{\"name\":\"value\"}";
            Assert.IsTrue(JsonAmbulance.IsValidJson(okJson));

            var maybeJson = "{\"name\":\"va\"lue\"}";
            Assert.IsFalse(JsonAmbulance.IsValidJson(maybeJson));

            var colonJson = "{\"name\":\"value\",}";
            Assert.IsTrue(JsonAmbulance.IsValidJson(colonJson));

            var doubleColonJson = "{\"name\":\"value\",,}";
            Assert.IsFalse(JsonAmbulance.IsValidJson(doubleColonJson));

            var newLineJson = "{\"name\":\"value\"} \r\n";
            Assert.IsTrue(JsonAmbulance.IsValidJson(newLineJson));
        }

        [Test]
        public void TestTrimmer()
        {
            var message = "This is what I want";
            var testString = "   \n " + message + "  \r\n ";
            var trimmedString = testString.Trim();
            Assert.AreEqual(message, trimmedString);
        }

        [Test]
        public void IndexOfExtentionTests()
        {
            var whiteSpaces = " \r\n  ";
            var testString = "asd fg hjkl";
            var s = whiteSpaces + testString + whiteSpaces;
            var index = whiteSpaces.Length;
            Assert.AreEqual(index, s.IndexOf<char>(c => !char.IsWhiteSpace(c)));
            Assert.AreEqual(testString[0], s[index]);
        }

        [Test]
        public void LastIndexOfExtentionTests()
        {
            var whiteSpaces = " \r\n  ";
            var testString = "asd fg hjkl";
            var s = whiteSpaces + testString + whiteSpaces;
            var index = whiteSpaces.Length + testString.Length - 1;
            Assert.AreEqual(index, s.LastIndexOf<char>(c => !char.IsWhiteSpace(c)));
            Assert.AreEqual(testString[testString.Length - 1], s[index]);
        }
    }
}