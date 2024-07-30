using System;
using Xunit;
using JSONParser;

namespace JSONParser.Tests
{
    public class JsonParserTests
    {
        [Fact]
        public void ParseSimpleObject()
        {
            string json = "{\"name\":\"John\",\"age\":30,\"isActive\":true}";
            var parser = new JsonParser();
            var result = parser.Parse(json);

            Assert.Equal("John", result["name"]);
            Assert.Equal(30, result["age"]);
            Assert.True((bool)result["isActive"]);
        }

        [Fact]
        public void ParseNestedObject()
        {
            string json = "{\"person\":{\"name\":\"John\",\"age\":30},\"isActive\":true}";
            var parser = new JsonParser();
            var result = parser.Parse(json);

            var person = (Dictionary<string, object>)result["person"];
            Assert.Equal("John", person["name"]);
            Assert.Equal(30, person["age"]);
            Assert.True((bool)result["isActive"]);
        }

        [Fact]
        public void ParseArray()
        {
            string json = "{\"names\":[\"John\",\"Jane\",\"Bob\"]}";
            var parser = new JsonParser();
            var result = parser.Parse(json);

            var names = (System.Collections.Generic.List<object>)result["names"];
            Assert.Equal("John", names[0]);
            Assert.Equal("Jane", names[1]);
            Assert.Equal("Bob", names[2]);
        }

        [Fact]
        public void ParseNull()
        {
            string json = "{\"name\":null}";
            var parser = new JsonParser();
            var result = parser.Parse(json);

            Assert.Null(result["name"]);
        }

        [Fact]
        public void ParseEscapedString()
        {
            string json = "{\"message\":\"Hello, \\\"world\\\"!\"}";
            var parser = new JsonParser();
            var result = parser.Parse(json);

            Assert.Equal("Hello, \"world\"!", result["message"]);
        }

        [Fact]
        public void ThrowsExceptionOnInvalidJson()
        {
            string json = "{\"name\":\"John\",\"age\":30,\"isActive\":true,}";
            var parser = new JsonParser();

            Assert.Throws<Exception>(() => parser.Parse(json));
        }
    }
}