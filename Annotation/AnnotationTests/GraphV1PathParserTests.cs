
using System;
using System.Collections.Generic;
using Annotation;
using Annotation.EdmUtil;
using Microsoft.OData.Edm;
using Xunit;

namespace AnnotationGenerator.Tests
{
    public class GraphV1PathParserTests
    {
        private IEdmModel _edmModel;

        public GraphV1PathParserTests()
        {
            _edmModel = EdmModelHelper.LoadEdmModel("graphV1.0.xml");
            Assert.NotNull(_edmModel);
        }

        [Fact]
        public void ParseEntitySetWorks()
        {
            string requestUri = "users";
            var segments = PathParser.Parse(requestUri, _edmModel);

            Assert.NotNull(segments);
            var pathSegment = Assert.Single<PathSegment>(segments);
            var entitySetSegment = Assert.IsType<EntitySetSegment>(pathSegment);

   //         Assert.Same(entitySetSegment.EntitySet, _users);
        }

        [Theory]
        [InlineData("/me/contacts", 2)]
        [InlineData("/users/{id | userPrincipalName}/contacts", 3)]
        [InlineData("/users/{id|userPrincipalName}/mailboxSettings/timeZone", 4)]
        [InlineData("/users/{id|userPrincipalName}/outlook/supportedLanguages", 4)]
        [InlineData("/me/outlook/supportedTimeZones(TimeZoneStandard=microsoft.graph.timeZoneStandard'{timezone_format}')", 3)]
        [InlineData("/users/{id|userPrincipalName}/outlook/supportedTimeZones(TimeZoneStandard=microsoft.graph.timeZoneStandard'{timezone_format}')", 4)]
        [InlineData("/drives/{drive-id}/items/{item-id}/permissions/{perm-id}", 6)]
        public void ParseGraphV1RequestUriWorks(string requestUri, int count)
        {
            var segments = PathParser.Parse(requestUri, _edmModel);

            Assert.NotNull(segments);
            Assert.Equal(count, segments.Count);
        }

        [Fact]
        public void ParseGraphV1RequestUriWithKeysWorks()
        {
            var segments = PathParser.Parse("/drives/{drive-id}/items/{item-id}/permissions/{perm-id}", _edmModel);

            Assert.NotNull(segments);
            Assert.Equal(6, segments.Count);
        }

        [Fact]
        public void ParseGraphV1RequestUriWithKeysWorks2()
        {
            var segments = PathParser.Parse("/me/calendar/{id}/events/{id}/attachments/{id}", _edmModel);

            Assert.NotNull(segments);
            Assert.Equal(6, segments.Count);
        }

        [Fact]
        public void ParseGraphV1RequestUriWithKeysWorks3()
        {
            var segments = PathParser.ParsePath("/me/calendar", _edmModel);

            Assert.NotNull(segments);
            Assert.Equal(2, segments.Count);
            Assert.Equal(PathKind.SingleNavigation, segments.Kind);
        }
    }
}
