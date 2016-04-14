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
                                              + "\"name\": \"Bridgette Miranda\", }";

        public const string SmallBadJson = "{ \"_id\": \"570fe32cebf5a9aca7e8ee54\","
                                           + "\"index\": 0,"
                                           + "\"guid\": \"a5558bd4-cad7-40cd-92d0-785d1f35bb15\","
                                           + "\"isActive\": false,"
                                           + "\"balance\": $1,324.92,"
                                           + "\"picture\": \"http://placehold.it/32x32\","
                                           + "\"age\": 30,"
                                           + "\"eyeColor\": green,"
                                           + "\"name\": \"Bridgette Miranda\", }";

        [Test]
        public void CheckGoodSmallJsonString()
        {
            var ja = new JsonAmbulance();
            var fixedJson = ja.FixJsonString(SmallGoodJson);
            Assert.AreEqual(SmallGoodJson, fixedJson);
        }

        [Test]
        public void FixBadSmallJsonString()
        {
            var ja = new JsonAmbulance();
            var fixedJson = ja.FixJsonString(SmallBadJson);
            Assert.AreNotEqual(SmallBadJson, fixedJson);
            Assert.AreEqual(SmallGoodJson, fixedJson);
        }
    }
}