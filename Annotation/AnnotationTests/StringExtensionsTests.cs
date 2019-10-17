
using System;
using System.Collections.Generic;
using Microsoft.OData.EdmUtils.Commons;
using Xunit;

namespace AnnotationGenerator.Tests
{
    public class StringExtensionsTests
    {
        #region ExtractParenthesis
        [Fact]
        public void ExtractParenthesisThrowsForInvalidInput()
        {
            string input = "abc(xyz}";

            Action test = () => input.ExtractParenthesis(out string parenthesisExpressions);

            var exception = Assert.Throws<Exception>(test);
            Assert.Equal($"Invalid identifier {input}, can't find the end character ')'", exception.Message);
        }

        [Fact]
        public void ExtractParenthesisWorksForNormalInput()
        {
            string input = "abc({value1},{value2})";

            string identifier = input.ExtractParenthesis(out string parenthesisExpressions);

            Assert.Equal("abc", identifier);
            Assert.Equal("({value1},{value2})", parenthesisExpressions);
        }

        #endregion

        [Fact]
        public void HasMultipleEmptyKeyPairsThrows()
        {
            string input = "({value1},{value2})";
            Action test = () => input.ExtractKeyValuePairs(out IDictionary<string, string> pairs, out string remaining);

            var exception = Assert.Throws<Exception>(test);
            Assert.Equal($"Invalid string '{input}', has multiple items without '='.", exception.Message);
        }

        [Theory]
        [InlineData("(={value})", "={value}")]
        [InlineData("(key1={value1},={value})", "={value}")]
        public void HasEmptyKeyValuePairsThrows(string input, string item)
        {
            Action test = () => input.ExtractKeyValuePairs(out IDictionary<string, string> pairs, out string remaining);

            var exception = Assert.Throws<Exception>(test);
            Assert.Equal($"Invalid string '{input}', has empty key in '{item}'.", exception.Message);
        }

        [Fact]
        public void ExtractNoramlKeyValuePairsWorks()
        {
            string input = "(key1={value1})";
            input.ExtractKeyValuePairs(out IDictionary<string, string> pairs, out string remaining);

            Assert.NotNull(pairs);
            var item = Assert.Single(pairs);
            Assert.Equal("key1", item.Key);
            Assert.Equal("{value1}", item.Value);
            Assert.Null(remaining);
        }

        [Fact]
        public void ExtractMultipleKeyValuesPairsWorks()
        {
            string input = "(key1={value1},key2={value2},key3=value3)";
            input.ExtractKeyValuePairs(out IDictionary<string, string> pairs, out string remaining);

            Assert.NotNull(pairs);
            Assert.Equal(3, pairs.Count);
            Assert.Equal(new[] { "key1", "key2", "key3" }, pairs.Keys);
            Assert.Equal(new[] { "{value1}", "{value2}", "value3" }, pairs.Values);
            Assert.Null(remaining);
        }

        [Fact]
        public void ExtractCompositeKeyValuesPairsWorks()
        {
            string input = "(key1={value1})({id})";
            input.ExtractKeyValuePairs(out IDictionary<string, string> pairs, out string remaining);

            Assert.NotNull(pairs);
            var item = Assert.Single(pairs);
            Assert.Equal("key1", item.Key);
            Assert.Equal("{value1}", item.Value);

            Assert.NotNull(remaining);
            Assert.Equal("({id})", remaining);
        }
    }
}
